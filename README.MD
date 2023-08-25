# URL Goat - URL Shortener Project

URL Goat is a straightforward URL shortener project crafted with ASP.NET Core 6 on the backend and Angular on the frontend. This application empowers you to condense long URLs into concise, user-friendly links.

## Key Features

- **URL Shortening**: Seamlessly transform extensive URLs into shorter, manageable links.
- **Validation**: Verify the validity of URLs before shortening to ensure they are functional.
- **Error Handling**: Gracefully manage errors and present user-friendly messages.

This guide outlines the setup process using Docker Compose, simplifying deployment and management.

## Prerequisites

Before you begin, ensure the following dependencies are met:

- **Git**: Install Git by following the instructions at [Git Installation Guide](https://github.com/git-guides/install-git).
- **Docker Desktop**: Download and install Docker Desktop from [Docker's official website](https://www.docker.com/products/docker-desktop/).

## Getting Started

1. **Clone the Repository**:
   - Use Git to clone the project repository from [URL Goat Repository](https://github.com/sdzuo/goaturl).

2. **Navigate to Project Root**:
   - Open a terminal/command prompt and navigate to the root folder of the cloned project.

3. **Build and Launch Containers**:
   - Execute the following command to initiate Docker Compose and build the necessary containers:
     ```bash
     docker compose up --build
     ```

## Usage 

1. **Database Initialization**:
   - After executing the Docker Compose command, you'll notice a SQL script running. Wait until it completes, and you should see a message stating "Changed database context to urlshortener." This process typically takes 1-2 minutes.

2. **Access the Frontend**:
   - Once the database is ready, you can access the frontend application at `http://localhost:4200`.

3. **Shorten URLs**:
   - Enter a long URL into the provided input field and click the "Shorten" button.

4. **View Shortened URLs**:
   - If the URL is valid, the shortened URL will be displayed on the screen.

5. **Note**:
   - If you attempt to make requests before the database setup is complete, you may encounter errors in the browser console.
   - If you are using a Apple Silicon Max, please read the following https://devblogs.microsoft.com/azure-sql/development-with-sql-in-containers-on-macos/.
   - If you would rather run the project locally, please see the LOCALREADME.MD file.

## Legal Notice

All rights reserved.

This comprehensive guide simplifies the setup process for using Docker Compose with the URL Goat application, allowing you to harness its URL shortening capabilities efficiently. If you have any questions or encounter issues during setup, feel free to reach out for assistance.

Feel free to further customize this README according to your project's specific needs or branding.
