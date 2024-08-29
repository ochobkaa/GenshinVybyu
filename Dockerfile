# Use the official .NET image as a base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Copy build artifacts from the build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["GenshinVybyu.csproj", "./"]
RUN dotnet restore "GenshinVybyu.csproj"
COPY . ./
RUN dotnet build "GenshinVybyu.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GenshinVybyu.csproj" -c Release -o /app/publish

# Copy the ModelData directory from the root project directory to the publish directory
COPY ModelData /app/publish/ModelData

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Add a debugging command to list the contents of /app/ModelData
RUN ls -l /app/ModelData

ENTRYPOINT ["dotnet", "GenshinVybyu.dll"]