# Use the official .NET SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:7.0

# Set the working directory inside the container
WORKDIR /app

# Retrieve Env Variables
ARG CONNECTION_STRING
ENV ConnectionsStrings__StocksDb=$CONNECTION_STRING

ARG ASPNETCORE_ENV
ENV ASPNETCORE_ENVIRONMENT=$ASPNETCORE_ENV

ARG EULA
ENV ACCEPT_EULA=$EULA

# Copy the contents of your local project directory into the container
COPY . /app

# Install GnuPG (GPG) tools
RUN apt-get update
RUN apt-get install -y gnupg gnupg1 gnupg2

# Update the package list and install curl
RUN apt-get update && apt-get install -y curl

# Use the official .NET SDK image as the base image
RUN curl -sSL https://packages.microsoft.com/keys/microsoft.asc -o /tmp/microsoft.asc && apt-key add /tmp/microsoft.asc
RUN curl https://packages.microsoft.com/config/ubuntu/20.04/prod.list | tee /etc/apt/sources.list.d/msprod.list

ENV ACCEPT_EULA=Y

# Install SQL Server tools
RUN apt-get update
RUN apt-get install -y mssql-tools
RUN curl -sSL https://packages.microsoft.com/keys/microsoft.asc | tee /etc/apt/trusted.gpg.d/microsoft.asc

# Download the Microsoft GPG key using curl
RUN curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor -o /usr/share/keyrings/microsoft-archive-keyring.gpg

# Add the Microsoft GPG key to the APT keyring
RUN echo "deb [arch=amd64 signed-by=/usr/share/keyrings/microsoft-archive-keyring.gpg] https://packages.microsoft.com/ubuntu/22.10/prod focal main" > /etc/apt/sources.list.d/mssql-release.list

# Install SQL Server Database Engine (replace with the desired version/tag)
RUN curl -sSL https://packages.microsoft.com/keys/microsoft.asc | tee /usr/share/keyrings/microsoft-archive-keyring.gpg > /dev/null
RUN curl https://packages.microsoft.com/config/ubuntu/20.04/mssql-server-2019.list > /etc/apt/sources.list.d/mssql-release.list

# ADD Ef package
RUN dotnet add stocks.csproj package Microsoft.EntityFrameworkCore

# Install Entity Framework Core tools
RUN dotnet tool install --global dotnet-ef
RUN apt-get update
ENV PATH="$PATH:/root/.dotnet/tools"

#ENV SA_PASSWORD=$DB_PASSWORD
ENV ACCEPT_EULA=Y

# Add a migration
RUN dotnet ef migrations add Migration10

# Update the database
#RUN dotnet ef database update

EXPOSE 8080:8080

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet nuget locals all --clear
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /App
COPY --from=build-env /App/out .

# Specify the entry point for your application
ENTRYPOINT ["dotnet","stocks.dll"]

