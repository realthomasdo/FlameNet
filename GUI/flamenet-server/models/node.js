const mongoose = require('mongoose');

const nodeSchema = new mongoose.Schema({
  nodeId: {
    type: String,
    required: true,
  },
  timestamp: {
    type: Date,
  },
  isMasterNode: {
    type: Boolean,
    required: true,
    default: false,
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
  co2Level: {
    type: Number,
  },
  ppm: {
    type: Number,
  },
  fireDetected: {
    type: Boolean,
    default: false,
  },
}, { collection: 'nodes' });

// Create a Node model based on the schema
const Node = mongoose.model('Node', nodeSchema);

module.exports = Node;
