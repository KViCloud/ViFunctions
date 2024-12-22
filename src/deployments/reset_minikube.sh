minikube delete
minikube start --driver=podman --container-runtime=cri-o
minikube addons enable metrics-server
minikube ip