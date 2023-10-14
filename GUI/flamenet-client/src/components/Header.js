import React, { useState } from 'react';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import IconButton from '@mui/material/IconButton';
import MenuIcon from '@mui/icons-material/Menu';
import StarIcon from '@mui/icons-material/Star';
import Menu from '@material-ui/core/Menu'; 
import MenuItem from '@mui/material/MenuItem';

export default function Header() {
  // Initialize the "open" state and a function to handle menu opening/closing
  const [open, setOpen] = useState(null);

  // Function to handle menu button click
  const menuButtonClick = (event) => {
    setOpen(event.currentTarget); // Toggle the "open" state when the menu button is clicked
  }
  const menuClose = () => {
    setOpen(null); 
  }
  


  return (
    <Box sx={{ flexGrow: 1 }}>
      <AppBar position="static">
        <Toolbar sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <IconButton
            size="large"
            edge="start"
            color="inherit"
            aria-label="menu"
            sx={{display: 'flex', alignItems: 'center', justifyContent: 'flex-start' }}
            aria-controls={open ? 'basic-menu' : undefined}
            aria-haspopup="true"
            aria-expanded={open ? 'true' : undefined}
            onClick={menuButtonClick}
          >
            <MenuIcon />
          </IconButton>

          {/*popup appears in weird spot */}
          <Menu
            id="basic-menu"
            open={open}
            onClose={menuClose}
            anchorOrigin={{
              vertical: 'bottom',
              horizontal: 'left',
            }}
            transformOrigin={{
              vertical: 'top',
              horizontal: 'left',
            }}
          >
            
            <MenuItem onClick={menuClose}>Page 1</MenuItem>
            <MenuItem onClick={menuClose}>Page 2</MenuItem>
            <MenuItem onClick={menuClose}>Page 3</MenuItem>
          </Menu>


          <Typography variant="h6" component="div" sx={{display: 'flex', alignItems: 'center', justifyContent: 'flex-start' }}>
            FlameNet
          </Typography>
          <IconButton
            size="large"
            edge="start"
            color="inherit"
            aria-label="star"
            sx={{display:'flex', justifyContent: 'flex-end', alignItems:'center'}}
          >
            <StarIcon />
          </IconButton>
        </Toolbar>
      </AppBar>
    </Box>
  );
}
