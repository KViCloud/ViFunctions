package main

import (
	"bytes"
	"context"
	"encoding/json"
	"log"
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
	log.Println("Starting server on :8081")
	if err := http.ListenAndServe(":8081", nil); err != nil {
		log.Fatalf("Server failed to start: %v", err)
	}
}

func deployHandler(w http.ResponseWriter, r *http.Request) {
	var req DeployRequest
	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		log.Printf("Error decoding request: %v", err)
		http.Error(w, "Invalid request payload", http.StatusBadRequest)
		return
	}
	log.Printf("Received deployment request: %+v", req)

	kubeconfig := os.Getenv("KUBECONFIG")
	config, err := clientcmd.BuildConfigFromFlags("", kubeconfig)
	if err != nil {
		log.Printf("Error building Kubernetes config: %v", err)
		http.Error(w, "Failed to build Kubernetes config", http.StatusInternalServerError)
		return
	}

	clientset, err := kubernetes.NewForConfig(config)
	if err != nil {
		log.Printf("Error creating Kubernetes client: %v", err)
		http.Error(w, "Failed to create Kubernetes client", http.StatusInternalServerError)
		return
	}

	// Deploy
	if err := deploy(clientset, req); err != nil {
		log.Printf("Error deploying application: %v", err)
		http.Error(w, "Failed to deploy application: "+err.Error(), http.StatusInternalServerError)
		return
	}
	log.Printf("Successfully deployed application: %s", req.Name)

	// HPA
	if err := createHPA(clientset, req); err != nil {
		log.Printf("Error creating HPA: %v", err)
		http.Error(w, "Failed to create HPA: "+err.Error(), http.StatusInternalServerError)
		return
	}
	log.Printf("Successfully created HPA for application: %s", req.Name)

	w.WriteHeader(http.StatusCreated)
}

func deploy(clientset *kubernetes.Clientset, req DeployRequest) error {
	tpl, err := template.ParseFiles("deployment.yaml")
	if err != nil {
		log.Printf("Error parsing deployment template: %v", err)
		return err
	}

	var deploymentYaml bytes.Buffer
	if err := tpl.Execute(&deploymentYaml, req); err != nil {
		log.Printf("Error executing deployment template: %v", err)
		return err
	}

	deployment := &v1.Deployment{}
	if err := yaml.Unmarshal(deploymentYaml.Bytes(), deployment); err != nil {
		log.Printf("Error unmarshalling deployment YAML: %v", err)
		return err
	}

	_, err = clientset.AppsV1().Deployments("default").Create(context.TODO(), deployment, metav1.CreateOptions{})
	if err != nil {
		log.Printf("Error creating deployment in Kubernetes: %v", err)
	}
	return err
}

func createHPA(clientset *kubernetes.Clientset, req DeployRequest) error {
	hpaTpl, err := template.ParseFiles("hpa.yaml")
	if err != nil {
		log.Printf("Error parsing HPA template: %v", err)
		return err
	}

	var hpaYaml bytes.Buffer
	if err := hpaTpl.Execute(&hpaYaml, req); err != nil {
		log.Printf("Error executing HPA template: %v", err)
		return err
	}

	hpa := &v2.HorizontalPodAutoscaler{}
	if err := yaml.Unmarshal(hpaYaml.Bytes(), hpa); err != nil {
		log.Printf("Error unmarshalling HPA YAML: %v", err)
		return err
	}

	_, err = clientset.AutoscalingV2().HorizontalPodAutoscalers("default").Create(context.TODO(), hpa, metav1.CreateOptions{})
	if err != nil {
		log.Printf("Error creating HPA in Kubernetes: %v", err)
	}
	return err
}
