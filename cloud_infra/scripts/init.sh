export PROJECT_ID=k8s-labs-fa VPC_NAME=minhph-vpc VPC_SUBNET_NAME=minhph-vpc-sub1 REGION=us-central1 ZONE=us-central1-a

stages:
  - checkov
  - deploy

checkov:
  stage: checkov
  script:
    - |
      export BRANCH_NAME=$(echo $CI_COMMIT_REF_NAME | sed 's/\//-/g') &&
      echo "Branch Name: $BRANCH_NAME" &&
      ./checkov.sh $BRANCH_NAME
      
deploy:
  stage: deploy
  script:
    - export PATH=/opt/homebrew/bin:$PATH
    - terraform --version
    - |
      export BRANCH_NAME=$(echo $CI_COMMIT_REF_NAME | sed 's/\//-/g') &&
      echo "Branch Name: $BRANCH_NAME" &&
      ./run.sh $BRANCH_NAME
  only:
    - branches
  except:
    - tags
  when: manual
