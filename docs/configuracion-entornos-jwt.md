# Configuración de entornos y JWT (estándar empresarial)

**Fecha:** 2025
**Motivo:** Error 500 al entrar a `/swagger` causado por `ArgumentNullException` en
`ServiceRegistration.cs` — `config["Authentication:SecretKey"]` era `null` porque el
proyecto no tenía `appsettings.json` base y el `SecretKey` estaba hardcodeado en
`appsettings.Development.json`.

---

## Estructura de configuración

| Archivo | ¿Se commitea? | Propósito |
|---|---|---|
| `SocialMediaApi/appsettings.json` | Sí | Config base. Defaults no sensibles. `SecretKey` y connection strings vacíos. |
| `SocialMediaApi/appsettings.Development.json` | Sí | Solo overrides locales (LocalDB, logging verbose). **Sin secretos.** |
| User Secrets (`secrets.json`) | No (fuera del repo) | `SecretKey` de desarrollo. |
| Variables de entorno | No | Secretos de producción. |

### Orden de precedencia (el último gana)

1. `appsettings.json`
2. `appsettings.{Environment}.json`
3. User Secrets (solo en Development)
4. Variables de entorno

---

## Cambios realizados

### 1. `SocialMediaApi/appsettings.json` (nuevo)

Archivo base commiteado con valores no sensibles:

```json
{
  "UseInMemoryDatabase": false,
  "ConnectionStrings": {
	"SocialMediaSomee": "",
	"SocialMediaHosting": ""
  },
  "Authentication": {
	"SecretKey": "",
	"Issuer": "LocalHostBackend",
	"Audience": "LocalHost",
	"DurationInMinutes": 60
  },
  "PaginationOptions": {
	"DefaultPageSize": 10,
	"DefaultPageNumber": 1
  },
  "Logging": {
	"LogLevel": {
	  "Default": "Information",
	  "Microsoft.AspNetCore": "Warning",
	  "Microsoft.EntityFrameworkCore.Database.Command": "Warning"
	}
  },
  "AllowedHosts": "*"
}
```

### 2. `SocialMediaApi/appsettings.Development.json`

Se eliminó el `SecretKey` hardcodeado (además contenía una clave PEM con espacios,
no válida como clave HMAC). Ahora solo tiene overrides locales:

```json
{
  "UseInMemoryDatabase": false,
  "ConnectionStrings": {
	"SocialMediaSomee": "Server=(localdb)\\MSSQLLocalDB;Database=SocialMediaYT;Integrated Security=true",
	"SocialMediaHosting": "Server=(localdb)\\MSSQLLocalDB;Database=SocialMediaYT;Integrated Security=true"
  },
  "Logging": {
	"LogLevel": {
	  "Default": "Information",
	  "Microsoft": "Information",
	  "Microsoft.Hosting.Lifetime": "Information",
	  "Microsoft.EntityFrameworkCore.Database.Command": "Warning"
	}
  }
}
```

### 3. User Secrets (desarrollo)

Se agregó `<UserSecretsId>socialmedia-api-dev-secrets</UserSecretsId>` a
`SocialMediaApi/SocialMediaApi.csproj` y se guardó el secreto fuera del repo:

```powershell
dotnet user-secrets set "Authentication:SecretKey" "<clave-dev-de-32+-caracteres>" --project SocialMediaApi
```

Los secretos se almacenan en `%APPDATA%\Microsoft\UserSecrets\socialmedia-api-dev-secrets\secrets.json`.
**Cada desarrollador del equipo debe ejecutar este comando una vez** tras clonar el repo.

### 4. Validación fail-fast en `SocialMedia.Infrastructure.Identity/ServiceRegistration.cs`

- La firma de `AddIdentityInfrastructureForApi` ahora recibe `IHostEnvironment`:
  `AddIdentityInfrastructureForApi(this IServiceCollection services, IConfiguration config, IHostEnvironment env)`.
- Al arrancar, la app valida y lanza `InvalidOperationException` con mensajes claros si falta:
  - La sección `Authentication` completa.
  - `Authentication:SecretKey` (y que tenga mínimo **32 caracteres** para HMAC-SHA256).
  - `Authentication:Issuer`.
  - `Authentication:Audience`.
- Los valores validados (`jwtSettings`) se usan en `TokenValidationParameters` en vez de
  leer `config[...]` directamente dentro del lambda de `AddJwtBearer`.
- `RequireHttpsMetadata = !env.IsDevelopment()` — `false` solo en desarrollo.

En `SocialMediaApi/Program.cs` la llamada quedó:

```csharp
builder.Services.AddIdentityInfrastructureForApi(builder.Configuration, builder.Environment);
```

### 5. `.gitignore`

- Se eliminó la regla `/SocialMediaApi/appsettings.json` (el base debe commitearse).
- Se agregaron reglas para proteger variantes con secretos:

```gitignore
# Environment-specific settings with secrets (base appsettings.json stays committed)
SocialMediaApi/appsettings.Production.json
SocialMediaApi/appsettings.Staging.json
```

---

## Producción

**No crear `appsettings.Production.json` con secretos.** Definir variables de entorno en
el servidor/servicio (nota el doble guion bajo `__` para las secciones anidadas):

```
Authentication__SecretKey=<clave real generada, ej. 64 chars aleatorios>
ConnectionStrings__SocialMediaSomee=<cadena de conexión de producción>
```

Opcionalmente también:

```
Authentication__Issuer=<issuer de producción>
Authentication__Audience=<audience de producción>
```

---

## Comportamiento resultante

- **Development:** la config se compone de `appsettings.json` + `appsettings.Development.json`
  + User Secrets. `/swagger` carga sin el error 500.
- **Producción:** la config se compone de `appsettings.json` + variables de entorno.
- **Config incompleta:** la app falla al arrancar con un mensaje indicando exactamente
  qué falta y cómo configurarlo, en vez del `ArgumentNullException` críptico en el
  primer request.

---

## Nota de seguridad

La clave PEM que estaba commiteada en `appsettings.Development.json` quedó expuesta en el
historial de git. Si esa clave se usaba para algo real, debe considerarse comprometida y
rotarse.
