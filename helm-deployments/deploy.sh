#!/bin/bash

# Check if the environment argument is provided
ENVIRONMENT=$1

if [[ -z "$ENVIRONMENT" ]]; then
  echo "Usage: ./deploy.sh <environment>"
  exit 1
fi

# List of services to deploy
#services=("images-builder" "serviceB")
services=("images-builder")

# Loop through each service and deploy
for service in "${services[@]}"; do
  echo "Deploying ${service} for environment: ${ENVIRONMENT}"

  # Helm upgrade --install command
  helm upgrade --install "${service}-${ENVIRONMENT}" "./${service}" \
    -f "./${service}/values.yaml" \
    -f "./${service}/values-${ENVIRONMENT}.yaml" \
    --namespace default  # Change to your desired namespace if needed

  if [[ $? -ne 0 ]]; then
    echo "Failed to deploy ${service}-${ENVIRONMENT}. Exiting."
    exit 1
  fi
done

echo "Deployment completed successfully."