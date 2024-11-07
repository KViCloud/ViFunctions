# User Story: Kubernetes Deployment Server

## Title: Simplify Kubernetes Deployments for Developers

### As a:
Kubernetes user (Developer/DevOps Engineer).

### I want to:
Easily deploy applications to my Kubernetes cluster using a simple HTTP API.

### So that:
I can streamline the deployment process and reduce redundancy, ensuring consistent deployment practices.

## Acceptance Criteria:

1. **HTTP Server Setup:**
    - The server should listen on port 8080.
    - The server should handle POST requests to the `/deploy` endpoint.

2. **Deployment Request:**
    - The request body should accept a JSON payload with `name` and `image` fields.
      ```json
      {
        "name": "my-app",
        "image": "my-app-image:v1"
      }
      ```

3. **Kubernetes Configuration:**
    - The server should use the `KUBECONFIG` environment variable to locate the Kubernetes configuration file.

4. **Deployment Creation:**
    - The server should parse a `deployment.yaml` template using the values provided in the request payload.
    - The server should create a Kubernetes deployment in the specified namespace (default if not specified).

5. **HPA Creation:**
    - The server should parse an `hpa.yaml` template using the values provided in the request payload.
    - The server should create a Horizontal Pod Autoscaler (HPA) for the deployment in the specified namespace (default if not specified).

6. **Error Handling:**
    - If the request payload is invalid, the server should respond with a `400 Bad Request` and an appropriate error message.
    - If there are errors in creating the deployment or HPA, the server should respond with a `500 Internal Server Error` and an appropriate error message.

7. **Success Response:**
    - On successful deployment and HPA creation, the server should respond with a `201 Created` status.

## Scenario:

### Given:
A developer has a Kubernetes cluster and a valid `kubeconfig` file.

### When:
The developer sends a POST request to the `/deploy` endpoint with the JSON payload containing the application's name and image.

### Then:
The server should create a deployment and Horizontal Pod Autoscaler (HPA) for the application in the Kubernetes cluster and respond with a `201 Created` status.

---

## Example API Usage

### Request:
```sh
curl -X POST http://localhost:8080/deploy -d '{
    "name": "my-app",
    "image": "my-app-image:v1"
}'
```

### Response:
- **Success:** HTTP `201 Created`
- **Failure:** HTTP `400 Bad Request` or `500 Internal Server Error` with a descriptive message.

## Templates

### Deployment Template (`deployment.yaml`):
```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: {{ .Name }}
spec:
  replicas: 1
  selector:
    matchLabels:
      app: {{ .Name }}
  template:
    metadata:
      labels:
        app: {{ .Name }}
    spec:
      containers:
      - name: {{ .Name }}
        image: {{ .Image }}
        ports:
        - containerPort: 80
```

### HPA Template (`hpa.yaml`):
```yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: {{ .Name }}-hpa
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: {{ .Name }}
  minReplicas: 1
  maxReplicas: 10
  metrics:
  - type: Resource
    resource:
      name: cpu
      target:
        type: Utilization
        averageUtilization: 80
```

---

This user story provides a comprehensive guide to implementing and validating the Kubernetes Deployment Server, ensuring it meets user needs and expectations.