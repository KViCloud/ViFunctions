# Cloud Function Documentation

## Overview

Welcome to the documentation for our cloud function capabilities. This open-source project aims to provide easy-to-use cloud functions that operate similarly to other major cloud providers. Please note that this project is still under development.

## Project Status

**Current Status**: In Development  
We are actively working on this project, and new features are being added regularly. Contributions and feedback are welcome.

## Features

- **Event-Driven Execution**: Trigger functions based on various events.
- **Automatic Scaling**: Functions scale automatically in response to performance needs.
- **Managed Infrastructure**: No server management required.

## Architecture

The architecture of our cloud function platform consists of several key components, detailed below:

- **Images builder**: The service builds an image from the source code. Supports multiple languages like Go, Python, and Node.js...
- **Functions deployer**: The service deploys function apps to a Kubernetes cluster.
- **Zero scale**: Automatically adjusts the number of instances running to zero.
- **Function ingress**: The service functions as a router, directing external requests to the function apps.

## Local Development

### Prerequisites

1. **Dotnet Sdk (Dotnet 8)**
2. **Docker or Podman**
3. **K3s (Version v-... recommended)**
4. **Helm**
5. **Buildah (Optional)**
6. **Mysql (Optional)**


### Installation

1. **Clone the Repository**:
  ```bash
    git clone https://github.com/NguyenQuang2016/ViFunctions.git
    cd ViFunctions
  ```
2. **Build Container Image**
  ```bash
    cd src/deployments
    chmod +x build_images.sh
    ./build_images.sh
  ```

4. **Install Mysql(Optional)**
  ```bash
    chmod +x install_mysql.sh
    ./install_mysql.sh
  ```

5. **Build Helm Chart**
  ```bash
    cd src/deployments/vifunction
    helm dependency update
    helm template vifunction . --namespace vifunction-ns --debug -f values-local.yaml
  ```


5. **Deploy App**
  ```bash
    cd ..
    chmod +x install_app.sh
    ./install_app.sh
  ```

6. **Test Api with Postman**
  ```bash
    git clone https://github.com/hacksider/Deep-Live-Cam.git
    cd Deep-Live-Cam
  ```
## License

Include the project's license information.