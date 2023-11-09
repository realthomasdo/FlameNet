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
  isMasterNode: {
    type: Boolean,
    default: false,
  },
}, { collection: 'nodeLogs' }); // You can change the collection name if needed

// Create a NodeLog model based on the schema
const NodeLog = mongoose.model('NodeLog', nodeLogSchema);

module.exports = NodeLog;
