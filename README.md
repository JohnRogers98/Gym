# Gym-buddy project
The core concept of this project is managing gym workouts. Admins can create calendar events, manage client`s accounts, register instructors. Instructors - create personal trainings, see their assigned events. Clients - see the account history, book and look for available events.

The project has three logical parts:
1. OAuth 2.0
2. WebApi
3. WebApplication

## OAuth 2.0
Gym-Buddy has implemented OAuth 2.0 specification with OpenId Connect extension. Gym.AuthorizationServer is the core part of that implementation with authorization_code flow as the main authorization method.

In order to use telegram WebApp was implemented custom Assertion method which takes initData sent by telegram validates it and authenticate a user.

To manage auth storage Gym.AuthorizationServer.Admin server is on duty. Currently, its main job is to create new users from. All events of user manipulation are sending to RabbitMQ message bus.

## WebApi
WebApi represents the core functionality of the application. It is divided into application, insfrastructure and domain projects. The Domain-Driven-Design pattern has been implemented.

## WebApplication
UI layer consists of blazor wasm application and BFF server. 

MudBlazor components are used by blazor extensively.

BFF stands for reverse proxy surrogate whose main job is to be API gateway and maintain jwt tokens. All endpoints are implemented individually for future extensibility.   


## Configuration

### WebApplication
[appsettings.json](./Gym.WebApplication/wwwroot/appsettings.json)

| Property | Description |
|----------|----------|
| `Bff:BaseUrl` | URL of the BFF server |

---

### BFF
[appsettings.json](./Gym.BFF/appsettings.json)

| Property | Description |
|----------|----------|
| `Urls:Spa:BaseUrl` | URL of the Web application. Used for CORS policy and callback endpoint |
| `Urls:WebApi:BaseUrl` | URL where Web API is hosted |
| `Urls:AuthorizationServer:BaseUrl` | URL where Auth Server is hosted |
| `Urls:AuthorizationServerAdminApi:BaseUrl` | URL where Auth Server Admin API is hosted |
| `ClientCredentials:RedirectUri` | Full URL for callback from Auth Server. Replace baseUrl with BFF URL |

---

### AuthorizationServer
[appsettings.json](./Gym.AuthorizationServer/appsettings.json)

| Property | Description |
|----------|----------|
| `CorsOrigins` | URLs of WebApplication and BFF respectively |
| `RabbitMQ` | RabbitMQ connection settings* |
| `MongoDb:ConnectionString` | Connection to auth-server database |
| `TelegramBot:Token` | Bot token for user data validation** |
| `Jwt:Issuer` | Base URL of Auth Server. Inserted into JWT tokens for 'issuer' claim validation |
| `Jwt:RsaKeyPath` | Absolute path to the generated RSA .Pem key for signing. Private key (public key is generated from it in the application stack). Requires manual generation |

---

### AuthorizationServer.Admin
[appsettings.json](./Gym.AuthorizationServer.Admin/appsettings.json)

| Property | Description |
|----------|----------|
| `AccessTokenIssuer` | Base URL of Auth Server (see AuthorizationServer Jwt:Issuer) |
| `RabbitMQ` | RabbitMQ connection settings* |
| `AuthorizationServer:BaseUrl` | Base URL of Auth Server |
| `MongoDb:ConnectionString` | Connection to Auth Server database (see AuthorizationServer MongoDb:ConnectionString) |

---

### WebApi
[appsettings.json](./Gym.WebApi/appsettings.json)

| Property | Description |
|----------|----------|
| `AccessTokenIssuer` | Base URL of Auth Server (see AuthorizationServer Jwt:Issuer) |
| `RabbitMQ` | RabbitMQ connection settings* |
| `AuthorizationServer:BaseUrl` | Base URL of Auth Server |
| `BffUrl` | URL of BFF server |
| `MongoDb:ConnectionString` | Connection to Gym API database |
| `TelegramBot:Token` | Bot token for sending notifications to users** |
| `Proxy` | Proxy settings*** |

---

### Примечания

- `*` — RabbitMQ connection is shared across all application parts.

- `**` — The token should be stored as a path variable under TelegramBot__Token. This path takes precedence over appsettings.json because path variables have higher priority, and the colon (':') in the configuration key is replaced with a double underscore ('__'). This behavior applies to all configuration settings.

- `***` — Proxy settings should also be stored in path variables at: Proxy__Host, Proxy__Port, Proxy__Login, Proxy__Password.
