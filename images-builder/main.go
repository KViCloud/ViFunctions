package main

import (
	"io"
	"log"
	"net/http"
	"os"
	"os/exec"
	"path/filepath"
)

func uploadHandler(w http.ResponseWriter, r *http.Request) {
	// Only allow POST requests
	if r.Method != http.MethodPost {
		http.Error(w, "Invalid request method", http.StatusMethodNotAllowed)
		return
	}

	// Parse the form with a 10 MB limit
	err := r.ParseMultipartForm(10 << 20)
	if err != nil {
		http.Error(w, "Failed to parse form", http.StatusBadRequest)
		log.Printf("Failed to parse form: %v\n", err)
		return
	}

	// Retrieve the uploaded file
	file, _, err := r.FormFile("file")
	if err != nil {
		http.Error(w, "Failed to retrieve file", http.StatusBadRequest)
		log.Printf("Failed to retrieve file: %v\n", err)
		return
	}
	defer file.Close()

	// Retrieve the image name
	imageName := r.FormValue("image_name")
	if imageName == "" {
		http.Error(w, "Image name is required", http.StatusBadRequest)
		log.Printf("Image name is required: %v\n", err)
		return
	}

	// Create a temporary directory for processing
	tempDir, err := os.MkdirTemp("", imageName)
	if err != nil {
		http.Error(w, "Failed to create temp directory", http.StatusInternalServerError)
		log.Printf("Failed to create temp directory: %v\n", err)
		return
	}
	//defer os.RemoveAll(tempDir) // Clean up

	// Create the destination tar file
	out, err := os.Create(filepath.Join(tempDir, imageName))
	if err != nil {
		http.Error(w, "Failed to create destination file", http.StatusInternalServerError)
		log.Printf("Failed to create destination file: %v\n", err)
		return
	}
	defer out.Close()

	// Copy the uploaded file to the destination
	_, err = io.Copy(out, file)
	if err != nil {
		http.Error(w, "Failed to copy file", http.StatusInternalServerError)
		log.Printf("Failed to copy file: %v\n", err)
		return
	}

	// Extract the tar file
	err = exec.Command("tar", "-xf", filepath.Join(tempDir, imageName), "-C", tempDir).Run()
	if err != nil {
		http.Error(w, "Failed to extract tar file", http.StatusInternalServerError)
		log.Printf("Failed to extract tar file: %v\n", err)
		return
	}

	// Build the Podman image with the custom image name
	cmd := exec.Command("podman", "build", "-t", imageName, tempDir)
	err = cmd.Run()
	if err != nil {
		http.Error(w, "Failed to build image", http.StatusInternalServerError)
		log.Printf("Failed to build image: %v\n", err)
		return
	}

	// Respond with success
	w.WriteHeader(http.StatusOK)
	w.Write([]byte("File uploaded and image built successfully"))
}

func main() {
	http.HandleFunc("/build", uploadHandler) // Updated route
	log.Println("Server started on :8080")
	log.Fatal(http.ListenAndServe(":8080", nil)) // Start the server
}
