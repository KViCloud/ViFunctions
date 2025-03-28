/usr/local/bin/k3s-uninstall.sh
sudo rm -rf /etc/rancher /var/lib/rancher /var/lib/k3s
systemctl list-units | grep k3s
sudo systemctl stop k3s
sudo systemctl disable k3s

curl -sfL https://get.k3s.io | INSTALL_K3S_VERSION="v1.31.4+k3s1" sh - 