podman build -t vifunction-dataservice:latest ../ViFunction.DataService
podman tag vifunction-dataservice:latest docker.io/quangnguyen2017/vifunction-dataservice:latest
podman push docker.io/quangnguyen2017/vifunction-dataservice:latest

podman build -t vifunction-gateway:latest ../ViFunction.Gateway
podman tag vifunction-gateway:latest docker.io/quangnguyen2017/vifunction-gateway:latest
podman push docker.io/quangnguyen2017/vifunction-gateway:latest

podman build -t vifunction-imagebuilder:latest ../ViFunction.ImageBuilder
podman tag vifunction-imagebuilder:latest docker.io/quangnguyen2017/vifunction-imagebuilder:latest
podman push docker.io/quangnguyen2017/vifunction-imagebuilder:latest