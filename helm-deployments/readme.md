./deploy.sh minikube

./deploy.sh staging

helm list
helm uninstall images-builder-minikube