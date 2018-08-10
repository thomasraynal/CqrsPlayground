powershell "Stop-Process -processname dotnet"

docker stop trade-event
docker rm trade-event

docker pull rabbitmq:3

start /d "." docker run -p 5672:5672 -it --rm --name trade-event rabbitmq:3

docker stop eureka
docker rm eureka

docker pull netflixoss/eureka:1.3.1

start /d "." docker run -it --rm  -p 8080:8080 --expose=8080 --name eureka netflixoss/eureka:1.3.1

timeout 5

cd cqrsplayground.compliance.service
start /d "." dotnet run --scheme=http --host=localhost --port=5002 --gateway=http://localhost:8080/eureka/v2
cd..

timeout 5

cd cqrsplayground.booking.service
start /d "." dotnet run --scheme=http --host=localhost --port=5001 --gateway=http://localhost:8080/eureka/v2
cd..

timeout 5

cd cqrsplayground.trade.service
start /d "." dotnet run --scheme=http --host=localhost --port=5000 --gateway=http://localhost:8080/eureka/v2
cd..

timeout 5

cd cqrsplayground.trade.repository
start /d "." dotnet run --scheme=http --host=localhost --port=5003 --gateway=http://localhost:8080/eureka/v2
cd..

timeout 5

cd cqrsplayground.inventory
start /d "." dotnet run --scheme=http --host=localhost --port=5004 --gateway=http://localhost:8080/eureka/v2
cd..

timeout 15

cd cqrsplayground.generator
start /d "." dotnet run 
cd..
