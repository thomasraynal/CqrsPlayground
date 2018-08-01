

cd cqrsplayground.trade.service
start /d "." dotnet run 
cd..

timeout 2

cd cqrsplayground.compliance.service
start /d "." dotnet run 
cd..

timeout 2

cd cqrsplayground.booking.service
start /d "." dotnet run 
cd..

timeout 5

cd cqrsplayground.generator
start /d "." dotnet run 
cd..