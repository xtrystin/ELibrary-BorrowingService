### ELibrary-BorrowingService
This service is part of **ELibrary Project** - to read more please scroll down

## ELibrary Project

### Live  Preview
https://elibrarypk-76b898964c75.herokuapp.com/

### User Manual
[UserManual.md](/docs/usermanual.md)

### Services Overview

| Name  |  Repo Link | Functionality |
|---|---|---|
| BookService  | [BookService](https://github.com/xtrystin/ELibrary-BookService)  | Manage Books, Tags, Categories, Authors  |
|  UserService | [UserService](https://github.com/xtrystin/ELibrary-UserService)  | Get, modify, block users, manage user' sfavourite book lists  |
|  AuthService | [AuthService](https://github.com/xtrystin/ELibrary-AuthService)  | Register, Add to Role, Login - JWT generation  |
|  ApiGateway | [ApiGateway](https://github.com/xtrystin/ELibrary-ApiGateway)  | public gateway for frontend app to services in VPN   |
|  BorrowingService | [BorrowingService](https://github.com/xtrystin/ELibrary-BorrowingService)   | Borrow, Book a book   |
|  NewsService      | [NewsService](https://github.com/mateusztoczek/ELibrary-NewsService)        | Manage news           |
| K8sScripts  | [ELibraryK8s](https://github.com/DyremiX/Elibrary_k8s)  | Kubernetes scripts  |
| Frontend  | [ELibraryFront](https://github.com/jonaszor/eBiblioteka)  | React frontend app  |

### System Architecture
![Pasted image 20230526225129](https://github.com/xtrystin/ELibrary-BorrowingService/assets/33805319/a04fa4c1-e140-4d82-bc4c-219c33530d91)

_Updated system architecture_
![Pasted image 20230612235536](https://github.com/xtrystin/ELibrary-BorrowingService/assets/33805319/02016500-b74b-4f53-a28e-680155deadc8)

### Class Diagram
![Diagram_klas](https://github.com/xtrystin/ELibrary-BorrowingService/assets/33805319/48fe1ba0-6ad4-4bb0-8aa5-31ceb91876f8)

_Final class diagrams_
1. BorrowingService
![borrowingServiceWithoutWatermark](https://github.com/xtrystin/ELibrary-BorrowingService/assets/33805319/f4444033-4ad7-4b3b-ae1e-21390f18b657)

2. BookService
![BookServiceWithoutWatermark](https://github.com/xtrystin/ELibrary-BorrowingService/assets/33805319/b7ef52ba-9594-423f-b343-6d72af09dcd3)

3. UserService
![userServiceWithoutWatermark](https://github.com/xtrystin/ELibrary-BorrowingService/assets/33805319/039ce2af-0dc1-4c58-99f8-a13ba952ff99)


### Services Description


#### Service Architecture
_Component diagram_
![Pasted image 20230612235951](https://github.com/xtrystin/ELibrary-BorrowingService/assets/33805319/760f0a6a-9b5f-4410-9067-e8a6c348172e)

![Pasted image 20230603203510](https://github.com/xtrystin/ELibrary-BorrowingService/assets/33805319/155b42b9-075f-49f3-8214-c9727071f860)

#### Command Query Separation
![Pasted image 20230515214222](https://github.com/xtrystin/ELibrary-BorrowingService/assets/33805319/e94845be-e4b2-44fe-a58b-bcee6fe21e76)

#### Domain
![Pasted image 20230515214353](https://github.com/xtrystin/ELibrary-BorrowingService/assets/33805319/6d70f89a-8cf2-4c42-b5dd-c1db4330b1c3)

#### Error Handling
- custom exception are catched in a first in request pipeline middleware and mapped to appropriate http status code

#### API Middlewares
- ErrorHandler -> httpsRedirection -> Cors Policy -> authentication -> authorization -> endpoint routing

#### JWT Authorization
- AuthService generates token for react app
- token is validated in each service

#### Database - PostGresql
- Code First using ORM - Entity Framework
- Each microservice has own scheme, ex: bookService's tables are in *bookService* scheme
- On Read Side we use Dapper microORM. Service has access to every scheme in database
- Migrations are stored in each service in directory *Migrations*

#### Unit Tests
- NUnit Framework

#### Jobs
- Quartz library
- Jobs: DeactivateBookingsJob in BorrowingService - deactivates active bookings, which exceeded booking's limit date  Run every day,  

#### EntityRepository - custom repository base class
- generic base class for Repositories
- Inheritance to prevent duplicated code
- ability to easily add new Repositories and custom methods to them 

![EntityRepository](https://github.com/xtrystin/ELibrary-BorrowingService/assets/33805319/96b433b5-6410-4696-a658-279df5fd41fe)



### Asynchronous Communication between Services
- To publish or consume message we use **MassTransit** library, which provides an abstraction over message broker
- Services are compatible with **RabbitMq** and **Azure Service Bus**
- We can change a message broker through changing environment variable **Flags__UseRabbitMq**
- In case of Azure Service Bus we use Basic Price Tier (only **1-1 queues** are available), so we have to create a unique message for each subscriber( example: instead of sending **one UserCreated** -> we have to send 2 messages: **UserCreatedU** for UserService and **UserCreatedB** for BorrowingService) 



#### Diagram

todo - update diagram based on messages table
![Pasted image 20230526225225](https://github.com/xtrystin/ELibrary-BorrowingService/assets/33805319/c9f10fb2-ebdf-44a1-9c81-c6b71bb99564)

#### Messages Table

| Message  |  Publishers | Subscribers | Description |
|---|---|---|---|
| BookCreated  | [BookService](https://github.com/xtrystin/ELibrary-BookService)  |  [UserService](https://github.com/xtrystin/ELibrary-UserService) , [BorrowingService](https://github.com/xtrystin/ELibrary-BorrowingService)| Book has been created  |
|  BookRemoved | [BookService](https://github.com/xtrystin/ELibrary-BookService)  |  [UserService](https://github.com/xtrystin/ELibrary-UserService) , [BorrowingService](https://github.com/xtrystin/ELibrary-BorrowingService) | Book has been deleted|
|  BookAvailabilityChanged |  [BookService](https://github.com/xtrystin/ELibrary-BookService),  [BorrowingService](https://github.com/xtrystin/ELibrary-BorrowingService) |   [BookService](https://github.com/xtrystin/ELibrary-BookService),  [BorrowingService](https://github.com/xtrystin/ELibrary-BorrowingService) | Book availability amount has been changed (book has been borrowed, returned, booked, unbooked, new books has been delivered to the library) |
|  UserCreated | [AuthService](https://github.com/xtrystin/ELibrary-AuthService)  |  [UserService](https://github.com/xtrystin/ELibrary-UserService), [BorrowingService](https://github.com/xtrystin/ELibrary-BorrowingService)   | User has created a new account|
| UserDeleted  | [AuthService](https://github.com/xtrystin/ELibrary-AuthService)  |  [UserService](https://github.com/xtrystin/ELibrary-UserService), [BorrowingService](https://github.com/xtrystin/ELibrary-BorrowingService)  | User account has been deleted|
|  UserBlocked | [UserService](https://github.com/xtrystin/ELibrary-UserService)  | [BorrowingService](https://github.com/xtrystin/ELibrary-BorrowingService)  | User account has been blocked (account amount to pay > 200)|
|  UserUnblocked |  [UserService](https://github.com/xtrystin/ELibrary-UserService)  | [BorrowingService](https://github.com/xtrystin/ELibrary-BorrowingService)  | User account has been unblocked|
|  OvertimeReturn |  [BorrowingService](https://github.com/xtrystin/ELibrary-BorrowingService) | [UserService](https://github.com/xtrystin/ELibrary-UserService)  | Book has been returned after deadline|


#### Messages Structure
1. BookCreated
```cs
public class BookCreated
{
    public int BookId { get; set; }
	public int Amount { get; set; }
}
```

2. BookRemoved
```cs
public class BookRemoved
{
    public int BookId { get; set; }
}
```

3. BookAvailabilityChanged
```cs
public class BookAvailabilityChanged
{
	public int BookId { get; set; }
	public int Amount { get; set; }
}
```
4. UserCreated
```cs
public class UserCreated
{
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
```
5. UserDeleted
```cs
public class UserDeleted
{
    public string UserId { get; set; }
}
```
6. UserBlocked
```cs
public class UserBlocked
{
    public string UserId { get; set; }
}

```
7. UserUnblocked
```cs
public class UserUnblocked
{
    public string UserId { get; set; }
}
```
8. OvertimeReturn
```cs
public class OvertimeReturn
{
    public string UserId { get; set; }
    public decimal AmountToPay { get; set; }
}
```

#### MessagePublisher class
- message is published to RabbitMq or send to queues in  ASB
- we use Basic Tier Azure Service Bus (only 1-1 queues are available), so we need to send message to each consumer instead of published one time
```cs
 public async Task Publish<T>(T message)
    {
        if (_configuration["Flags:UserRabbitMq"] == "1")
        {
            await _bus.Publish(message);
        }
        else
        {
            // Publisg to many queues -> because Basic Tier ASB allowed only 1-1 queues, no topics
            if (message is UserCreated)
            {
                var m = message as UserCreated;
                var userServiceMessage = new UserCreatedU() { UserId = m.UserId, 
	                FirstName = m.FirstName, LastName = m.LastName };
                var borrowingServiceMessage = new UserCreatedB() { UserId = m.UserId, 
	                FirstName = m.FirstName, LastName = m.LastName };

                await _bus.Send(userServiceMessage);
                await _bus.Send(borrowingServiceMessage);
            }
            else if (message is UserDeleted)
            {
                var m = message as UserDeleted;
                var userServiceMessage = new UserDeletedU() { UserId = m.UserId };
                var borrowingServiceMessage = new UserDeletedB() { UserId = m.UserId };

                await _bus.Send(userServiceMessage);
                await _bus.Send(borrowingServiceMessage);
            }
            else
            {
                await _bus.Send(message);   // send to one queue
            }
        }
```

#### ServiceBus Configuration using MassTransit
- message broker is chosen based on environment variable *Flags:UseRabbitMq*
```cs
public static IServiceCollection AddServiceBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            if (configuration["Flags:UseRabbitMq"] == "1")   //todo change to preprocessor directive #if
            {
                RabbitMqOptions rabbitMqOptions = configuration.GetSection(nameof(RabbitMqOptions)).Get<RabbitMqOptions>();

                x.UsingRabbitMq((hostContext, cfg) =>
                {
                    cfg.Host(rabbitMqOptions.Uri, "/", c =>
                    {
                        c.Username(rabbitMqOptions.UserName);
                        c.Password(rabbitMqOptions.Password);
                    });
                });
            }
            else
            {
                // Azure Basic Tier - only 1-1 queues
                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host(configuration["AzureServiceBusConnectionString"]);

                    /// Publishers configuration ///
                    // UserCreated
                    EndpointConvention.Map<UserCreatedU>(new Uri($"queue:{nameof(UserCreatedU)}"));
                    cfg.Message<UserCreatedU>(cfgTopology => cfgTopology.SetEntityName(nameof(UserCreatedU)));
                    EndpointConvention.Map<UserCreatedB>(new Uri($"queue:{nameof(UserCreatedB)}"));
                    cfg.Message<UserCreatedB>(cfgTopology => cfgTopology.SetEntityName(nameof(UserCreatedB)));

                    // UserDeleted
                    EndpointConvention.Map<UserDeletedU>(new Uri($"queue:{nameof(UserDeletedU)}"));
                    cfg.Message<UserDeletedU>(cfgTopology => cfgTopology.SetEntityName(nameof(UserDeletedU)));
                    EndpointConvention.Map<UserDeletedB>(new Uri($"queue:{nameof(UserDeletedB)}"));
                    cfg.Message<UserDeletedB>(cfgTopology => cfgTopology.SetEntityName(nameof(UserDeletedB)));
                });
            }

        });

        return services;
    }
```


### CI/CD Pipelines on Github Action

git push -> github action -> docker hub -> azure app service
```yaml
name: UserServicePipeline
on:
  push:
    branches:
      - "main"
    paths:
      - "src/**"

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      -
        name: Checkout
        uses: actions/checkout@v3
      -
        name: Login to Docker Hub
        uses: docker/login-action@v2
        with:
          username: ${{ secrets.DOCKERHUB_USERNAME }}
          password: ${{ secrets.DOCKERHUB_TOKEN }}
      -
        name: Set up Docker Buildx
        uses: docker/setup-buildx-action@v2
      -
        name: Build and push
        uses: docker/build-push-action@v4
        with:
          context: .
          file: src/API/Dockerfile
          push: true
          tags: ${{ secrets.DOCKERHUB_USERNAME }}/elib-userservice:latest
```


### Environments
1. Azure - App Services, PostgreSQL DB, Azure Service Bus
2. Kubernetes
