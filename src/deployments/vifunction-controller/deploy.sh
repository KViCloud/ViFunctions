#!/bin/bash

# Constants
RELEASE_NAME="vifunction"
CHART_PATH="./vifunction-controller"
# Function to print usage
print_usage() {
  echo "Usage: $0 [environment]"
  echo "environment: local | staging | production"
}

# Check if environment is passed
if [ -z "$1" ]; then
  echo "Error: environment not specified."
  print_usage
  exit 1
fi

ENV=$1

# Set values file based on environment
case $ENV in
  local)
    VALUES_FILE="values-local.yaml"
    ;;
  staging)
    VALUES_FILE="values-staging.yaml"
    ;;
  production)
    VALUES_FILE="values.yaml"  # You can create values-production.yaml if needed
    ;;
  *)
    echo "Error: invalid environment specified."
    print_usage
    exit 1
    ;;
esac

# Deploy the Helm chart
echo "Deploying Helm chart for $ENV environment using $VALUES_FILE..."
helm upgrade --install $RELEASE_NAME $CHART_PATH -f $CHART_PATH/$VALUES_FILE

if [ $? -ne 0 ]; then
  echo "Deployment failed!"
  exit 1
else
  echo "Deployment succeeded!"
fi