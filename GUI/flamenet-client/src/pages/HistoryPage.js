import React from 'react';
import Header from '../components/Header';
import Table from '../components/Table';
import Grid from '@mui/material/Grid';

function HistoryPage() {
  return (
    <div>
      <Header />
      <Grid container justifyContent="center" alignItems="center" spacing={12}>
        <Grid item xs={12}>
            {/* blank grid here */}
        </Grid>
        <Grid item xs={10}>
          {/* Add any margins or other components here */}
          <Table />
          {/* Add other components here */}
        </Grid>
      </Grid>
    </div>
  );
}

export default HistoryPage;
