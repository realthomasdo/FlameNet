import React, { useEffect, useMemo, useState } from 'react';
import { MaterialReactTable } from 'material-react-table';

//nested data is ok, see accessorKeys in ColumnDef below
//example data 
//airqual in PM2.5 or PM10, micrograms/m^3
const DataTable = () => {
  const[data, setData] = useState([]);
  //should be memoized or stable
  const columns = useMemo(
    () => [
      {
        accessorKey: 'id.dataID', //access nested data with dot notation
        header: 'Data ID',
        size: 100,
      },
      {
        accessorKey: 'id.nodeID',
        header: 'Node ID',
        size: 100,
      },
      {
        accessorKey: 'timestamp', //normal accessorKey
        header: 'Timestamp',
        size: 150,
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
        accessorKey: 'longitude',
        header: 'Longitude',
        size: 150,
      },
      {
        accessorKey: 'latitude',
        header: 'Latitude',
        size: 150,
      },
      {
        accessorKey: 'airqual',
        header: 'AirQuality',
        size: 20,
      },

    ],
    [],
  );

  useEffect(() => {
    fetch('https://87b96220-8f88-4790-833e-fc196aa3e959.mock.pstmn.io/fakeapidatabase/table')
    .then((response) => response.json())
    .then((apiData) =>{
      console.log(apiData);
      setData(apiData);
    })
    .catch((error) => {
      console.error('Data request error: ', error);
    });
  }, []);

  return <MaterialReactTable columns={columns} data={data} />;
};

export default DataTable;
