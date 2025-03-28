sudo iptables -A INPUT -p tcp --dport 6443 -j ACCEPT
sudo iptables -A INPUT -s 10.42.0.0/16 -j ACCEPT
sudo iptables -A INPUT -s 10.43.0.0/16 -j ACCEPT
# Save iptables rules
sudo iptables-save > /etc/iptables/rules.v4