#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
ENV ASPNETCORE_ENVIRONMENT="Development"
WORKDIR /app
EXPOSE 80
EXPOSE 5000
EXPOSE 443


FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ProductsService/ProductsService.csproj", "ProductsService/"]
COPY ["ExceptionHandling/ExceptionHandling.csproj", "ExceptionHandling/"]
RUN dotnet restore "ProductsService/ProductsService.csproj"
COPY . .
WORKDIR "/src/ProductsService"
RUN dotnet build "ProductsService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductsService.csproj" -c Release -o /app/publish /p:UseAppHost=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ProductsService.dll"]