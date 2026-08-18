---
title: Postgres
---

# Postgres Docker

First, we need to get the Postgres Database up and running.

Install the Docker Desktop App, create the docker-compose.yml and .env file and run the npm command in the same folder:

```tsx
docker-compose up -D
```

```tsx title="docker-compose.yml"
version: '3.8'
services:
  db:
    image: postgres:latest
    container_name: postgres_use_case
    environment:
      POSTGRES_PASSWORD: ${POSTGRES_PASSWORD}
      POSTGRES_USER: ${POSTGRES_USER}
      POSTGRES_DB: ${POSTGRES_DB}
    ports:
      - ${POSTGRES_PORT}
    volumes:
      - ./data:/var/lib/postgresql/data
    networks:
      - postgres_network

networks:
  postgres_network:
    driver: bridge
```
```tsx title=".env"
POSTGRES_PASSWORD=postgres_pw
POSTGRES_USER=postgres_user
POSTGRES_DB=use_case_db
POSTGRES_PORT=5432:5432
```

Connection string is saved in the ASP.NET project under appsettings.json

```tsx title="appsettings.json"
"ConnectionStrings": {
  "UseCasePostgres": "Host=localhost;Port=5432;Username=postgres_user;Password=postgres_pw;Database=postgres;"
}
```