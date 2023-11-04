# server.js

Defines all api endpoints that the map uses to get data.
GET/UPDATE jsons are formatted as such:

{
  "nodeId": "node001",
  "latitude": 30.61696,
  "longitude": -96.3435232,
  "temperature": 25.5,
  "humidity": 50,
  "timestamp": "2023-11-04T14:30:00Z",
  "co2Level": 400,
  "ppm": 1000
}

# models/node.js

Defines the Node Mongoose Model.

# testing/updateTemperature.js

A dummy script that updates the temperature every 3 seconds, emulating
a real-time data collection. Use "node updateTemperature.js" in the CL to run.