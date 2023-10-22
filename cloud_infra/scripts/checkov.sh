#!/bin/bash

set -a # Enable automatic exporting of variables

BRANCH_NAME=$1

# Change to the 'main_logic' directory
cd main

echo "Generating terraform plan"
terraform init -upgrade
terraform plan -var="env=$ENV" -var="key_file_path=$KEY_FILE" -out tf.plan || exit 1

terraform show -json tf.plan > tf.json || exit 1

echo "Running checkov"
checkov -f tf.json || exit 1

echo "Checkov completed successfully"
