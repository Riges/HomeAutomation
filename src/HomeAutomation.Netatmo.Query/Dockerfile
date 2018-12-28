FROM microsoft/dotnet:2.2-sdk-alpine AS dotnet-build
WORKDIR /src
COPY HomeAutomation.sln .
COPY HomeAutomation.Referential/HomeAutomation.Referential.csproj ./HomeAutomation.Referential/
COPY HomeAutomation.Event.Infrastructure/HomeAutomation.Event.Infrastructure.csproj ./HomeAutomation.Event.Infrastructure/
COPY HomeAutomation.Event.Model/HomeAutomation.Event.Model.csproj ./HomeAutomation.Event.Model/
COPY HomeAutomation.Event.Weather/HomeAutomation.Event.Weather.csproj ./HomeAutomation.Event.Weather/
COPY HomeAutomation.Netatmo.Query/HomeAutomation.Netatmo.Query.csproj ./HomeAutomation.Netatmo.Query/
RUN dotnet restore
COPY HomeAutomation.Referential/ /HomeAutomation.Referential/
COPY HomeAutomation.Event.Infrastructure/ /HomeAutomation.Event.Infrastructure/
COPY HomeAutomation.Event.Model/ /HomeAutomation.Event.Model/
COPY HomeAutomation.Event.Weather/ /HomeAutomation.Event.Weather/
COPY HomeAutomation.Netatmo.Query/ /HomeAutomation.Netatmo.Query/
RUN dotnet publish /HomeAutomation.Netatmo.Query/HomeAutomation.Netatmo.Query.csproj -c Release -o /out 

# build runtime image
FROM microsoft/dotnet:2.2-runtime-alpine
WORKDIR /app
COPY --from=dotnet-build /out ./
ENTRYPOINT ["dotnet", "HomeAutomation.Netatmo.Query.dll"]