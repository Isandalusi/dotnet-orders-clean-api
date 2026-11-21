# Orders Clean Architecture API (.NET 8 + EF Core + xUnit + Docker)

![CI - .NET 8](https://github.com/Isandalusi/dotnet-orders-clean-api/actions/workflows/dotnet-ci.yml/badge.svg)

API REST de gestión de órdenes diseñada bajo **Arquitectura Limpia (Clean Architecture)** en **C# / .NET 8**, con separación explícita de capas (**Domain**, **Application**, **Infrastructure**, **API**), persistencia con **Entity Framework Core + SQLite**, documentación **Swagger/OpenAPI**, pruebas automatizadas con **xUnit**, **Dockerfile** multi-stage e integración continua en **GitHub Actions**.

---

## Arquitectura del Proyecto
```text
dotnet-orders-clean-api/
├── src/
│   ├── Orders.Domain/          # Entidades y reglas invariantes de negocio (Order, OrderStatus)
│   ├── Orders.Application/     # Casos de uso, DTOs e interfaces de repositorio
│   ├── Orders.Infrastructure/  # AppDbContext (EF Core + SQLite) y OrderRepository
│   └── Orders.API/             # Controladores REST, DI, Swagger y healthcheck
├── tests/
│   └── Orders.Tests/           # Suite de pruebas unitarias e integración con xUnit + EF InMemory
├── .github/workflows/dotnet-ci.yml
└── Dockerfile
```

---

## Endpoints Principales
| Método | Ruta | Descripción |
|---|---|---|
| `GET` | `/health` | Verificación de estado del servicio |
| `GET` | `/api/orders` | Listado de órdenes (filtro opcional `?status=Pending`) |
| `GET` | `/api/orders/{id}` | Detalle de una orden por ID |
| `POST` | `/api/orders` | Alta de orden validando número único y monto > 0 |
| `POST` | `/api/orders/{id}/confirm` | Transición de estado `Pending -> Confirmed` |
| `POST` | `/api/orders/{id}/dispatch` | Transición de estado `Confirmed -> Dispatched` |

---

## Ejecución Local y Pruebas
```bash
# Ejecutar pruebas automatizadas con xUnit
dotnet test tests/Orders.Tests/Orders.Tests.csproj

# Levantar la API con Swagger
dotnet run --project src/Orders.API/Orders.API.csproj
```

### Con Docker
```bash
docker build -t dotnet-orders-clean-api .
docker run -p 8080:8080 dotnet-orders-clean-api
```

---

## Autor
**Jesús Vega Vicente** — Técnico Analista Programador & Lic. en Gestión Informática (UAI, tesis pendiente)  
- LinkedIn: [jesus-vega-a71399314](https://www.linkedin.com/in/jesus-vega-a71399314/)
- Portfolio Web: [docuflow-ai-pro.netlify.app](https://docuflow-ai-pro.netlify.app)
