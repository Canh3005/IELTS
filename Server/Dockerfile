# Base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5004

# Thêm dòng này để ASP.NET lắng nghe mọi IP (không chỉ localhost)
ENV ASPNETCORE_URLS=http://+:5004

# Build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy toàn bộ source code backend
COPY . .

# Khai báo tên project chính xác tại đây (đường dẫn tương đối từ /src)
RUN dotnet restore "./MyMvcBackend.csproj"
RUN dotnet build "./MyMvcBackend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./MyMvcBackend.csproj" -c Release -o /app/publish

# Final runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyMvcBackend.dll"]
