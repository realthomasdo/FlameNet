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
  pressure: {
    type: Number,
  },
  co2Level: {
    type: Number,
  },
  ppm1: {
    type: Number,
  },
  ppm2_5: {
    type: Number,
  },
  ppm10: {
    type: Number,
  },
  fireDetected: {
    type: Boolean,
    default: false,
  },
  windVelocity: {
    type: Number,
  },
  windDirection: {
    type: Number,
  },
}, { collection: 'nodes' });

// Create a Node model based on the schema
const Node = mongoose.model('Node', nodeSchema);

module.exports = Node;
