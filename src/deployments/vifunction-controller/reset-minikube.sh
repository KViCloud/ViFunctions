minikube delete
minikube start --driver=podman --container-runtime=cri-o
./vifunction-controller/deploy.sh local
