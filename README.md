# Features
### Optimized data schemas
* [Entity Framework](https://github.com/dotnet/efcore) - Fully fledged object-database mapper for data manipulation.
* [Dapper](https://github.com/StackExchange/Dapper) - Light and simple object mapper for database reads. Using SQL gives access to complex query optimizaton mechanizms.

### Separation of concerns
* Segregating the read and write sides mean maintainable and flexible models. Most of the complex business logic goes into the write model. The read model can be relatively simple.

### Security
* It's easier to ensure that only the right domain entities are performing writes on the data.

### Independent scaling 
* Split to WRITE and READ databases comming soon

--------------

## Setup

1. Download and install [StackOverflow Database](https://www.brentozar.com/archive/2015/10/how-to-download-the-stack-overflow-database-via-bittorrent) - version `2018-06` or newer
2. Go to [appsettings.json](https://github.com/gs1993/SO/blob/master/SO/Api/appsettings.json) and set the connection string to database
3. Run project and go to app url: [http://localhost:5002](http://localhost:5002)
