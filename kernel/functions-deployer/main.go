package main

import (
	"bytes"
	"context"
	"encoding/json"
	"net/http"
	"os"
	"text/template"

	"k8s.io/api/apps/v1"
	"k8s.io/api/autoscaling/v2"
	metav1 "k8s.io/apimachinery/pkg/apis/meta/v1"
	"k8s.io/apimachinery/pkg/util/yaml"
	"k8s.io/client-go/kubernetes"
	"k8s.io/client-go/tools/clientcmd"
)

type DeployRequest struct {
	Name  string `json:"name"`
	Image string `json:"image"`
}

func main() {
	http.HandleFunc("/deploy", deployHandler)
	http.ListenAndServe(":8080", nil)
}

func deployHandler(w http.ResponseWriter, r *http.Request) {
	var req DeployRequest
	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		http.Error(w, err.Error(), http.StatusBadRequest)
		return
	}

	kubeconfig := os.Getenv("KUBECONFIG")
	config, err := clientcmd.BuildConfigFromFlags("", kubeconfig)
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

	clientset, err := kubernetes.NewForConfig(config)
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

	// Deploy
	if err := deploy(clientset, req); err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

	// HPA
	if err := createHPA(clientset, req); err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusCreated)
}

func deploy(clientset *kubernetes.Clientset, req DeployRequest) error {
	tpl, err := template.ParseFiles("deployment.yaml")
	if err != nil {
		return err
	}

	var deploymentYaml bytes.Buffer
	if err := tpl.Execute(&deploymentYaml, req); err != nil {
		return err
	}

	deployment := &v1.Deployment{}
	if err := yaml.Unmarshal(deploymentYaml.Bytes(), deployment); err != nil {
		return err
	}

	_, err = clientset.AppsV1().Deployments("default").Create(context.TODO(), deployment, metav1.CreateOptions{})
	return err
}

func createHPA(clientset *kubernetes.Clientset, req DeployRequest) error {
	hpaTpl, err := template.ParseFiles("hpa.yaml")
	if err != nil {
		return err
	}

	var hpaYaml bytes.Buffer
	if err := hpaTpl.Execute(&hpaYaml, req); err != nil {
		return err
	}

	hpa := &v2.HorizontalPodAutoscaler{}
	if err := yaml.Unmarshal(hpaYaml.Bytes(), hpa); err != nil {
		return err
	}

	_, err = clientset.AutoscalingV2().HorizontalPodAutoscalers("default").Create(context.TODO(), hpa, metav1.CreateOptions{})
	return err
}
