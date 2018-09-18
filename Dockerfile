FROM mysql:8.0.12
WORKDIR /app
EXPOSE 3306
COPY ./AspNetCoreWithDocker/DataBase/DataSet/ /home/database
COPY ./AspNetCoreWithDocker/DataBase/Migrations/ /home/database
COPY ./AspNetCoreWithDocker/ci/init_database.sh /docker-entrypoint-initdb.d/init_database.sh

