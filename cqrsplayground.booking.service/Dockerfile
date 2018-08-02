FROM microsoft/aspnetcore-build:2.0
COPY ./publish /app
WORKDIR /app
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet", "cqrsplayground.booking.service.dll"]