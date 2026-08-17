<<<<<<< HEAD
# Gym
```
Gym
├─ Dockerfile
├─ Gym.Application
│  ├─ DependencyInjection.cs
│  ├─ Extensions
│  │  └─ MappingExtensions.cs
│  ├─ Gym.Application.csproj
│  └─ Services
│     ├─ CalendarEventApi
│     │  ├─ CalendarEventDetails.cs
│     │  ├─ CreateCalendarEvent
│     │  │  ├─ CreateCalendarEventCommand.cs
│     │  │  └─ CreateCalendarEventHandler.cs
│     │  ├─ GetAllCalendarEvents
│     │  │  ├─ GetAllCalendarEventsHandler.cs
│     │  │  └─ GetAllCalendarEventsQuery.cs
│     │  └─ GetCalendarEventById
│     │     ├─ GetCalendarEventByIdHandler.cs
│     │     └─ GetCalendarEventByIdQuery.cs
│     ├─ DomainEventPublisher
│     │  ├─ DomainEventNotification.cs
│     │  ├─ DomainEventPublisher.cs
│     │  └─ IDomainEventPublisher.cs
│     ├─ InstructorApi
│     │  ├─ CreateInstructor
│     │  │  ├─ CreateInstructorCommand.cs
│     │  │  └─ CreateInstructorHandler.cs
│     │  ├─ GetAllInstructors
│     │  │  ├─ GetAllInstructorsHandler.cs
│     │  │  └─ GetAllInstructorsQuery.cs
│     │  ├─ GetInstructorById
│     │  │  ├─ GetInstructorByIdHandler.cs
│     │  │  └─ GetInstructorByIdQuery.cs
│     │  └─ InstructorDetails.cs
│     ├─ TrainingApi
│     │  ├─ CreateTraining
│     │  │  ├─ CreateTrainingCommand.cs
│     │  │  └─ CreateTrainingHandler.cs
│     │  ├─ GetAllTrainings
│     │  │  ├─ GetAllTrainingsHandler.cs
│     │  │  └─ GetAllTrainingsQuery.cs
│     │  ├─ GetTrainingById
│     │  │  ├─ GetTrainingByIdHandler.cs
│     │  │  └─ GetTrainingByIdQuery.cs
│     │  └─ TrainingDetails.cs
│     └─ UserApi
│        ├─ Events
│        │  └─ NotifyUserAboutRegistrationHandler.cs
│        ├─ TelegramAuthentication
│        │  ├─ AuthenticateUserCommand.cs
│        │  └─ AuthenticateUserHandler.cs
│        └─ UserDetails.cs
├─ Gym.Application.Tests
│  └─ Gym.Application.Tests.csproj
├─ Gym.CompositionRoot
│  ├─ Extensions
│  │  └─ ServiceCollectionExtensions.cs
│  └─ Gym.CompositionRoot.csproj
├─ Gym.Domain
│  ├─ AssemblyInfo.cs
│  ├─ BookingAggregate
│  │  ├─ Booking.cs
│  │  ├─ BookingId.cs
│  │  ├─ BookingStatus.cs
│  │  ├─ Errors
│  │  │  └─ IncorrectBookingStatusStateError.cs
│  │  ├─ Events
│  │  │  ├─ TrainingBookedDomainEvent.cs
│  │  │  ├─ TrainingBookingCancelledDomainEvent.cs
│  │  │  ├─ TrainingCompletedDomainEvent.cs
│  │  │  └─ TrainingRebookedDomainEvent.cs
│  │  ├─ IBookingQueryService.cs
│  │  └─ IBookingRepository.cs
│  ├─ CalendarEventAggregate
│  │  ├─ CalendarEvent.cs
│  │  ├─ Errors
│  │  │  ├─ EventHasNotFreeSpaceError.cs
│  │  │  ├─ EventTimeHasExpired.cs
│  │  │  └─ UserAlreadyBookedError.cs
│  │  ├─ ICalendarEventQueryService.cs
│  │  ├─ ICalendarEventRepository.cs
│  │  ├─ InstructorInfo.cs
│  │  └─ TrainingInfo.cs
│  ├─ ClientAggregate
│  │  ├─ Client.cs
│  │  ├─ ClientId.cs
│  │  ├─ Events
│  │  │  └─ CreatedNewClientDomainEvent.cs
│  │  ├─ IClientQueryService.cs
│  │  └─ IClientRepository.cs
│  ├─ Gym.Domain.csproj
│  ├─ InstructorAggregate
│  │  ├─ IInstructorQueryService.cs
│  │  ├─ IInstructorRepository.cs
│  │  ├─ Instructor.cs
│  │  └─ InstructorId.cs
│  ├─ TrainingAggregate
│  │  ├─ ITrainingQueryService.cs
│  │  ├─ ITrainingRepository.cs
│  │  ├─ Training.cs
│  │  └─ TrainingId.cs
│  ├─ UserAggregate
│  │  ├─ Authentication
│  │  │  ├─ ITelegramSignatureVerifier.cs
│  │  │  └─ ValidatedTelegramUserInfo.cs
│  │  ├─ Errors
│  │  │  └─ TelegramInitDataInvalidHashError.cs
│  │  ├─ INotificationService.cs
│  │  ├─ IUserQueryService.cs
│  │  ├─ IUserRepository.cs
│  │  ├─ TelegramId.cs
│  │  ├─ User.cs
│  │  └─ UserRole.cs
│  ├─ _Common
│  │  ├─ AggregateRoot.cs
│  │  ├─ DomainError.cs
│  │  ├─ DomainEvent.cs
│  │  ├─ IUnitOfWork.cs
│  │  └─ Result.cs
│  ├─ _Exceptions
│  │  └─ DomainException.cs
│  └─ _Shared
│     ├─ CalendarEventId.cs
│     ├─ Services
│     │  └─ TrainingBookingService.cs
│     └─ UserId.cs
├─ Gym.Domain.Tests
│  ├─ BookingAggregateTests.cs
│  ├─ CalendarEventTests.cs
│  └─ Gym.Domain.Tests.csproj
├─ Gym.Infrastructure
│  ├─ Configurations
│  │  └─ MongoDbOptions.cs
│  ├─ DependencyInjection.cs
│  ├─ Entities
│  │  ├─ Extensions
│  │  │  ├─ Mappings
│  │  │  │  ├─ BookingExtensions.cs
│  │  │  │  ├─ CalendarEventExtensions.cs
│  │  │  │  ├─ ClientExtensions.cs
│  │  │  │  ├─ InstructorExtensions.cs
│  │  │  │  ├─ TrainingExtensions.cs
│  │  │  │  └─ UserExtensions.cs
│  │  │  └─ StringExtensions.cs
│  │  ├─ MongoUnitOfWork.cs
│  │  └─ Repositories
│  │     ├─ Bookings
│  │     │  ├─ BookingEntity.cs
│  │     │  └─ BookingRepository.cs
│  │     ├─ CalendarEvents
│  │     │  ├─ CalendarEventEntity.cs
│  │     │  └─ CalendarEventRepository.cs
│  │     ├─ Clients
│  │     │  ├─ ClientEntity.cs
│  │     │  └─ ClientRepository.cs
│  │     ├─ Instructors
│  │     │  ├─ InstructorEntity.cs
│  │     │  └─ InstructorRepository.cs
│  │     ├─ Trainings
│  │     │  ├─ TrainingEntity.cs
│  │     │  └─ TrainingRepository.cs
│  │     └─ Users
│  │        ├─ UserEntity.cs
│  │        └─ UserRepository.cs
│  ├─ Gym.Infrastructure.csproj
│  └─ Telegram
│     ├─ TelegramBotNotificationService.cs
│     ├─ TelegramBotToken.cs
│     └─ TelegramSignatureVerifier.cs
├─ Gym.Infrastructure.Tests
│  ├─ Gym.Infrastructure.Tests.csproj
│  └─ Telegram
│     └─ TelegramSignatureVerifierTests.cs
├─ Gym.WebApi
│  ├─ Controllers
│  │  └─ Api
│  │     ├─ CalendarEvents
│  │     │  ├─ CreateCalendarEventController.cs
│  │     │  ├─ GetCalendarEventController.cs
│  │     │  └─ ListCalendarEventsController.cs
│  │     ├─ Instructors
│  │     │  ├─ CreateInstructorController.cs
│  │     │  ├─ GetInstructorController.cs
│  │     │  └─ ListInstructorsController.cs
│  │     ├─ Trainings
│  │     │  ├─ CreateTrainingController.cs
│  │     │  ├─ GetTrainingController.cs
│  │     │  └─ ListTrainingsController.cs
│  │     └─ Users
│  │        ├─ Jwt
│  │        │  └─ IAccessTokenGenerator.cs
│  │        └─ WebAppAuthController.cs
│  ├─ Extensions
│  │  ├─ CorsPolicy.cs
│  │  ├─ SecurityPolicy.cs
│  │  └─ ServiceCollectionExtensions.cs
│  ├─ Gym.WebApi.csproj
│  ├─ Gym.WebApi.http
│  ├─ Mappings
│  │  └─ DtoMappings.cs
│  ├─ Program.cs
│  ├─ Properties
│  │  └─ launchSettings.json
│  ├─ appsettings.Development.json
│  └─ appsettings.json
├─ Gym.WebApplication
│  ├─ App.razor
│  ├─ Extensions
│  │  ├─ DateTimeExtensions.cs
│  │  └─ ServiceCollectionExtensions.cs
│  ├─ Features
│  │  ├─ Calendar
│  │  │  ├─ CalendarPage.razor
│  │  │  ├─ ItemCard.razor
│  │  │  ├─ ItemsContainer.razor
│  │  │  ├─ Picker.razor
│  │  │  └─ Services
│  │  │     ├─ CalendarService.cs
│  │  │     └─ ICalendarService.cs
│  │  ├─ Home
│  │  │  └─ HomePage.razor
│  │  ├─ Login
│  │  │  ├─ MockedWebAppInitData.cs
│  │  │  ├─ MockedWebAppLogin.razor
│  │  │  └─ Services
│  │  │     ├─ IWebAppAuthService.cs
│  │  │     ├─ WebAppAuthService.cs
│  │  │     └─ WebAppAuthStateProvider.cs
│  │  └─ NotFound
│  │     └─ NotFound.razor
│  ├─ Gym.WebApplication.csproj
│  ├─ Mappings
│  │  └─ DtoMapping.cs
│  ├─ Program.cs
│  ├─ Properties
│  │  └─ launchSettings.json
│  ├─ ViewModels
│  │  ├─ CalendarItemViewModel.cs
│  │  ├─ InstructorViewModel.cs
│  │  └─ TrainingViewModel.cs
│  ├─ _Imports.razor
│  └─ wwwroot
│     ├─ css
│     │  └─ app.css
│     ├─ favicon.png
│     ├─ icon-192.png
│     └─ index.html
├─ Gym.WebDto
│  ├─ Dto
│  │  ├─ CalendarEventDto.cs
│  │  ├─ InstructorDto.cs
│  │  └─ TrainingDto.cs
│  ├─ Gym.WebDto.csproj
│  ├─ Requests
│  │  ├─ CalendarEvent
│  │  │  └─ CreateCalendarEventRequest.cs
│  │  ├─ Instructor
│  │  │  └─ CreateInstructorRequest.cs
│  │  ├─ Training
│  │  │  └─ CreateTrainingRequest.cs
│  │  └─ Users
│  │     └─ WebAppAuthRequest.cs
│  └─ Responses
│     ├─ CalendarEvent
│     │  ├─ CreateCallendarEventResponse.cs
│     │  └─ GetCalendarEventResponse.cs
│     ├─ Instructor
│     │  ├─ CreateInstructorResponse.cs
│     │  └─ GetInstructorResponse.cs
│     ├─ ListResponse.cs
│     ├─ Training
│     │  ├─ CreateTrainingResponse.cs
│     │  └─ GetTrainingResponse.cs
│     └─ Users
│        └─ WebAppAuthResponse.cs
├─ Gym.sln
├─ Gym.slnLaunch
├─ Gym.slnx
├─ README.md
└─ docker-compose.yml

```
=======
# Gym-buddy project
The core concept of this project is managing gym workouts. Admins can create calendar events, manage client`s accounts, register instructors. Instructors - create personal trainings, see their assigned events. Clients - see the account history, book and look for available events.

The project has three logical parts:
1. OAuth 2.0
2. WebApi
3. WebApplication

## OAuth 2.0
Gym-Buddy has implemented OAuth 2.0 specification with OpenId Connect extension. Gym.AuthorizationServer is the core part of that implementation with authorization_code flow as the main authorization method.

In order to use telegram WebApp custom Assertion method was implemented which takes initData sent by telegram, validates it and authenticate a user.

To manage auth storage Gym.AuthorizationServer.Admin server is on duty. Currently, its main job is to create new users. All events of user manipulation are sending to RabbitMQ message bus.

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

### Annotations

- `*` — RabbitMQ connection is shared across all application parts.

- `**` — The token should be stored as a path variable under TelegramBot__Token. This path takes precedence over appsettings.json because path variables have higher priority, and the colon (':') in the configuration key is replaced with a double underscore ('__'). This behavior applies to all configuration settings.

- `***` — Proxy settings should also be stored in path variables at: Proxy__Host, Proxy__Port, Proxy__Login, Proxy__Password.
>>>>>>> master
