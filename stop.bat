powershell "Stop-Process -processname dotnet"

docker stop trade-event
docker rm trade-event

docker stop eureka
docker rm eureka