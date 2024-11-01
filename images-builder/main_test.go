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
	tarFilePath := "functions_app_67857.tar"

	if _, err := os.Stat(tarFilePath); os.IsNotExist(err) {
		t.Fatalf("file %s does not exist", tarFilePath)
	}

	var b bytes.Buffer
	w := multipart.NewWriter(&b)

	fw, err := w.CreateFormFile("file", filepath.Base(tarFilePath))
	if err != nil {
		t.Fatal(err)
	}

	tarFile, err := os.Open(tarFilePath)
	if err != nil {
		t.Fatal(err)
	}
	defer tarFile.Close()

	if _, err := io.Copy(fw, tarFile); err != nil {
		t.Fatal(err)
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
