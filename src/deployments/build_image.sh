podman build --platform linux/amd64 -t vifunction-store:latest ../ViFunction.Store
podman tag vifunction-store:latest docker.io/quangnguyen2017/vifunction-store:latest
podman push docker.io/quangnguyen2017/vifunction-store:latest

podman build --platform linux/amd64 -t vifunction-gateway:latest ../ViFunction.Gateway
podman tag vifunction-gateway:latest docker.io/quangnguyen2017/vifunction-gateway:latest
podman push docker.io/quangnguyen2017/vifunction-gateway:latest

podman build --platform linux/amd64 -t vifunction-imagebuilder:latest ../ViFunction.ImageBuilder
podman tag vifunction-imagebuilder:latest docker.io/quangnguyen2017/vifunction-imagebuilder:latest
podman push docker.io/quangnguyen2017/vifunction-imagebuilder:latest

podman build --platform linux/amd64 -t vifunction-kubeops:latest ../ViFunction.KubeOps
podman tag vifunction-kubeops:latest docker.io/quangnguyen2017/vifunction-kubeops:latest
podman push docker.io/quangnguyen2017/vifunction-kubeops:latest