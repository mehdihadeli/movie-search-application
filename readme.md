# Movie Search Application

[![Actions Status](https://github.com/mehdihadeli/movie-search-app/workflows/build-dot-net/badge.svg?branch=main)](https://github.com/mehdihadeli/movie-search-app/actions)


## Application Structure

In this application I used a [mediator pattern](https://dotnetcoretutorials.com/2019/04/30/the-mediator-pattern-in-net-core-part-1-whats-a-mediator/) with using [MediatR](https://github.com/jbogard/MediatR) library in my controllers for a clean and [thin controller](https://codeopinion.com/thin-controllers-cqrs-mediatr/), also instead of using a `application service` class because after some times our controller will depends to different services and this breaks single responsibility principle. We use mediator pattern to manage the delivery of messages to handlers. One of the advantages behind the [mediator pattern](https://lostechies.com/jimmybogard/2014/09/09/tackling-cross-cutting-concerns-with-a-mediator-pipeline/) is that it allows the application code to define a pipeline of activities for requests . For example in our controllers we create a command and send it to mediator and mediator will route our command to a specific command handler in application layer. 

To support [Single Responsibility Principle](https://en.wikipedia.org/wiki/Single_responsibility_principle) and [Don't Repeat Yourself principles](https://en.wikipedia.org/wiki/Don%27t_repeat_yourself), the implementation of cross-cutting concerns is done using the mediatr [pipeline behaviors](https://github.com/jbogard/MediatR/wiki/Behaviors) or creating a [mediatr decorators](https://lostechies.com/jimmybogard/2014/09/09/tackling-cross-cutting-concerns-with-a-mediator-pipeline/).

Also in this project I used [clean architecture](https://www.youtube.com/watch?v=dK4Yb6-LxAk) and [vertical slice architecture](https://jimmybogard.com/vertical-slice-architecture/) or [Restructuring to a Vertical Slice Architecture](https://www.youtube.com/watch?v=cVVMbuKmNes) also I used [feature folder structure](http://www.kamilgrzybek.com/design/feature-folders/) in this project.

Also here I used cqrs for decompose my features to very small parts that make our application

- maximize performance, scalability and simplicity.
- adding new feature to this mechanism is very easy without any breaking change in other part of our codes. New features only add code, we're not changing shared code and worrying about side effects.
- easy to maintain and any changes only affect on one command or query and avoid any breaking changes on other parts
- it gives us better separation of concerns and cross cutting concern (with help of mediatr behavior pipelines) in our code instead of a big service class for doing a lot of things.  

I treat each request as a distinct use case or slice, encapsulating and grouping all concerns from front-end to back.
When adding or changing a feature in an application in n-tire architecture, we are typically touching many different "layers" in an application. we are changing the user interface, adding fields to models, modifying validation, and so on. Instead of coupling across a layer, we couple vertically along a slice. we `Minimize coupling` `between slices`, and `maximize coupling` `in a slice`.

With this approach, each of our vertical slices can decide for itself how to best fulfill the request. New features only add code, we're not changing shared code and worrying about side effects.

![](./assets/Vertical-Slice-Architecture.jpg)

With using CQRS pattern, we cut each business functionality into some vertical slices, and inner each of this slices we have [technical folders structure](http://www.kamilgrzybek.com/design/feature-folders/) specific to that feature (command, handlers, infrastructure, repository, controllers, ...). In Our CQRS pattern each command/query handler is a separate slice. This is where you can reduce coupling between layers. Each handler can be a separated code unit, even copy/pasted. Thanks to that, we can tune down the specific method to not follow general conventions (e.g. use custom SQL query or even different storage). In a traditional layered architecture, when we change the core generic mechanism in one layer, it can impact all methods. 

In this Project I covered most of important tests like `Unit Testing`, `Integration Testing` and `End To End` testing. For naming tests, I used [vladimir khorikov](https://enterprisecraftsmanship.com/posts/you-naming-tests-wrong/) naming convention in his article and it makes our tests more readable like a documentation for our developers.

In this app for increase performance we could use caching mechanism simply with implementing a interface `ICachePolicy<,>` and our object for caching, this will handle with a chancing pipeline on mediateR as cross cutting concern with name of `CachingBehavior`. For example for caching our `FindMovieByIdQuery` query we could use bellow code:

``` csharp
public class FindMovieByIdQuery : IQuery<FindMovieByIdQueryResult>
{
    public int Id { get; init; }

    public class CachePolicy : ICachePolicy<FindMovieByIdQuery, FindMovieByIdQueryResult>
    {
        public DateTimeOffset? AbsoluteExpirationRelativeToNow => DateTimeOffset.Now.AddMinutes(15);

        public string GetCacheKey(FindMovieByIdQuery query)
        {
            return CacheKey.With(query.GetType(), query.Id.ToString());
        }
    }
}
```
## Communication with External APIs

In this application for communicating with external apis like [TMDB Apis](https://developers.themoviedb.org/3) and [Youtube Apis](https://developers.google.com/youtube/v3) I used a [Anti Corruption Layer](https://deviq.com/domain-driven-design/anti-corruption-layer) as a [mediator](https://dev.to/asarnaout/the-anti-corruption-layer-pattern-pcd) between our system domain model and external systems domain model. The reason why you might use an anti corruption layer is to create a little padding between subsystems so that they do not `leak into each other` too much.

An Anti-Corruption Layer (ACL) is a set of patterns placed between the domain model and other bounded contexts or third party dependencies. The intent of this layer is to prevent the intrusion of foreign concepts and models into the domain model. The patterns are used to map between foreign domain models and APIs to types and interfaces that are part of the domain model.
This layer translates requests that one subsystem makes to the other subsystem. Use this pattern to ensure that an application's design is not limited by dependencies on outside subsystems.

![](./assets/diagram.png)

In our application I create an anti corruption class for `TMDB api` with name of [TMDBServiceClient](./src/MovieSearch.Infrastructure/Services/Clients/MovieDb/TMDBServiceClient.cs) and an anti corruption class for `youtube api` with name of [YoutubeVideoServiceClient](./src/MovieSearch.Infrastructure/Services/Clients/Video/YoutubeVideoServiceClient.cs).

In this anti corruption classes we also used some [resiliency mechanisms](https://medium.com/@emanuele.bucarelli/improve-resilience-in-the-net-application-80adda2c7710) like `Retry`, `Circuit-breaker`,`Timeout` and`Bulkhead` patterns for increasing our resiliency in calling third party apis and for doing this I used [Polly](https://github.com/App-vNext/Polly) library.


## Technologies - Libraries
- ✔️ **[`.NET Core 5`](https://dotnet.microsoft.com/download)** - .NET Framework and .NET Core, including ASP.NET and ASP.NET Core
- ✔️ **[`Newtonsoft.Json`](https://github.com/JamesNK/Newtonsoft.Json)** - Json.NET is a popular high-performance JSON framework for .NET
- ✔️ **[`MVC Versioning API`](https://github.com/microsoft/aspnet-api-versioning)** - Set of libraries which add service API versioning to ASP.NET Web API, OData with ASP.NET Web API, and ASP.NET Core
- ✔️ **[`FluentValidation`](https://github.com/FluentValidation/FluentValidation)** - Popular .NET validation library for building strongly-typed validation rules
- ✔️ **[`Swagger & Swagger UI`](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)** - Swagger tools for documenting API's built on ASP.NET Core
- ✔️ **[`Serilog`](https://github.com/serilog/serilog)** - Simple .NET logging with fully-structured events
- ✔️ **[`Polly`](https://github.com/App-vNext/Polly)** - Polly is a .NET resilience and transient-fault-handling library that allows developers to express policies such as Retry, Circuit Breaker, Timeout, Bulkhead Isolation, and Fallback in a fluent and thread-safe manner
- ✔️ **[`Scrutor`](https://github.com/khellang/Scrutor)** - Assembly scanning and decoration extensions for Microsoft.Extensions.DependencyInjection
- ✔️ **[`EasyCaching`](https://github.com/dotnetcore/EasyCaching)** - Open source caching library that contains basic usages and some advanced usages of caching which can help us to handle caching more easier.
- ✔️ **[`AutoMapper`](https://github.com/AutoMapper/AutoMapper)** - Convention-based object-object mapper in .NET.
- ✔️ **[`MediatR`](https://github.com/jbogard/MediatR)** - Simple, unambitious mediator implementation in .NET.
- ✔️ **[`Hellang.Middleware.ProblemDetails`](https://github.com/khellang/Middleware/tree/master/src/ProblemDetails)** - A middleware for handling exception in .Net Core
- ✔️ **[`GuardClauses`](https://github.com/ardalis/GuardClauses)** - A simple package with guard clause extensions.
- ✔️ **[`AspNetCore.HealthChecks.UI`](https://github.com/xabaril/AspNetCore.Diagnostics.HealthChecks)** - Enterprise HealthChecks for ASP.NET Core Diagnostics Package
- ✔️ **[`NSubstitute`](https://nsubstitute.github.io/)** - A friendly substitute for .NET mocking libraries.
- ✔️ **[`FluentAssertions`](https://github.com/fluentassertions/fluentassertions)** - A very extensive set of extension methods that allow you to more naturally specify the expected outcome of a TDD or BDD-style unit tests.


## Configs
For using this app we need a [YouTube ApiKey](https://developers.google.com/youtube/v3/getting-started) also for using TMDB api we need a [TMDB Api Key](https://www.themoviedb.org/settings/api) (I put a api key for TMDB in setting file for test purpose). you should set your api keys in [appsettings.json](./src/MovieSearch.Api/appsettings.json) file in bellow section:

``` json
  "TMDBOptions": {
    "BaseApiAddress": "https://api.themoviedb.org/3",
    "ApiKey": "122f0c50ae02fa84601c07025cb6d2f1", 
    "Language": "en-US",
    "Region": "US"
  },
  "YoutubeVideoOptions": {
    "ApiKey": "", //your youtube api key
    "SearchPart": "snippet",
    "SearchType": "video",
    "Order": 4
  },
```

Also for security purpose of our Apis I used [API key Authentication](https://codingsonata.com/secure-asp-net-core-web-api-using-api-key-authentication/) and you have to pas a API key in header of request or in query string of your api url with this key name `X-Api-Key`.
For example for url It will be like this:

``` bash
http://localhost:5000/api/v1/movies/150/with-trailers?trailersCount=10&X-Api-Key=C5BFF7F0-B4DF-475E-A331-F737424F013C
```

For setup valid api key there is a class with name [InMemoryGetApiKeyQuery](./src/MovieSearch.Infrastructure/Security/InMemoryGetApiKeyQuery.cs), that this class is a in-memory registry for all valid Api Key. this class implemented `IGetApiKeyQuery` class. you can implement this interface and store your keys in your favorite provider like EF Core Sql Server or a Json file, ...
Some of valid keys for test are:

``` json
C5BFF7F0-B4DF-475E-A331-F737424F013C
5908D47C-85D3-4024-8C2B-6EC9464398AD
06795D9D-A770-44B9-9B27-03C6ABDB1BAE
```
Also our swagger is fully compatible with this Api key and you can authenticate in swagger UI before making any request. Use one of the above keys in this text box.

![](./assets/API-Key-Auth.png)



