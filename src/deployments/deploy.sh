#!/bin/bash

set -e

NAMESPACE="vifunction-ns"
RELEASE_NAME="vifunction"
CHART_DIR="./vifunction"

# Create the namespace if it doesn't exist
if kubectl get namespace "$NAMESPACE" >/dev/null 2>&1; then
  echo "Namespace '$NAMESPACE' already exists."
else
  echo "Creating namespace '$NAMESPACE'."
  kubectl create namespace "$NAMESPACE"
fi

# Check if the release exists
if helm ls -n $NAMESPACE | grep $RELEASE_NAME >/dev/null 2>&1; then
  echo "Upgrading the Helm release..."
  # Upgrade the release
  helm upgrade $RELEASE_NAME $CHART_DIR -n $NAMESPACE -f vifunction/values-local.yaml
else
  echo "Installing the Helm release..."
  # Install the release
  helm install $RELEASE_NAME $CHART_DIR -n $NAMESPACE -f vifunction/values-local.yaml
fi

echo "Deployment complete."