# Use the official .NET 8 SDK as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
#EXPOSE 443

# Install OpenSSL if it's not already installed in the image
RUN apt-get update && apt-get install -y openssl

# Generate a self-signed certificate with OpenSSL
RUN mkdir /https && \
    openssl req -x509 -nodes -days 365 \
    -subj "/CN=localhost" \
    -addext "subjectAltName=DNS:localhost" \
    -newkey rsa:2048 -keyout /https/localhost.key -out /https/localhost.crt && \
    openssl pkcs12 -export -out /https/localhost.pfx -inkey /https/localhost.key -in /https/localhost.crt -password pass:yourpassword

# Set environment variables to configure HTTPS with the generated certificate
#ENV ASPNETCORE_URLS=https://+:443;http://+:5000
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/localhost.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=yourpassword

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy .csproj and restore as distinct layers
COPY ["Datum.Blog.Api/Datum.Blog.Api.csproj", "Datum.Blog.Api/"]
COPY ["Datum.Blog.Application/Datum.Blog.Application.csproj", "Datum.Blog.Application/"]
COPY ["Datum.Blog.Domain/Datum.Blog.Domain.csproj", "Datum.Blog.Domain/"]
COPY ["Datum.Blog.Infrastructure/Datum.Blog.Infrastructure.csproj", "Datum.Blog.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "Datum.Blog.Api/Datum.Blog.Api.csproj"

# Copy all files and build the app
COPY . .
WORKDIR "/src/Datum.Blog.Api"
RUN dotnet build "Datum.Blog.Api.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "Datum.Blog.Api.csproj" -c Release -o /app/publish

# Final stage: runtime environment
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set entry point
ENTRYPOINT ["dotnet", "Datum.Blog.Api.dll"]