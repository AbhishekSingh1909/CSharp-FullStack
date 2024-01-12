FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# COPY . .

# Copy solution file
COPY *.sln .

# Copy project files for NuGet restore
COPY fStore.WEBAPI/fStore.WEBAPI.csproj fStore.WEBAPI/
COPY fStore.Core/fStore.Core.csproj fStore.Core/
COPY fStore.Business/fStore.Business.csproj fStore.Business/
COPY fStore.Controller/fStore.Controller.csproj fStore.Controller/
COPY fStore.Test/fStore.Test.csproj fStore.Test/

# Restore NuGet packages
RUN dotnet restore

# Copy the rest of the source code
COPY fStore.WEBAPI/ fStore.WEBAPI/
COPY fStore.Core/ fStore.Core/
COPY fStore.Business/ fStore.Business/
COPY fStore.Controller/ fStore.Controller/
COPY fStore.Test/ fStore.Test/

# Build and publish the API project
WORKDIR /app/fStore.WEBAPI
RUN dotnet publish -c Release -o /publish

# Final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "fStore.WEBAPI.dll"]  # Replace with the actual DLL name
