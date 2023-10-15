import React, { useState } from 'react';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import IconButton from '@mui/material/IconButton';
import MenuIcon from '@mui/icons-material/Menu';
import StarIcon from '@mui/icons-material/Star';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
import { Link } from 'react-router-dom';

export default function Header() {
  const [anchorEl, setAnchorEl] = useState(null);

  const handleMenuClick = (event) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  return (
    <Box sx={{ flexGrow: 1 }}>
      <AppBar position="static">
        <Toolbar sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <IconButton
            size="large"
            edge="start"
            color="inherit"
            aria-label="menu"
            aria-controls="basic-menu"
            aria-haspopup="true"
            aria-expanded={Boolean(anchorEl)}
            onClick={handleMenuClick}
          >
            <MenuIcon />
          </IconButton>

          <Menu
            id="basic-menu"
            anchorEl={anchorEl}
            open={Boolean(anchorEl)}
            onClose={handleMenuClose}
            anchorOrigin={{
              vertical: 'bottom',
              horizontal: 'right',
            }}
            transformOrigin={{
              vertical: 'top',
              horizontal: 'right',
            }}
            getContentAnchorEl={null}
          >
            <MenuItem onClick={handleMenuClose}>
              <Link to="/main" style={{ textDecoration: 'none', color: 'inherit' }}>
                Main Page
              </Link>
            </MenuItem>
            <MenuItem onClick={handleMenuClose}>
              <Link to="/history" style={{ textDecoration: 'none', color: 'inherit' }}>
                History Page
              </Link>
            </MenuItem>
          </Menu>

          <Typography variant="h6" component="div" sx={{ display: 'flex', alignItems: 'center', justifyContent: 'flex-start' }}>
            FlameNet
          </Typography>
          <IconButton
            size="large"
            edge="start"
            color="inherit"
            aria-label="star"
            sx={{ display: 'flex', justifyContent: 'flex-end', alignItems: 'center' }}
          >
            <StarIcon />
          </IconButton>
        </Toolbar>
      </AppBar>
    </Box>
  );
}
