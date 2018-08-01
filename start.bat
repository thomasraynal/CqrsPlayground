docker stop trade-event
docker rm trade-event

docker kill generator

docker kill trade
docker kill compliance
docker kill booking

docker rm generator
docker rm location
docker rm compliance
docker rm booking


docker network rm cqrs
docker network create cqrs

start /d "." docker run -it --rm --network=cqrs --name trade-event rabbitmq:3

cd cqrsplayground.trade.service
dotnet publish -c Release -o publish
cd..

docker build -t cqrsplayground.trade.service ./cqrsplayground.trade.service
start /d "." docker run -p 5000:80 --expose=5000 -it --rm --network=cqrs --name trades cqrsplayground.trade.service

cd cqrsplayground.compliance.service
dotnet publish -c Release -o publish
cd..

docker build -t cqrsplayground.compliance.service ./cqrsplayground.compliance.service
start /d "." docker run -p 5001:80 --expose=5001 -it --rm --network=cqrs --name compliance cqrsplayground.compliance.service

cd cqrsplayground.booking.service
dotnet publish -c Release -o publish
cd..

docker build -t cqrsplayground.booking.service ./cqrsplayground.booking.service
start /d "." docker run -p 5002:80 --expose=5002 -it --rm --network=cqrs --name booking cqrsplayground.booking.service

cd cqrsplayground.generator
dotnet publish -c Release -o publish
cd..

docker build -t cqrsplayground.generator ./cqrsplayground.generator
start /d "." docker run -p 5002:80 --expose=5002 -it --rm --network=cqrs --name generator cqrsplayground.generator


