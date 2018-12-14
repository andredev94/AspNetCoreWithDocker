FROM mysql:8.0.12
WORKDIR /app
EXPOSE 3306
COPY ./DataBase/DataSet/ /home/database
COPY ./DataBase/Migrations/ /home/database
COPY ./ci/init_database.sh /docker-entrypoint-initdb.d/init_database.sh

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 50977
EXPOSE 44383

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



