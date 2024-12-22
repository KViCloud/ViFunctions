
### File 2: `kubernetes_worker_setup.md`

# Setting Up Kubernetes Worker Node on AWS EC2

## Prerequisites

1. **AWS Account**: Ensure you have an AWS account.
2. **EC2 Instance**: Launch an EC2 instance using Ubuntu as the OS.
3. **Security Groups**: Open necessary ports:
   - TCP 6443 (Kubernetes API server)
   - TCP 10250 (Kubelet API)
   - TCP 10255 (Read-only Kubelet API)
   - TCP 30000-32767 (NodePort Services)
   - TCP 22 (SSH)

## Step 1: Update System Packages

SSH into your worker node and run:

```bash
sudo apt update && sudo apt upgrade -y
```
## Step 2: Install Required Packages

Install Docker, `kubeadm`, `kubelet`, and `kubectl`:

```bash
# Install Docker
sudo apt install -y docker.io
sudo systemctl enable docker
sudo systemctl start docker

# Add Kubernetes signing key
sudo apt-get install -y apt-transport-https ca-certificates curl gpg

curl -fsSL https://pkgs.k8s.io/core:/stable:/v1.32/deb/Release.key | sudo gpg --dearmor -o /etc/apt/keyrings/kubernetes-apt-keyring.gpg

echo 'deb [signed-by=/etc/apt/keyrings/kubernetes-apt-keyring.gpg] https://pkgs.k8s.io/core:/stable:/v1.32/deb/ /' | sudo tee /etc/apt/sources.list.d/kubernetes.list

# Update package list and install Kubernetes components
sudo apt update
sudo apt install -y kubelet kubeadm kubectl
sudo apt-mark hold kubelet kubeadm kubectl
```

## Step 3: Disable Swap

Disable swap on the worker node:

```bash
sudo swapoff -a
```

To permanently disable swap, comment out or remove any swap entries in `/etc/fstab`.

## Step 4: Join the Worker Node

Run the `kubeadm join` command from the master node's initialization output:

```bash
sudo kubeadm join <master-ip>:6443 --token <token> --discovery-token-ca-cert-hash sha256:<hash>
```

## Step 5: Verify the Cluster

Back on the master node, check the node status:

```bash
kubectl get nodes
```

You should see both the master and worker nodes listed.