const mongoose = require('mongoose');

const nodeSchema = new mongoose.Schema({
  nodeId: {
    type: String,
    required: true,
  },
  latitude: {
    type: Number,
    required: true,
  },
  longitude: {
    type: Number,
    required: true,
  },
  temperature: {
    type: Number,
  },
  humidity: {
    type: Number,
  },
  timestamp: {
    type: Date,
  },
  co2Level: {
    type: Number,
  },
  ppm: {
    type: Number,
  },
}, { collection: 'nodes' });

// Create a Node model based on the schema
const Node = mongoose.model('Node', nodeSchema);

module.exports = Node;
