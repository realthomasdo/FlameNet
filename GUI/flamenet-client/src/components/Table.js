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
        accessorKey: 'temperature',
        header: 'Temperature',
        size: 1,
      },
      {
        accessorKey: 'humidity',
        header: 'Humidity',
        size: 10,
      },
      {
        accessorKey: 'longitude',
        header: 'Longitude',
        size: 10,
      },
      {
        accessorKey: 'latitude',
        header: 'Latitude',
        size: 10,
      },
      {
        accessorKey: 'co2Level',
        header: 'CO2 Level',
        size: 10,
      },
      {
        accessorKey: 'ppm',
        header: 'PPM',
        size: 10,
      },
      {
        accessorKey: 'fireDetected',
        header: 'Fire Detected',
        size: 10,
      },
      {
        accessorKey: 'isMasterNode',
        header: 'Master Node',
        size: 10,
      },
    ],
    [],
  );

  useEffect(() => {
    fetch('https://flamenet-server.onrender.com/api/getNodeLogs')
      .then((response) => response.json())
      .then((apiData) => {
        console.log(apiData);
        setData(apiData);
      })
      .catch((error) => {
        console.error('Data request error: ', error);
      });
  }, []);

  return (
    <MaterialReactTable
      columns={columns}
      data={data}
      initialSortBy={[
        { accessorKey: 'commitTimestamp', descending: true },
        { accessorKey: 'timestamp', descending: true },
        { accessorKey: 'nodeID', descending: false },
      ]}
    />
  );
};

export default DataTable;
