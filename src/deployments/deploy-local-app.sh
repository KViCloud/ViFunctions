#!/bin/bash

set -e

VI_KERNEL="vifunction-ns"
VI_FUNC_HUB="funchub-ns"

RELEASE_NAME="vifunction"
CHART_DIR="./vifunction"

# Create the namespace if it doesn't exist
if kubectl get namespace "$VI_KERNEL" >/dev/null 2>&1; then
  echo "Namespace '$VI_KERNEL' already exists."
else
  echo "Creating namespace '$VI_KERNEL'."
  kubectl create namespace "$VI_KERNEL"
fi

# Create the namespace if it doesn't exist
if kubectl get namespace "$VI_FUNC_HUB" >/dev/null 2>&1; then
  echo "Namespace '$VI_FUNC_HUB' already exists."
else
  echo "Creating namespace '$VI_FUNC_HUB'."
  kubectl create namespace "$VI_FUNC_HUB"
fi

# Check if the release exists
if helm ls -n $VI_KERNEL | grep $RELEASE_NAME >/dev/null 2>&1; then
  echo "Upgrading the Helm release..."
  # Upgrade the release
  helm upgrade $RELEASE_NAME $CHART_DIR -n $VI_KERNEL -f vifunction/values-local.yaml
else
  echo "Installing the Helm release..."
  # Install the release
  helm install $RELEASE_NAME $CHART_DIR -n $VI_KERNEL -f vifunction/values-local.yaml
fi

echo "Deployment complete."