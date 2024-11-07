#!/bin/bash

# Set default values
NAMESPACE="kernel"
CHART_DIR="./images-builder-chart"

# Check for environment argument
if [ "$#" -ne 1 ]; then
    echo "Usage: $0 {local|staging}"
    exit 1
fi

ENV=$1

# Set values file based on environment
if [ "$ENV" == "local" ]; then
    VALUES_FILE="$CHART_DIR/values-local.yaml"
elif [ "$ENV" == "staging" ]; then
    VALUES_FILE="$CHART_DIR/values-staging.yaml"
else
    echo "Invalid environment. Use 'local' or 'staging'."
    exit 1
fi

# Create the namespace if it doesn't exist
kubectl get namespace "$NAMESPACE" &>/dev/null
if [ $? -ne 0 ]; then
    echo "Creating namespace: $NAMESPACE"
    kubectl create namespace "$NAMESPACE"
fi

# Deploy the Helm chart
echo "Deploying images-builder in the $ENV environment..."
helm upgrade --install images-builder "$CHART_DIR" -f "$VALUES_FILE" --namespace "$NAMESPACE"

# Check the deployment status
if [ $? -eq 0 ]; then
    echo "Deployment successful!"
else
    echo "Deployment failed!"
    exit 1
fi