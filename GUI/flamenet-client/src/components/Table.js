import React, { useEffect, useMemo, useState } from 'react';
import { MaterialReactTable } from 'material-react-table';

const DataTable = () => {
  const [data, setData] = useState([]);

  const columns = useMemo(
    () => [
      {
        accessorKey: 'nodeId',
        header: 'Node ID',
        size: 100,
      },
      {
        accessorKey: 'timestamp',
        header: 'Timestamp',
        size: 150,
      },
      {
        accessorKey: 'commitTimestamp',
        header: 'Commit Timestamp',
        size: 100,
      },
      {
        accessorKey: 'isMasterNode',
        header: 'Master Node',
        size: 20,
      },
      {
        accessorKey: 'latitude',
        header: 'Latitude',
        size: 20,
      },
      {
        accessorKey: 'longitude',
        header: 'Longitude',
        size: 20,
      },
      {
        accessorKey: 'temperature',
        header: 'Temperature',
        size: 20,
      },
      {
        accessorKey: 'humidity',
        header: 'Humidity',
        size: 20,
      },
      {
        accessorKey: 'pressure',
        header: 'Pressure',
        size: 20,
      },
      {
        accessorKey: 'co2Level',
        header: 'CO2 Level',
        size: 20,
      },
      {
        accessorKey: 'ppm2_5',
        header: 'PPM2.5',
        size: 20,
      },
      {
        accessorKey: 'ppm10',
        header: 'PPM10',
        size: 20,
      },
      {
        accessorKey: 'fireDetected',
        header: 'Fire Detected',
        size: 20,
      },
      {
        accessorKey: 'windVelocity',
        header: 'Wind Velocity',
        size: 20,
      },
      {
        accessorKey: 'windDirection',
        header: 'Wind Direction',
        size: 20,
      },
    ],
    [],
  );

   const fetchData = () => {
    fetch('https://flamenet-server.onrender.com/api/getNodeLogs')
      .then((response) => response.json())
      .then((apiData) => {
        console.log(apiData);
        setData(apiData);
      })
      .catch((error) => {
        console.error('Data request error: ', error);
      });
  };
  useEffect(() => {
    // Initial data fetch
    fetchData();
  
    // Set up interval for periodic data fetch
    const pollInterval = setInterval(() => {
      fetchData();
    }, 3000);
  
    // Cleanup the interval on component unmount
    return () => clearInterval(pollInterval);
  }, []); // Empty dependency array to run the effect only once on mount
  

  return (
    <div style={{ height: '80vh', overflow: 'auto' }}>
      <MaterialReactTable
        columns={columns}
        data={data}
        initialSortBy={[
          { accessorKey: 'commitTimestamp', descending: true },
          { accessorKey: 'timestamp', descending: true },
          { accessorKey: 'nodeID', descending: false },
        ]}
      />
      </div>
  );
};

export default DataTable;
