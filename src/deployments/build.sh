podman build -t vifunction-store:latest ../ViFunction.Store
podman tag vifunction-store:latest docker.io/quangnguyen2017/vifunction-store:latest
podman push docker.io/quangnguyen2017/vifunction-store:latest

podman build -t vifunction-gateway:latest ../ViFunction.Gateway
podman tag vifunction-gateway:latest docker.io/quangnguyen2017/vifunction-gateway:latest
podman push docker.io/quangnguyen2017/vifunction-gateway:latest

podman build -t vifunction-imagebuilder:latest ../ViFunction.ImageBuilder
podman tag vifunction-imagebuilder:latest docker.io/quangnguyen2017/vifunction-imagebuilder:latest
podman push docker.io/quangnguyen2017/vifunction-imagebuilder:latest

podman build -t vifunction-kubeops:latest ../ViFunction.KubeOps
podman tag vifunction-kubeops:latest docker.io/quangnguyen2017/vifunction-kubeops:latest
podman push docker.io/quangnguyen2017/vifunction-kubeops:latest