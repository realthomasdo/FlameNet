import React from 'react';
import Header from '../components/Header';
import Grid from '@mui/material/Grid';
import MapComponent from '../components/MapComponent';

function MainPage() {
  return (
    <div>
      <Header />
      <Grid container justifyContent="center" alignItems="center" spacing={2}>
        <Grid item xs={12}>
          {/* blank grid here */}
        </Grid>
        <Grid item xs={10}>
          <MapComponent />
        </Grid>
      </Grid>
    </div>
  );
}

export default MainPage;
