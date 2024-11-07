# Container Image Builder Service

This project provides a simple HTTP service that allows developers to upload source code tar files, build container images using `buildah`, and push them to Docker Hub. The service aims to automate the process of container image creation and deployment.

## Installation

To run this service, you need to have Go and `buildah` installed on your machine. You will also need Docker Hub credentials.

1. **Install Go dependencies:**

    ```sh
    go mod tidy
    ```

2. **Set up environment variables for Docker Hub credentials:**

    ```sh
    export DOCKER_USERNAME=<your-docker-hub-username>
    export DOCKER_PASSWORD=<your-docker-hub-password>
    ```

## Run service

To start the HTTP service, run:

```sh
go run main.go
```

The server will start on port `8080` by default.


## Request Example

Here is an example of how to make a request using `curl`:

```sh
curl -X POST http://localhost:8080/build \
    -F "image_name=my-image" \
    -F "file=@/path/to/your/source.tar"
```

## Running Tests

To run the tests, simply use the `go test` command:

```sh
go test ./...
```
To run the tests in container (macos/windows):

```sh
podman build -f container-files/golang/Containerfile-test -t images-builder-test .
podman run --rm -it --privileged images-builder-test
```
## Push image

To build image, simply use the `go test` command:

Build and tag image command:
```sh
podman build -f container-files/golang/Containerfile -t kernel-images-builder .
podman tag kernel-images-builder:latest quangnguyen2017/giongfunctions-kernel-images-builder:latest
```

Push image command:
```sh
podman login docker.io
podman push quangnguyen2017/giongfunctions-kernel-images-builder:latest
podman pull quangnguyen2017/giongfunctions-kernel-images-builder:latest
```