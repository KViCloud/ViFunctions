Here are two Markdown files for setting up Kubernetes on AWS EC2.

### File 1: `kubernetes_master_setup.md`

```markdown
# Setting Up Kubernetes Master Node on AWS EC2

## Prerequisites

1. **AWS Account**: Ensure you have an AWS account.
2. **EC2 Instance**: Launch an EC2 instance using Ubuntu as the OS.
3. **Security Groups**: Open necessary ports:
   - TCP 6443 (Kubernetes API server)
   - TCP 10250 (Kubelet API)
   - TCP 10255 (Read-only Kubelet API)
   - TCP 30000-32767 (NodePort Services)
   - TCP 22 (SSH)
```

## Step 1: Update System Packages

SSH into your master node and run:

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

Disable swap on the master node:

```bash
sudo swapoff -a
```

To permanently disable swap, comment out or remove any swap entries in `/etc/fstab`.

## Step 4: Initialize the Master Node

Run the following command to initialize the master node:

```bash
sudo kubeadm init --apiserver-advertise-address=172.31.37.104 --pod-network-cidr=192.168.0.0/16
```

## Step 5: Set Up `kubectl` for the Master Node

Run these commands to set up `kubectl`:

```bash
# Create a directory for kube config
mkdir -p $HOME/.kube

# Copy the kube config
sudo cp -i /etc/kubernetes/admin.conf $HOME/.kube/config

# Set permissions
sudo chown $(id -u):$(id -g) $HOME/.kube/config
```

## Step 6: Install a Network Add-on

Install Calico as a network add-on:

```bash
kubectl apply -f https://docs.projectcalico.org/manifests/calico.yaml
```

## Conclusion

You now have a Kubernetes master node set up on AWS EC2.
```