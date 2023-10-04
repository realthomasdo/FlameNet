// routes/userRoutes.js

const express = require('express');
const router = express.Router();
const User = require('../models/userModel');

// Route to get all users
router.get('/', async (req, res) => {
  try {
    const users = await User.find();
    res.json(users);
  } catch (err) {
    res.status(500).json({ message: err.message });
  }
});

// Other routes for creating, updating, and deleting users

module.exports = router;
