FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app
COPY PaperlessREST ./
RUN dotnet restore
RUN dotnet publish -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PaperlessREST.Api.dll"]
