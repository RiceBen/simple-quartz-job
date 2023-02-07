FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["simple-quartz-job/simple-quartz-job.csproj", "simple-quartz-job/"]
RUN dotnet restore "simple-quartz-job/simple-quartz-job.csproj"
COPY . .
WORKDIR "/src/simple-quartz-job"
RUN dotnet build "simple-quartz-job.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "simple-quartz-job.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "simple-quartz-job.dll"]
