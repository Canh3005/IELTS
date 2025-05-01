# Use an official .NET 9.0 runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5004

# Use an official .NET 9.0 SDK as a build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["MyMvcBackend.csproj", "./"]
RUN dotnet restore "./MyMvcBackend.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "MyMvcBackend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyMvcBackend.csproj" -c Release -o /app/publish

# Final stage: runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyMvcBackend.dll"]