# User Story: Upload and Build Container Images

## Title: On-Demand Container Image Build and Push

### As a:
Developer

### I want to:
Upload source code tar files to an HTTP service that will build container images and push them to Docker Hub. 
In the future we will build our image registration service

### So that:
I can automate and streamline the process of building and deploying container images without manual intervention.

---

### Acceptance Criteria:

1. **Endpoint for Uploads**:
    - The system should provide an HTTP endpoint (`/build`) that accepts POST requests for uploading source code tar files.

2. **Request Validation**:
    - The system should only accept POST requests.
    - The request must contain a multipart form with an `image_name` and a `file` field for the tar file.
    - If the request validation fails, an appropriate error message should be returned.

3. **Temporary Directory Creation**:
    - A temporary directory should be created for processing the uploaded files.
    - This directory should be cleaned up after the build process, regardless of success or failure.

4. **File Handling**:
    - The system should save the uploaded tar file to the temporary directory.
    - If saving the file fails, an appropriate error message should be returned.

5. **Source Code Extraction**:
    - The system should extract the contents of the uploaded tar file into the temporary directory.
    - If the extraction fails, an appropriate error message should be returned.

6. **Image Building**:
    - The system should build a container image from the extracted source code using `buildah`.
    - The image should be tagged with the provided `image_name`.
    - If the build process fails, an appropriate error message should be returned.

7. **Image Tagging**:
    - The system should tag the built image with the `:latest` tag.
    - If tagging fails, an appropriate error message should be returned.

8. **Docker Hub Login**:
    - The system should log in to Docker Hub using predefined credentials.
    - If the login process fails, an appropriate error message should be returned.

9. **Image Pushing**:
    - The system should push the tagged image to Docker Hub.
    - If pushing the image fails, an appropriate error message should be returned.

10. **Response on Success**:
    - If all processes succeed, the system should return an HTTP 200 OK status with a message indicating the successful upload and build of the image.

11. **Logging**:
    - The system should log important steps and errors throughout the process for troubleshooting and auditing purposes.

---

### Example Workflow:

1. A developer sends a POST request to `/build` with a multipart form containing the `image_name` and the source code tar file as `file`.
2. The system validates the request.
3. The system creates a temporary directory and saves the uploaded tar file to this directory.
4. The system extracts the contents of the tar file.
5. The system builds a container image using `buildah`.
6. The system tags the built image with `:latest`.
7. The system logs into Docker Hub.
8. The system pushes the tagged image to Docker Hub.
9. The system responds with HTTP 200 OK, indicating success.

By following this user story, developers can automate and simplify their workflow for building and deploying container images. The clear acceptance criteria ensure that all necessary steps are covered and error handling is in place for each stage of the process.