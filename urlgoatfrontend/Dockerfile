# Stage 1: Build the Angular application
FROM node:latest as build

# Set the working directory in the build stage
WORKDIR /app

# Copy package.json and package-lock.json to the container
COPY package*.json ./

# Install Angular CLI globally
RUN npm install -g @angular/cli

# Install project dependencies
RUN npm install

# Copy the rest of the application code
COPY . .

# Build the Angular application
#RUN ng build --configuration

# Expose port 4200 (or the port your Angular app will run on)
EXPOSE 4200

# Start the Angular development server with ng serve -o
CMD ["ng", "serve", "--host", "0.0.0.0", "--port", "4200", "--open"]