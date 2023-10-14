import React, { useMemo } from 'react';
import { MaterialReactTable } from 'material-react-table';

//nested data is ok, see accessorKeys in ColumnDef below
//example data 
//airqual in PM2.5 or PM10, micrograms/m^3
const data = [
  {
    id: {
      dataID: '1',
      nodeID: '1',
    },
    timestamp: '6:00',
    temperature: '80',
    humidity: '40',
    latitude: '30.591733619',
    longitude: '-96.330444763',
    airqual: '99',
  },
  {
    id: {
      dataID: '2',
      nodeID: '2',
    },
    timestamp: '6:10',
    temperature: '77',
    humidity: '38',
    latitude: '30.596417',
    longitude: '-96.335634',
    airqual: '80',
  },{
    id: {
      dataID: '2',
      nodeID: '2',
    },
    timestamp: '6:20',
    temperature: '69',
    humidity: '60',
    latitude: '30.356655',
    longitude: '-96.45454',
    airqual: '110',
  },{
    id: {
      dataID: '3',
      nodeID: '3',
    },
    timestamp: '6:30',
    temperature: '90',
    humidity: '55',
    latitude: '30.5454524',
    longitude: '-96.42333',
    airqual: '120',
  },{
    id: {
      dataID: '4',
      nodeID: '4',
    },
    timestamp: '6:40',
    temperature: '104',
    humidity: '50',
    latitude: '30.59456456',
    longitude: '-96.453345',
    airqual: '150',
  },
];

const DataTable = () => {
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

  return <MaterialReactTable columns={columns} data={data} />;
};

export default DataTable;
