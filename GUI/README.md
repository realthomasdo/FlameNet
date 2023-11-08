# server.js

Defines all api endpoints that the map uses to get data.
GET/UPDATE jsons are formatted as such:

TODO(Thomas): add fire columns

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

# models/node.js (models are how "tables" are structured)

Defines the Node Model
TODO(Thomas): add NodeLog Model

# testing/updateTemperature.js

A dummy script that updates the temperature every 3 seconds, emulating
a real-time data collection. Use "node updateTemperature.js" in the CL to run.

# How to Send HTTP Requests
server.js contains all the endpoints.
For example, one endpoint is '/api/getNodes' which means you can use
'curl https://flamenet-server.onrender.com/api/getNodes' from the CL to 
return all the nodes in the database. Additionally you can also do this through Postman.

# ChatGPT's tutorial on how to send json with curl
You can use `curl` to send a JSON payload to an endpoint by using the `-d` option to specify the data you want to send. Here's how to do it:

Suppose you want to send the following JSON data to an endpoint:

```json
{
  "name": "John Doe",
  "email": "johndoe@example.com"
}
```

You can use `curl` as follows:

```bash
curl -X POST -H "Content-Type: application/json" -d '{
  "name": "John Doe",
  "email": "johndoe@example.com"
}' http://example.com/api/endpoint
```

Here's a breakdown of the command:

- `-X POST`: Specifies that you want to send a POST request to the endpoint. You can change it to `PUT`, `GET`, or other HTTP methods as needed.
- `-H "Content-Type: application/json"`: Sets the `Content-Type` header to indicate that you're sending JSON data.
- `-d '...'`: The single quotes contain the JSON data you want to send. Make sure to properly escape any single quotes inside the JSON if necessary.
- `http://example.com/api/endpoint`: Replace this URL with the actual URL of the endpoint you want to send the JSON to.

By running this `curl` command, you'll send the JSON data to the specified endpoint using a POST request with the correct `Content-Type` header.

