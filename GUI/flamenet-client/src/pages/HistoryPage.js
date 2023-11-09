import React from 'react';
import Header from '../components/Header';
import Table from '../components/Table';

function HistoryPage() {
  return (
    <div style={{ height: '100vh', overflow: 'hidden' }}>
      <Header />
      <Table style={{ width: '100%', height: '90vh' }} />
      {/* Add other components here */}
    </div>
  );
}

export default HistoryPage;
