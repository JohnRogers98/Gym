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