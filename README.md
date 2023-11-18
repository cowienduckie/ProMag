# ProMag - A Project Management Web Application

[![wakatime](https://wakatime.com/badge/github/cowienduckie/promag.svg)](https://wakatime.com/badge/github/cowienduckie/promag)

## Introduction

**ProMag** is the graduated project for my engineering degree. It is a web application that helps members in a project team can easily to collaborate and manage personal tasks, and the project's progress too!

The project is built with microservice architecture using .NET 8 and ReactJS. In addition, it can run locally with Docker or any Kubernetes cluster, or cloud services. Moreover, it also provide monitoring tools for tracing logging and metrics, CI/CD pipelines for building and testing.

## Technical Stacks

- .NET 8
- GraphQL with [HotChocolate](https://chillicream.com/products/hotchocolate) using as a GraphQL Server and Gateway
- Inter-service communication with [gRPC](https://grpc.io/)
- Message Broker with [RabbitMQ](https://www.rabbitmq.com/) and [MassTransit](https://masstransit.io/)
- SQL Database with [PostgreSQL](https://www.postgresql.org/)
- Secrets Management with [Vault](https://www.vaultproject.io/)
- Accessing database with [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- In-process messaging with [MediatR](https://github.com/jbogard/MediatR)
- Logging with [Serilog](https://serilog.net/), [Seq](https://datalust.co/seq)
- OAuth2 and OIDC implementation with [Duende IdentityServer](https://duendesoftware.com/products/identityserver)
- Containerization and manage them with [Docker](https://www.docker.com/), [Docker Compose](https://docs.docker.com/compose/), [Kubernetes](https://kubernetes.io/), [Helm](https://helm.sh/)

- Frontend with [ReactJS](https://reactjs.org/), [Apollo Client](https://www.apollographql.com/docs/react/), [Ant Design](https://ant.design/)

## Local Development

### Prepare For Development

```SHELL
npm install
dotnet build Promag.sln
```

### Docker Compose

From the root directory `promag` of the project, run the following command:

```SHELL
docker compose -f docker-compose.yml -f docker-compose.override.yml -p promag up -d --build
```

### Kubernetes

#### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Docker Compose](https://docs.docker.com/compose/)
- [Kubernetes](https://kubernetes.io/)
- [Helm](https://helm.sh/)
- [Lens](https://k8slens.dev/) (Optional)

#### Prepare Steps

**Step 1.** Enable **Kubernetes** in Docker Desktop and enable `dns`, `helm3`, `registry`, `storage`

**Step 2.** Get into the `promag` directory:

```SHELL
cd <path-to-promag-directory>
```

**Step 3.** Create a namespace for the project named `local`:

```SHELL
kubectl create namespace local
```

**Step 4.** Run a local registry using this command at port `32000`:

```SHELL
docker run -d -p 32000:5000 --restart=always --name registry registry:2
```

#### Deploy Steps

**Step 1.** Build the images:

```SHELL
sh ./deploy-all.sh --image-build --skip-infrastructure --skip-service
```

**Step 2.** Push the images to the local registry:

```SHELL
sh ./deploy-all.sh --image-push --skip-clean --skip-service --skip-infrastructure
```

**Step 3.** Deploy the infrastructure using helm for `secret`, `pvc`, `rabbitmq`, `mailhog`, `postgresql`:

```SHELL
sh ./deploy-all.sh --skip-clean --skip-service
```

**Step 4.** Deploy the services, including `identity server`, `graphql gateway`, `api gateway`, `masterdata api`, `personaldata api`, `portal api`, `communication`, `services status`:

```SHELL
sh ./deploy-all.sh --skip-clean --skip-infrastructure
```

**Step 5.** Checking the status of the components:

```SHELL
kubectl get pods --namespace=local
kubectl get services --namespace=local
```

After all the pods are running, you can access the services via the following URLs:

- GraphQL Gateway: [http://localhost:31100/graphql](http://localhost:31100/graphql)
- Identity Server: [http://localhost:31101](http://localhost:31101)
