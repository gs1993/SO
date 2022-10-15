# DDD and CQRS on a large-scale dataset

The demo app uses Entity Framework Core to manage 400 GB of data from the [StackOverflow Database](https://www.brentozar.com/archive/2015/10/how-to-download-the-stack-overflow-database-via-bittorrent) in a clean and consistent manner

## Architecture
![Architecture](https://github.com/gs1993/SO/blob/feature/readme/images/Architecture.PNG)

## Features

### Domain Driven Design

##### Bounded Contexts:
![Architecture](https://github.com/gs1993/SO/blob/feature/readme/images/ProjectLogic.PNG)

##### Rich domain model:<br />
![Entity](https://github.com/gs1993/SO/blob/feature/readme/images/PostEntity.PNG)

##### Value Objects:<br />
![ValueObject](https://github.com/gs1993/SO/blob/feature/readme/images/ProfileInfoValueObject.PNG)

### CQRS

##### Command-query separation:<br />
![CQRS](https://github.com/gs1993/SO/blob/feature/readme/images/Cqrs.PNG)

Read-write separation on application level:
1. Separate connection strings for read and write databases
2. Independent [Read](https://github.com/gs1993/SO/blob/feature/readme/SO/Logic/Utils/Db/ReadOnlyDatabaseContext.cs) and [write](https://github.com/gs1993/SO/blob/feature/readme/SO/Logic/Utils/Db/DatabaseContext.cs) db contexts
3. More efficient scaling on [micro](https://github.com/gs1993/SO/blob/feature/readme/SO/Logic/Utils/Db/DbExtensions.cs) and [macro](https://learn.microsoft.com/en-us/sql/relational-databases/replication/sql-server-replication?view=sql-server-ver16) levels

#### Tests
![Tests](https://github.com/gs1993/SO/blob/feature/readme/images/Tests.PNG)

1. [Unit tests](https://github.com/gs1993/SO/blob/feature/readme/SO/Tests/UnitTests/Logic/Posts/PostTests.cs) - in-depth tests for complex domain logic
2. [Integration tests](https://github.com/gs1993/SO/blob/feature/readme/SO/Tests/IntegrationTests/Posts/PostControllerIntegrationTests.cs) - api-level test with a separate database created dynamically using Docker
3. [Benchmark test](https://github.com/gs1993/SO/blob/feature/readme/SO/Tests/BenchmarkTests/APIs/RestBenchmarks.cs) - comparison of REST and GraphQL performance


#### APIs

[REST](https://github.com/gs1993/SO/tree/master/SO/Api/Controllers) - with [Swagger](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) documentation

[GraphQL](https://github.com/gs1993/SO/tree/master/SO/Api/GraphQL) - using [Grpc.AspNetCore](https://github.com/grpc/grpc-dotnet)

[gRPC](https://github.com/gs1993/SO/tree/master/SO/Api/Grpc) - using [Google.Protobuf](https://github.com/protocolbuffers/protobuf)

--------------

## Setup

1. Download and install [StackOverflow Database](https://www.brentozar.com/archive/2015/10/how-to-download-the-stack-overflow-database-via-bittorrent) - version `2018-06` or newer
2. Go to [appsettings.json](https://github.com/gs1993/SO/blob/feature/readme/SO/Api/appsettings.json) and set the connection strings to database
3. Run database migrations
```cmd
dotnet ef database update
```
4. Run project and go to app url: [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)
