/usr/local/bin/k3s-uninstall.sh
sudo rm -rf /etc/rancher /var/lib/rancher /var/lib/k3s
systemctl list-units | grep k3s
sudo systemctl stop k3s
sudo systemctl disable k3s