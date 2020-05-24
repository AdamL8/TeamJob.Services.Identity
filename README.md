**What is TeamJob.Services.Identity?**
----------------

TeamJob.Services.Identity is the microservice being part of [TeamJob] solution.

**How to start the application?**
----------------

Service can be started locally via `dotnet run` command (executed in the `/src/TeamJob.Services.Identity.API` directory) or by running `./scripts/start.sh` shell script in the root folder of repository.

By default, the service will be available under http://localhost:5000.

You can also start the service via Docker, either by building a local Dockerfile: 

`docker build -t teamjob.services.identity .` 

**What HTTP requests can be sent to the microservice API?**
----------------

You can find the list of all HTTP requests in [TeamJob.Services.Identity.rest] file placed in the root folder of the repository.
This file is compatible with [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) plugin for [Visual Studio Code](https://code.visualstudio.com). 

**What is the project structure of this microservice?**
----------------

The solution of this microservice is divided into 4 different projects: 

 * `TeamJob.Services.API`: This layer is where all the various API routes are declared and mapped to their respective Command or Query. Note that there is no controller as it uses endpoint matching (see `Program.cs`)
 * `TeamJob.Services.Application`: This is the business logic layer where all the various Commands or Queries get handled. This layer is also responsible for declaring all the interfaces it requires to function in an effort to reach maximum decoupling
 * `TeamJob.Services.Infrastructure`: This is the layer responsible for interfacing with the various dependencies and resources such as Database, MessageBroker and more. It is also here that the interfaces declared in `TeamJob.Services.Application`  and in `TeamJob.Services.Core` get defined
 * `TeamJob.Services.Core`: This is the domain layer. No logic should reside in here as the only purpose of this layer is to declare low-level classes that are to be used by the Infrastructure and Application layers. It is also here that the Interfaces used to communicate

 Note that the Dependencies of the projects go as follow : API -> Application/Infrastructure -> Core
