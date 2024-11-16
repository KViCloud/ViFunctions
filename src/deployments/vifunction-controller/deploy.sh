#!/bin/bash

# Constants
NAMESPACE="vifunction-controller"
RELEASE_NAME="vifunction-controller"
CHART_PATH="."

# Check if the namespace exists
if ! kubectl get namespace "$NAMESPACE" > /dev/null 2>&1; then
  echo "Creating namespace $NAMESPACE"
  kubectl create namespace "$NAMESPACE"
else
  echo "Namespace $NAMESPACE already exists"
fi

# Update Helm dependencies
echo "Updating Helm dependencies"
helm dependency update "$CHART_PATH"

# Check if the release already exists
if helm status "$RELEASE_NAME" --namespace "$NAMESPACE" > /dev/null 2>&1; then
  echo "Upgrading release $RELEASE_NAME in namespace $NAMESPACE"
  helm upgrade "$RELEASE_NAME" "$CHART_PATH" --namespace "$NAMESPACE"
else
  echo "Installing release $RELEASE_NAME in namespace $NAMESPACE"
  helm install "$RELEASE_NAME" "$CHART_PATH" --namespace "$NAMESPACE"
fi