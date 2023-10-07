# DDD and CQRS on a large-scale dataset

The technology demonstration app uses Entity Framework Core to manage <b>400GB</b> of data from the [StackOverflow Database](https://www.brentozar.com/archive/2015/10/how-to-download-the-stack-overflow-database-via-bittorrent) in a clean and consistent manner 

[![Unit tests](https://github.com/gs1993/SO/actions/workflows/unit-tests.yml/badge.svg?branch=master)](https://github.com/gs1993/SO/actions/workflows/unit-tests.yml)

![Mutation tests score](https://img.shields.io/badge/Mutation%20Score-00.00%25-red.svg)

## Table of Contents
- [Architecture](#architecture)
- [Features](#features)
  * [CQRS](#cqrs)
  * [Tests](#tests)
  * [Domain Driven Design](#domain-driven-design)
  * [APIs](#apis)
  * [Machine Learning](#machine-learning)
  * [Feature flags](#feature-flags)
- [Setup](#setup)

## Architecture
![Architecture](https://github.com/gs1993/SO/blob/master/images/Architecture2.PNG)

## Features

### CQRS

##### Command-query separation:<br />
![CQRS](https://github.com/gs1993/SO/blob/master/images/Cqrs2.PNG)

Read-write separation on application level:
1. Separate connection strings for read and write databases
2. Independent [Read](https://github.com/gs1993/SO/blob/master/SO/Logic/Utils/Db/ReadOnlyDatabaseContext.cs) and [write](https://github.com/gs1993/SO/blob/master/SO/Logic/Utils/Db/DatabaseContext.cs) db contexts
3. More efficient scaling on [micro](https://github.com/gs1993/SO/blob/master/SO/Logic/Utils/Db/DbExtensions.cs) and [macro](https://learn.microsoft.com/en-us/sql/relational-databases/replication/sql-server-replication?view=sql-server-ver16) levels

##### EF Compiled Queries:
Employing [Compiled Queries](https://github.com/gs1993/SO/blob/master/SO/Logic/Read/Posts/Queries/GetLastestPostsQuery.cs) serves as an effective strategy to significantly boost the performance on the data retrieval side of the application.

### Tests
![Tests](https://github.com/gs1993/SO/blob/master/images/Tests.PNG)

1. [Unit tests](https://github.com/gs1993/SO/blob/master/SO/Tests/UnitTests/Logic/Posts/PostTests.cs) - in-depth tests for complex domain logic
2. [Integration tests](https://github.com/gs1993/SO/blob/master/SO/Tests/IntegrationTests/Posts/PostControllerIntegrationTests.cs) - api-level test with a separate database created dynamically using Docker
3. [Benchmark test](https://github.com/gs1993/SO/blob/master/SO/Tests/BenchmarkTests/APIs/RestBenchmarks.cs) - comparison of REST and GraphQL performance

### Domain Driven Design

##### Bounded Contexts:
![Architecture](https://github.com/gs1993/SO/blob/master/images/ProjectLogic2.PNG)

##### Rich domain model:<br />
![Entity](https://github.com/gs1993/SO/blob/master/images/PostEntity.PNG)

##### Value Objects:<br />
![ValueObject](https://github.com/gs1993/SO/blob/master/images/ProfileInfoValueObject.PNG)


### APIs

[REST](https://github.com/gs1993/SO/tree/master/SO/Api/Controllers) - with [Swagger](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) documentation

[GraphQL](https://github.com/gs1993/SO/tree/master/SO/Api/GraphQL) - using [Grpc.AspNetCore](https://github.com/grpc/grpc-dotnet)

[gRPC](https://github.com/gs1993/SO/tree/master/SO/Api/Grpc) - using [Google.Protobuf](https://github.com/protocolbuffers/protobuf)


### Machine Learning
Spam detection is accomplished through a machine learning model trained on a large dataset of previous posts from a database. [The project](https://github.com/gs1993/SO/tree/master/SO/Services/PostContentEvaluator/PostScoreEvaluationEngine.cs) harnesses the power of the [ML.NET](https://github.com/dotnet/machinelearning) library to efficiently analyze and identify spam, ensuring a high level of accuracy and reliability.

#### Training results:
![Training_results](https://github.com/gs1993/SO/blob/master/images/Training_results.PNG)

### Feature flags
Feature flags provide a easy way to turn features on or off dynamically.
[FeatureFlagController.cs](https://github.com/gs1993/SO/blob/master/SO/Api/Controllers/FeatureFlagsDemoController.cs) - demo edpoints
[FeatureFlags.cs](https://github.com/gs1993/SO/blob/master/SO/Logic/Read/FeatureFlagsDemo/FeatureFlag.cs) - logic for the flags
[appsettings.json](https://github.com/gs1993/SO/blob/master/SO/Api/appsettings.json) - set by "FeatureFlags" section in appsettings

--------------

## Setup

### Api

1. Download and install [StackOverflow Database](https://www.brentozar.com/archive/2015/10/how-to-download-the-stack-overflow-database-via-bittorrent) - version `2018-06` or newer
2. Go to [Api/appsettings.json](https://github.com/gs1993/SO/blob/master/SO/Api/appsettings.json) and set the connection strings to database
3. Run database migrations
```cmd
dotnet ef database update
```
4. Run project and go to app url: [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)

### Machine learning models

1. Go to [ModelBuilder/appsettings.json](https://github.com/gs1993/SO/blob/master/SO/Services/MachineLearning/PostContentEvaluator/appsettings.json). Set `ConnectionString` to database and `SaveModelPath` - (path to generate ML model)
2. Run `ModelBuilder` in release mode and wait for ML model to generate .zip file in specified location
3. Go to [PredictionEngineApi/appsettings.json](https://github.com/gs1993/SO/blob/master/SO/Services/MachineLearning/PredictionEngineApi/appsettings.json). Set `PredictionModelPath` (path and name of generated ML model)
4. Run project and go to app url: [http://localhost:5001/swagger/index.html](http://localhost:5001/swagger/index.html)
5. aa docker-compose -f elasticsearch-kibana-docker-compose.yaml up -d
