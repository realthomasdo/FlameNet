import React from 'react';
import { GoogleMap, useJsApiLoader } from '@react-google-maps/api';
import NodeMarker from './NodeMarker';
import { useEffect } from 'react';
import { useState } from 'react';

const containerStyle = {
  width: '100%',
  height: 'calc(100vh - 100px)' // Adjust the margin as needed
};

const center = {
  lat: 30.6375624,
  lng: -96.3232865
};

function MapComponent() {
  const { isLoaded } = useJsApiLoader({
    id: 'google-map-script',
    googleMapsApiKey: "AIzaSyDDJxEAi7kcIqXxKIFxHn19CbewQwMfuEg" // do not expose this
  });

  const [map, setMap] = React.useState(null);
  const [nodes, setNodes] = useState([]);

  useEffect(() => {
    const fetchNodes = async () => {
      try {
        const response = await fetch('http://localhost:3001/api/getNodes');
        const data = await response.json();
        setNodes(data);
      } catch (error) {
        console.error(error);
      }
    };

    // Implement long polling by repeatedly fetching data
    const pollInterval = 1000; // 1 second (adjust as needed)
    const pollData = () => {
      fetchNodes();
      setTimeout(pollData, pollInterval);
    };

    pollData();
  }, []);

  const onLoad = React.useCallback(function callback(map) {
    const bounds = new window.google.maps.LatLngBounds(center);
    map.fitBounds(bounds);
    setMap(map);
  }, []);

  const onUnmount = React.useCallback(function callback(map) {
    setMap(null);
  }, []);

  return isLoaded ? (
    <GoogleMap
      mapContainerStyle={containerStyle}
      center={center}
      zoom={8}
      onLoad={onLoad}
      onUnmount={onUnmount}
    >
      {nodes.map((node) => (
        <NodeMarker
          key={node.nodeId}
          position={{ lat: node.latitude, lng: node.longitude }}
          temperature={node.temperature}
          humidity={node.humidity}
          timestamp={node.timestamp}
          co2Level={node.co2Level}
          ppm={node.ppm}
        />
      ))}
    </GoogleMap>
  ) : <></>;
}

export default React.memo(MapComponent);
