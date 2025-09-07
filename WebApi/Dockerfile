FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

EXPOSE 80
COPY Application/*.csproj Application/
COPY Infrastructure/*.csproj Infrastructure/
COPY WebApi/*.csproj WebApi/

RUN dotnet restore WebApi/*.csproj

COPY . .
RUN dotnet publish WebApi/*.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app


COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "WebApi.dll"]
