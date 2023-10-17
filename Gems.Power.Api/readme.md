# Gems Power Calculation

## Purpose
Complete code challenge proposed by Engie to join Gems team

## Build the application
The application was built in .Net Core 7, so with the framework installed in your machine runs the commands below

dotnet restore

dotnet build

dotnet run

## Run tests

dotnet test

## How to use the API
After running the application, access 
https://localhost:8888/swagger/index.html

This will show you the swagger client to the api.

http://localhost:8888/productionplan
is the main endpoint, and you can use the swagger tool to help you calling it

If you want to use command line instead, try to run curl, with the following command:


```
curl -X 'POST' \
  'https://localhost:8888/ProductionPlan' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "load": 910,
  "fuels":
  {
    "gas(euro/MWh)": 13.4,
    "kerosine(euro/MWh)": 50.8,
    "co2(euro/ton)": 20,
    "wind(%)": 60
  },
  "powerplants": [
    {
      "name": "gasfiredbig1",
      "type": "gasfired",
      "efficiency": 0.53,
      "pmin": 100,
      "pmax": 460
    },
    {
      "name": "gasfiredbig2",
      "type": "gasfired",
      "efficiency": 0.53,
      "pmin": 100,
      "pmax": 460
    },
    {
      "name": "gasfiredsomewhatsmaller",
      "type": "gasfired",
      "efficiency": 0.37,
      "pmin": 40,
      "pmax": 210
    },
    {
      "name": "tj1",
      "type": "turbojet",
      "efficiency": 0.3,
      "pmin": 0,
      "pmax": 16
    },
    {
      "name": "windpark1",
      "type": "windturbine",
      "efficiency": 1,
      "pmin": 0,
      "pmax": 150
    },
    {
      "name": "windpark2",
      "type": "windturbine",
      "efficiency": 1,
      "pmin": 0,
      "pmax": 36
    }
  ]
}
'
```

[![.NET](https://github.com/aaronbock/powerplant-coding-challenge/actions/workflows/dotnet.yml/badge.svg)](https://github.com/aaronbock/powerplant-coding-challenge/actions/workflows/dotnet.yml)