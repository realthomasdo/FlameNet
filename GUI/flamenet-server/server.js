const cors = require('cors');
const express = require('express');
const mongoose = require('mongoose');
const Node = require('./models/node');

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

// Create a new node
app.post('/api/createNode', async (req, res) => {
  try {
    const newNode = new Node(req.body);
    await newNode.save();
    res.status(201).json(newNode);
  } catch (error) {
    console.error(error);
    res.status(500).json({ error: 'Internal Server Error' });
  }
});

// Update an existing node
app.put('/api/nodes/:id', async (req, res) => {
  const nodeId = req.params.id;
  const updatedData = req.body;

  try {
    const updatedNode = await Node.findOneAndUpdate({ nodeId }, updatedData, { new: true });

    if (updatedNode) {
      res.json(updatedNode);
    } else {
      res.status(404).json({ error: 'Node not found' });
    }
  } catch (error) {
    console.error(error);
    res.status(500).json({ error: 'Internal Server Error' });
  }
});

// Get all nodes
app.get('/api/getNodes', async (req, res) => {
  try {
    const nodes = await Node.find();
    res.json(nodes);
  } catch (error) {
    console.error(error);
    res.status(500).json({ error: 'Internal Server Error' });
  }
});

app.listen(port, () => {
  console.log(`Server is running on port ${port}`);
});
