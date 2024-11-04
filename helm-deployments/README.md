minikube config set driver podman
minikube config set rootless false
minikube start --driver=podman --container-runtime=cri-o
minikube delete

./deploy.sh minikube

./deploy.sh staging

helm list
helm uninstall images-builder-minikube

# Build the Podman image
podman build -t images-builder .
podman run -p 8080:8080 images-builder