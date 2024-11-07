package main

import (
	"bytes"
	"io"
	"mime/multipart"
	"net/http"
	"net/http/httptest"
	"os"
	"path/filepath"
	"testing"
)

func TestBuildHandler(t *testing.T) {
	files := []string{"examplefunc/main.go", "examplefunc/go.mod", "examplefunc/Containerfile"} // Add more files as needed

	var b bytes.Buffer
	w := multipart.NewWriter(&b)

	for _, filePath := range files {
		if _, err := os.Stat(filePath); os.IsNotExist(err) {
			t.Fatalf("file %s does not exist", filePath)
		}

		fw, err := w.CreateFormFile("file", filepath.Base(filePath))
		if err != nil {
			t.Fatal(err)
		}

		file, err := os.Open(filePath)
		if err != nil {
			t.Fatal(err)
		}
		defer file.Close()

		if _, err := io.Copy(fw, file); err != nil {
			t.Fatal(err)
		}
	}

	if err := w.WriteField("image_name", "functions_app_67857"); err != nil {
		t.Fatal(err)
	}
	w.Close()

	req := httptest.NewRequest(http.MethodPost, "/build", &b)
	req.Header.Set("Content-Type", w.FormDataContentType())

	rr := httptest.NewRecorder()
	uploadHandler(rr, req)
	res := rr.Result()
	if res.StatusCode != http.StatusOK {
		t.Fatalf("expected status OK; got %v", res.Status)
	}

	body, err := io.ReadAll(res.Body)
	if err != nil {
		t.Fatal(err)
	}
	defer res.Body.Close()

	expected := "File uploaded and image built successfully"
	if string(body) != expected {
		t.Errorf("expected response body to be %q; got %q", expected, string(body))
	}
}
