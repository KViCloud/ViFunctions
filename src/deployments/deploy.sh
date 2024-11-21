#!/bin/bash

set -e

NAMESPACE="vifunction-ns"
RELEASE_NAME="vifunction"
CHART_DIR="./vifunction"

# Create the namespace if it doesn't exist
kubectl get namespace $NAMESPACE 1>/dev/null 2>/dev/null || kubectl create namespace $NAMESPACE

# Check if the release exists
if helm ls -n $NAMESPACE | grep $RELEASE_NAME >/dev/null 2>&1; then
  echo "Upgrading the Helm release..."
  # Upgrade the release
  helm upgrade $RELEASE_NAME $CHART_DIR -n $NAMESPACE
else
  echo "Installing the Helm release..."
  # Install the release
  helm install $RELEASE_NAME $CHART_DIR -n $NAMESPACE
fi

echo "Deployment complete."