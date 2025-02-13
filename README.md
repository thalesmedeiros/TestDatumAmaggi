# Datum.Blog.Api

## Visão Geral
Datum.Blog.Api é um projeto de CRUD em .NET 8 que segue a arquitetura limpa (Clean Architecture) e o padrão CQRS utilizando o MediatR. Este projeto utiliza um único banco de dados com a tabela `Lista de Tarefas`, além de incluir testes de unidade para garantir a qualidade e as melhores práticas do código.

### Tecnologias e Ferramentas Utilizadas
- .NET 8
- Docker & Docker Compose
- Clean Architecture
- CQRS com MediatR
- Swagger para documentação de API
- xUnit, FluentAssertions e Moq para testes
- Integração contínua e entrega contínua (CI/CD) com GitHub Actions
- Controle de versão com Git


## Estrutura do Projeto

A estrutura do projeto segue a organização em camadas para manter a separação de responsabilidades:

- **Datum.Blog.Api**: Contém os controladores de API e a configuração de endpoints.
- **Datum.Blog.Application**: Contém os serviços e casos de uso da aplicação, utilizando MediatR para consultas e comandos.
- **Datum.Blog.Domain**: Define as entidades, interfaces e lógica de domínio.
- **Datum.Blog.Infrastructure**: Implementa os repositórios e o acesso a dados.

## Configuração de Injeção de Dependência

A configuração da injeção de dependência utiliza extensões personalizadas para registrar todos os serviços e repositórios necessários, seguindo as melhores práticas de organização.

## Documentação da API com Swagger

A API está documentada com o Swagger para facilitar o entendimento e a interação com os endpoints. A documentação pode ser acessada na rota `/swagger` após iniciar a aplicação.

## Testes de Unidade

Os testes foram implementados utilizando **xUnit** e **FluentAssertions** para garantir a qualidade do código e cobrir os casos de uso dos serviços e repositórios.

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)

## Configuração do Banco de Dados

A aplicação utiliza o PostgreSQL como banco de dados. A configuração é feita através do arquivo `docker-compose.yml`, onde o banco de dados é configurado com um volume persistente.

## Execução

Para executar a aplicação, utilize o Docker Compose. No terminal, navegue até o diretório do projeto e execute:

```docker
docker-compose up --build
```

### Docker Compose
```docker
services:
  api:
    build:
      context: .
      dockerfile: Datum.Blog.Api/Dockerfile
    ports:
      - "5000:5000"      
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__DefaultConnection=Host=localhost;Database=datum;Username=admin;Password=yourpassword
    depends_on:
      - postgresdb

  postgresdb:
    image: postgres:latest
    environment:
      POSTGRES_USER: admin
      POSTGRES_PASSWORD: yourpassword
      POSTGRES_DB: datum
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data

volumes:
  postgres_data:
```

### Dockerfile
```docker
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
```

A aplicação estará disponível em `http://localhost:5000`.


### Deletar Pessoa

- **DELETE** `/person/delete/{id:guid}`
- **Response**: `202 Accepted` se a deleção for bem-sucedida ou `404 Not Found` se a pessoa não for encontrada.

### Logging

O logging é implementado através da injeção de dependência do `ILogger` nos serviços e handlers. Isso permite registrar informações sobre operações, como adições, atualizações e exclusões de pessoas.

## Licença

Este projeto está licenciado sob a MIT License. Veja o arquivo LICENSE para mais detalhes.