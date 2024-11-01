podman build -t kernel-images-builder .
podman tag kernel-images-builder:latest quangnguyen2017/giongfunctions-kernel-images-builder:latest

podman login docker.io
podman push quangnguyen2017/giongfunctions-kernel-images-builder:latest
podman pull quangnguyen2017/giongfunctions-kernel-images-builder:latest
