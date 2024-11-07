package main

import (
	"log"
	"net/http"
	"os"
)

func uploadHandler(w http.ResponseWriter, r *http.Request) {
	log.Println("Received request to build upload file")

	validRequest := validateRequest(w, r)
	if !validRequest {
		return
	}
	imageName := r.FormValue("image_name")

	// Create a temporary directory for processing
	buildDir, created := createBuildDir(w, imageName)
	if !created {
		return
	}
	defer os.RemoveAll(buildDir) // Clean up

	saved := saveUploadedFile(w, r, buildDir)
	if !saved {
		return
	}

	built := buildImage(w, imageName, buildDir)
	if !built {
		return
	}

	taggedImage, tagged := tagImage(w, imageName)
	if !tagged {
		return
	}

	//should build registry service
	logged := loginDockerIo(w)
	if !logged {
		return
	}

	pushImage(w, taggedImage)

	// Respond with success
	w.WriteHeader(http.StatusOK)
	w.Write([]byte("File uploaded and image built successfully"))
	log.Println("Response sent to client")
}

func main() {
	http.HandleFunc("/build", uploadHandler) // Updated route
	log.Println("Server started on :8080")
	log.Fatal(http.ListenAndServe(":8080", nil)) // Start the server
}
