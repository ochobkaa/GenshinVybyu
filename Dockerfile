FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "GenshinVybyu.csproj"
RUN dotnet publish "GenshinVybyu.csproj" -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./

EXPOSE 8443

ENTRYPOINT ["dotnet", "GenshinVybyu.dll"]