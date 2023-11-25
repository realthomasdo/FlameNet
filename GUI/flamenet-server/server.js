const cors = require('cors');
const express = require('express');
const mongoose = require('mongoose');
const Node = require('./models/node');
const NodeLog = require('./models/nodeLog');

const app = express();
const port = 3001;

// Enable CORS for all routes
app.use(cors());

// Connect to MongoDB
mongoose.connect('mongodb+srv://flamenet-admin:H4uYO7SkVwb2e4wH@cluster0.2nnxixp.mongodb.net/flamenet-db', {
  useNewUrlParser: true,
  useUnifiedTopology: true,
});

app.use(express.json());

// Create or update nodes
app.post('/api/createOrUpdateNodes', async (req, res) => {
  try {
    const nodes = Array.isArray(req.body) ? req.body : [req.body];

    // Validate that req.body is an array
    if (!Array.isArray(nodes)) {
      return res.status(400).json({ error: 'Invalid request body. Expected an array of nodes.' });
    }

    const createdOrUpdatedNodes = [];

    for (const node of nodes) {
      const { nodeId, ...nodeData } = node;

      // Check if a node with the given nodeId already exists
      const existingNode = await Node.findOne({ nodeId });

      if (existingNode) {
        // Update the existing node
        const updatedNode = await Node.findOneAndUpdate({ nodeId }, nodeData, { new: true });

        // Log the update in the NodeLog table
        await NodeLog.create({
          nodeId,
          ...nodeData,
          commitTimestamp: Date.now(),
        });

        createdOrUpdatedNodes.push(updatedNode);
      } else {
        // Create a new node
        const newNode = await Node.create({ nodeId, ...nodeData });

        // Log the creation in the NodeLog table
        await NodeLog.create({
          nodeId,
          ...nodeData,
          commitTimestamp: Date.now(),
        });

        createdOrUpdatedNodes.push(newNode);
      }
    }

    res.status(201).json(createdOrUpdatedNodes);
  } catch (error) {
    console.error(error);
    res.status(500).json({ error: 'Internal Server Error' });
  }
});

// Get all current nodes
app.get('/api/getNodes', async (req, res) => {
  try {
    const nodes = await Node.find();
    res.json(nodes);
  } catch (error) {
    console.error(error);
    res.status(500).json({ error: 'Internal Server Error' });
  }
});

// Get all node logs
app.get('/api/getNodeLogs', async (req, res) => {
  try {
    const nodeLogs = await NodeLog.find();
    res.json(nodeLogs);
  } catch (error) {
    console.error(error);
    res.status(500).json({ error: 'Internal Server Error' });
  }
});

//sort getNodeLogs by node, 
app.get('/api/getNodeLogsSorted', async (req, res) => {
  try {
    const data = await NodeLog.find().sort({ timestamp: -1 });

    // Sorting is already done in the database query, so you can remove the sort operation here

    
    const sortedArr = [];

    data.forEach((entry) => {
      const { nodeId, timestamp, temperature, humidity, ppm, ppm2_5 } = entry;
      let row = sortedArr.find((row) => row[0] === nodeId);

      if (!row) {
        row = [nodeId];
        sortedArr.push(row);
      }

      row.push({ timestamp, temperature, humidity, ppm, ppm2_5 });
    });

    res.json(sortedArr); // Send the sorted data as the response
  } catch (error) {
    console.error(error);
    res.status(500).json({ error: 'Internal Server Error - Alerts' });
  }
});


app.listen(port, () => {
  console.log(`Server is running on port ${port}`);
});
