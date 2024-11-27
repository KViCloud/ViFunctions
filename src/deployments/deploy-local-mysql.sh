#!/bin/bash

# Set variables
NAMESPACE="mysql"
RELEASE_NAME="mysql-release"
MYSQL_ROOT_PASSWORD="P@ssw0rd"  # Replace with a secure password

# Function to check for errors
check_error() {
    if [ $? -ne 0 ]; then
        echo "Error occurred. Exiting."
        exit 1
    fi
}

# Step 1: Add Bitnami Helm repository
echo "Adding Bitnami Helm repository..."
helm repo add bitnami https://charts.bitnami.com/bitnami
check_error

echo "Updating Helm repositories..."
helm repo update
check_error

# Step 2: Create a Kubernetes namespace
echo "Creating namespace '$NAMESPACE'..."
kubectl create namespace $NAMESPACE --dry-run=client -o yaml | kubectl apply -f -
check_error

# Step 3: Install MySQL using Helm
echo "Installing MySQL..."
helm install $RELEASE_NAME bitnami/mysql --namespace $NAMESPACE \
    --set auth.rootPassword=$MYSQL_ROOT_PASSWORD
check_error

# Step 4: Verify the installation
echo "Verifying the installation..."
kubectl get pods --namespace $NAMESPACE
kubectl get svc --namespace $NAMESPACE

echo "MySQL deployment is complete!"
echo "You can access MySQL using the following command:"
echo "kubectl run -i --tty --rm debug --image=mysql:5.7 --namespace $NAMESPACE -- bash"
echo "Then connect using: mysql -h $RELEASE_NAME -u root -p"