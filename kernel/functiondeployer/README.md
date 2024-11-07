# Functions deployer

This Go application is a web server that handles deployment requests for Kubernetes applications. It listens on port 8080 and processes incoming HTTP POST requests to the `/deploy` endpoint.

## Prerequisites

- [Go](https://golang.org/dl/) 1.23.2 or higher
- Kubernetes cluster
- Kubeconfig file with access to the Kubernetes cluster
- `deployment.yaml` and `hpa.yaml` templates

## Build the application

```sh
go build -o functions-deployer
```

## Configuration

Ensure you have the `KUBECONFIG` environment variable set to your Kubernetes configuration file path:

```sh
export KUBECONFIG=/path/to/your/kubeconfig
```

## Usage

Run the web server:

```sh
./k8s-deployment-server
```

Run from container:

```sh
podman build -t functions-deployer .
podman run -p 8080:8080 functions-deployer
```

Send a POST request to the `/deploy` endpoint with a JSON payload containing the name and image of the application to be deployed. For example:

```sh
curl -X POST http://localhost:8080/deploy -d '{
    "name": "my-app",
    "image": "my-app-image:v1"
}'
```