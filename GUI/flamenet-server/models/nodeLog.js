const mongoose = require('mongoose');

const nodeLogSchema = new mongoose.Schema({
  nodeId: {
    type: String,
    required: true,
  },
  timestamp: {
    type: Date,
  },
  commitTimestamp: {
    type: Date,
    default: Date.now, // Default value is the current timestamp when the log is created
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
}, { collection: 'nodeLogs' }); // You can change the collection name if needed

// Create a NodeLog model based on the schema
const NodeLog = mongoose.model('NodeLog', nodeLogSchema);

module.exports = NodeLog;
