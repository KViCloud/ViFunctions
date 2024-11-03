./deploy.sh minikube

./deploy.sh staging

helm list
helm uninstall images-builder-minikube

# Build the Podman image
podman build -t images-builder .
podman run -p 8080:8080 images-builder