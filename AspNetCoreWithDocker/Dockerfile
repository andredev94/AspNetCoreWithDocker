FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY AspNetCoreWithDocker/AspNetCoreWithDocker.csproj AspNetCoreWithDocker/
RUN dotnet restore AspNetCoreWithDocker/AspNetCoreWithDocker.csproj
COPY . .
WORKDIR /src/AspNetCoreWithDocker
RUN dotnet build AspNetCoreWithDocker.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish AspNetCoreWithDocker.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AspNetCoreWithDocker.dll"]
