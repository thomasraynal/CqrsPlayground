docker kill booking
docker rm booking

cd cqrsplayground.booking.service
dotnet publish -c Release -o publish
cd..

docker build -t cqrsplayground.booking.service ./cqrsplayground.booking.service
start /d "." docker run -p 5000:5000 -p 5002:5002 --expose=5002  --expose=5000 -it --rm --network=cqrs --name booking cqrsplayground.booking.service


