# VDCore v 1.0.3
## DATABASE
#### Postgres: 
`docker pull postges`

`docker run -p 5432:5432 --name myTestPostgresDB -e POSTGRES_PASSWORD=yourPassword -d postgres`
#### MariaDB: 
`docker pull mariadb:10.1.48`

`docker run -p 127.0.0.1:3306:3306  --name mariadb -e MYSQL_ROOT_PASSWORD=root -d mariadb:10.1.48`

## DOCKER
#### Push new version and run
`docker build -t 360deg/vdcore .`

`docker push 360deg/vdcore`

`docker run --name VDCore -it -p 5228:80 360deg/vdcore`

