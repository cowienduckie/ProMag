#!/usr/bin/env bash

usage() {
  cat <<END
deploy.sh: deploys the application to a Kubernetes cluster using Helm.
Parameters:
  --help | -h
  --build-solution | -b
    Force build solution before deploy (default: false)
  --tag | -t
    The tag used for the newly created docker images.
  --registry | -r
    Specifies the container registry to use (required)
  --image-build
    Build docker image
  --image-push
    Push docker image to registry
  --skip-clean
    Do not clean the Kubernetes cluster (default is to clean the cluster).
  --namespace
    Specifies the namespace name to deploy the app (Default: local)
  --skip-infrastructure
    Do not deploy infrastructure resources
  --skip-service
    Do not deploy service resources
END
}

clean='yes'
skip_infrastructure=''
skip_service=''
build_solution=''
build_images=''
push_images=''
namespace='local'
value_file='values.local.yaml'
image_tag='local'
container_registry='localhost:32000'

while [[ $# -gt 0 ]]; do
  case "$1" in
    -h | --help)
      usage; exit 1;;
    -b | --build-solution )
      build_solution="yes"; shift;;
    -t | --tag )
      image_tag="$2"; shift 2;;
    -r | --registry )
      container_registry="$2"; shift 2;;
    --image-build )
      build_images='yes'; shift ;;
    --image-push )
      push_images='yes'; shift ;;
    --skip-clean )
      clean=''; shift ;;
    --namespace )
      namespace="$2"; shift 2;;
    --value-file )
      value_file="$2"; shift 2;;
    --skip-infrastructure)
      skip_infrastructure='yes'; shift ;;
    --skip-service)
      skip_service='yes'; shift ;;
    *)
      echo "Unknown option $1"
      usage
      exit 2
      ;;
  esac
done

export TAG=$image_tag
export REGISTRY=$container_registry

if [[ $build_solution ]]; then
  echo "#################### Building solution ####################"
  pwd
  dotnet build Promag.sln
fi


if [[ $build_images ]]; then
  echo "#################### Building the Docker images ####################"
  docker-compose -f docker-compose.yml -f docker-compose.override.yml build

  # Remove temporary images
  docker rmi $(docker images -qf "dangling=true")
fi

if [[ $push_images ]]; then
  echo "#################### Pushing images to the container registry ####################"
  services=(portal-api identity-api communication-api personal-data-api master-data-api graph-gateway service-status)

  for service in "${services[@]}"
  do
    echo "Pushing image for service $service..."
    docker push "$container_registry/$service:$image_tag"
  done
fi

if [[ -n $clean ]]; then
  echo "Cleaning previous helm releases..."
  if [[ -z $(helm ls -q --namespace $namespace) ]]; then
    echo "No previous releases found"
  else
    helm uninstall --namespace $namespace $(helm ls -q --namespace $namespace)
    echo "Previous releases deleted"
    waitsecs=10; while [ $waitsecs -gt 0 ]; do echo -ne "$waitsecs\033[0K\r"; sleep 1; : $((waitsecs--)); done
  fi
fi

echo "#################### Begin $app_name installation using Helm ####################"

if [[ -z $skip_infrastructure ]]; then
  helm repo add stable https://charts.helm.sh/stable
  helm repo add bitnami https://charts.bitnami.com/bitnami
  helm repo add codecentric https://codecentric.github.io/helm-charts

  echo "Installing secret"
  helm upgrade --install --namespace $namespace -f k8s/charts/secret/$value_file secret k8s/charts/secret
  echo "Installing pvc"
  helm upgrade --install --namespace $namespace -f k8s/charts/pvc/$value_file pvc k8s/charts/pvc
  echo "Install RabbitMQ"
  helm upgrade --install --namespace $namespace rabbitmq -f k8s/charts/rabbitmq/$value_file bitnami/rabbitmq
  echo "Install mailhog"
  helm upgrade --install --namespace $namespace mailhog -f k8s/charts/mailhog/$value_file codecentric/mailhog
  echo "Install postgresql"
  helm upgrade --install --namespace $namespace postgresql -f k8s/charts/postgresql/$value_file bitnami/postgresql

  waitsecs=20; while [ $waitsecs -gt 0 ]; do echo -ne "$waitsecs\033[0K\r"; sleep 1; : $((waitsecs--)); done
fi

if [[ -z $skip_service ]]; then
  charts=(communication-api identity-api  master-data-api personal-data-api portal-api  graph-gateway web-apigw)

  for chart in "${charts[@]}"
  do
      echo "Installing: $chart"
      helm upgrade --install --namespace $namespace -f k8s/charts/$chart/$value_file $chart k8s/charts/$chart
  done
fi

echo "FINISHED."
