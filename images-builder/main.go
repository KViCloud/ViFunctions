package main

import (
	"io"
	"log"
	"net/http"
	"os"
	"os/exec"
	"path/filepath"
	"strings"
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
		return
	}

	// Retrieve the uploaded file
	file, _, err := r.FormFile("file")
	if err != nil {
		http.Error(w, "Failed to retrieve file", http.StatusBadRequest)
		return
	}
	defer file.Close()

	// Retrieve the custom tar file name
	tarFileName := r.FormValue("tar_file_name")
	if tarFileName == "" {
		http.Error(w, "Tar file name is required", http.StatusBadRequest)
		return
	}

	// Retrieve the image name
	imageName := r.FormValue("image_name")
	if imageName == "" {
		http.Error(w, "Image name is required", http.StatusBadRequest)
		return
	}

	// Create a temporary directory for processing
	tempDirName := strings.TrimSuffix(tarFileName, ".tar")
	tempDir, err := os.MkdirTemp("", tempDirName)
	if err != nil {
		http.Error(w, "Failed to create temp directory", http.StatusInternalServerError)
		return
	}
	defer os.RemoveAll(tempDir) // Clean up

	// Create the destination tar file
	out, err := os.Create(filepath.Join(tempDir, tarFileName))
	if err != nil {
		http.Error(w, "Failed to create destination file", http.StatusInternalServerError)
		return
	}
	defer out.Close()

	// Copy the uploaded file to the destination
	_, err = io.Copy(out, file)
	if err != nil {
		http.Error(w, "Failed to copy file", http.StatusInternalServerError)
		return
	}

	// Extract the tar file
	err = exec.Command("tar", "-xf", filepath.Join(tempDir, tarFileName), "-C", tempDir).Run()
	if err != nil {
		http.Error(w, "Failed to extract tar file", http.StatusInternalServerError)
		return
	}

	// Build the Podman image with the custom image name
	cmd := exec.Command("podman", "build", "-t", imageName, tempDir)
	err = cmd.Run()
	if err != nil {
		http.Error(w, "Failed to build image", http.StatusInternalServerError)
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
