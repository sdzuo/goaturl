# Use the official SQL Server image as the base image
FROM mcr.microsoft.com/mssql/server:2019-latest

# Set environment variables for SQL Server
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=YourStrongPassword

# Create a database during container startup
COPY create-database.sql /docker-entrypoint-initdb.d/