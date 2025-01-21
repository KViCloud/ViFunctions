# Setting Up RKE2 Kubernetes Cluster on AWS EC2

## Prerequisites
1. **AWS Account**: Ensure you have an active AWS account.
2. **IAM Role**: Create an IAM role with EC2 permissions to allow your instances to interact with other AWS services.
3. **Key Pair**: Create an SSH key pair for accessing your EC2 instances.
4. https://ranchermanager.docs.rancher.com/getting-started/installation-and-upgrade/installation-requirements

## Step 1: Launch EC2 Instances
### Launch Master Node
1. Go to the EC2 Dashboard.
2. Click on "Launch Instance".
3. Choose an Amazon Machine Image (AMI), ideally a lightweight Linux distribution like Ubuntu or Amazon Linux 2.
4. Select an instance type (t2.medium or t3.medium recommended).
5. Configure the network settings (VPC, subnet).
6. Add storage as needed.
7. Configure security group rules to allow SSH (port 22) and Kubernetes API (port 6443).
8. Launch the instance and note its public IP.

### Launch Worker Node
- Repeat the above steps to launch a worker node, ensuring it’s in the same VPC and subnet as the master node.

## Step 2: SSH into Instances
- Use SSH to connect to the master node:
  ```bash
  ssh -i "vi-server.pem" ubuntu@ec2-52-40-189-33.us-west-2.compute.amazonaws.com
  ```

## Step 3: Install RKE2 on Master Node
1. **Download and Install RKE2**:
   ```bash
      curl -sfL https://get.rke2.io | sudo INSTALL_RKE2_VERSION=v1.30.8+rke2r1 sh - | tee rke2_install.log

2.  INSTALL_RKE2_VERSION=v1.28.15+rke2r1 ./install.sh
   ```

2. **Enable and Start RKE2**:
   ```bash
   sudo systemctl enable rke2-server.service
   sudo systemctl start rke2-server.service
   sudo systemctl status rke2-server
   ```
2. **Install kubectl**:
   ```bash
   sudo apt-get install -y kubectl
   kubectl version --client
   ```
2. **Cluster Access**:
   ```bash
   sudo cp /etc/rancher/rke2/rke2.yaml ~/.kube/config
   sudo chown $(id -u):$(id -g) ~/.kube/config
   kubectl get nodes
   ```

3. **Get the RKE2 Join Token**:
   ```bash
   cat /var/lib/rancher/rke2/server/node-token
   ```

## Step 4: Install RKE2 on Worker Node
1. **SSH into Worker Node**:
   ```bash
   ssh -i "your-key.pem" ec2-user@<Worker-Node-Public-IP>
   ```

2. **Download and Install RKE2**:
   ```bash
   curl -sfL https://get.rke2.io | sh -
   ```

3. **Configure the Worker Node**:
   Create a configuration file for RKE2:
   ```bash
   echo "server: https://<Master-Node-Public-IP>:9345" > /etc/rancher/rke2/config.yaml
   echo "token: <your-token>" >> /etc/rancher/rke2/config.yaml
   ```

4. **Enable and Start RKE2**:
   ```bash
   systemctl enable rke2-agent.service
   systemctl start rke2-agent.service
   ```

## Step 5: Verify the Cluster
1. **Check Node Status**:
   - SSH back into the master node.
   - Run the following command:
   ```bash
   kubectl get nodes
   ```

   You should see both the master and worker nodes listed.

## Step 6: Install Kubernetes Dashboard (Optional)
If you'd like to install the Kubernetes dashboard, you can do so using the following command:
```bash
kubectl apply -f https://raw.githubusercontent.com/kubernetes/dashboard/v2.5.1/aio/deploy/recommended.yaml
```

## Conclusion
Your RKE2 Kubernetes cluster with one master node and one worker node is now set up on AWS EC2. You can start deploying applications and managing your cluster using `kubectl`.

## Notes
- Ensure you monitor your EC2 instances and manage costs effectively.
- Consider implementing a load balancer for production deployments.

Feel free to ask if you need further assistance!
```

You can copy this text into a `.md` file for your documentation. Let me know if you need any more help!