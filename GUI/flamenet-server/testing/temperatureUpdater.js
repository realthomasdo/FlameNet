const cron = require('node-cron');
const mongoose = require('mongoose');
const Node = require('../models/node');

// Connect to MongoDB
mongoose.connect('mongodb+srv://flamenet-admin:H4uYO7SkVwb2e4wH@cluster0.2nnxixp.mongodb.net/flamenet-db', {
  useNewUrlParser: true,
  useUnifiedTopology: true,
});

// Function to update temperatures for all nodes
const updateTemperatures = async () => {
  try {
    const nodes = await Node.find(); // Fetch all nodes
    nodes.forEach(async (node) => {
      // Generate a random temperature value between 40 and 80
      const newTemperature = Math.random() * 40 + 40;

      // Update the temperature for the node in the database
      node.temperature = newTemperature;
      await node.save();
    });
    console.log('Temperatures updated successfully.');
  } catch (error) {
    console.error('Error updating temperatures:', error);
  }
};

// Schedule the update to run every 3 seconds
cron.schedule('*/3 * * * * *', updateTemperatures);

