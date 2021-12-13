# https://devblogs.microsoft.com/nuget/microsoft-author-signing-certificate-update/
# https://github.com/NuGet/Home/issues/10491

# the first, heavier image to build your code

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS builder

# Setup working directory for the project	 
WORKDIR /app
RUN curl -o /usr/local/share/ca-certificates/verisign.crt -SsL https://crt.sh/?d=1039083 && update-ca-certificates
COPY ./BuildingBlocks/BulidingBlocks/BuildingBlocks.csproj ./BuildingBlocks/BulidingBlocks/ 
COPY ./MovieSearch.Core/MovieSearch.Core.csproj ./MovieSearch.Core/ 
COPY ./MovieSearch.Application/MovieSearch.Application.csproj ./MovieSearch.Application/
COPY ./MovieSearch.Infrastructure/MovieSearch.Infrastructure.csproj ./MovieSearch.Infrastructure/
COPY ./MovieSearch.Api/MovieSearch.Api.csproj ./MovieSearch.Api/
 
# Restore nuget packages	 
RUN dotnet restore ./MovieSearch.Api/MovieSearch.Api.csproj 


# Copy project files
COPY ./BuildingBlocks/BulidingBlocks ./BuildingBlocks/BulidingBlocks/ 
COPY ./MovieSearch.Core ./MovieSearch.Core/ 
COPY ./MovieSearch.Application ./MovieSearch.Application/
COPY ./MovieSearch.Infrastructure ./MovieSearch.Infrastructure/
COPY ./MovieSearch.Api ./MovieSearch.Api/

# Build project with Release configuration
# and no restore, as we did it already
RUN dotnet build -c Release --no-restore ./MovieSearch.Api/MovieSearch.Api.csproj

# Publish project to output folder	 
# and no build, as we did it already	
WORKDIR /app/MovieSearch.Api
RUN dotnet publish -c Release --no-build -o out


# second, final, lighter image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal

# Setup working directory for the project  
WORKDIR /app

# Copy published in previous stage binaries	 
  
# from the `builder` image
COPY --from=builder /app/MovieSearch.Api/out  .		

ENV ASPNETCORE_URLS http://*:5000
ENV ASPNETCORE_ENVIRONMENT docker  

# sets entry point command to automatically	 
# run application on `docker run`	 
ENTRYPOINT ["dotnet", "MovieSearch.Api.dll"]