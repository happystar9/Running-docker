FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /App
COPY . .
RUN dotnet tool install --global dotnet-ef

RUN dotnet publish TetrisWeb.csproj --configuration Release --output /App/publish
WORKDIR /App/publish

EXPOSE 5000:5000
ENTRYPOINT dotnet TetrisWeb.dll
