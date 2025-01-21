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

## Local development

### Prerequisites

- K3s
- Helm
- Dotnet Sdk
- Docker or Podman
- Buildah

### Installation

Installation instructions will be provided once the project reaches a stable release. Stay tuned!


## License

Include the project's license information.