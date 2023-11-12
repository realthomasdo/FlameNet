import React, { useState, useEffect, useCallback } from 'react';
import { GoogleMap, useJsApiLoader } from '@react-google-maps/api';
import NodeMarker from './NodeMarker';

const containerStyle = {
  width: '100%',
  height: 'calc(100vh - 100px)' // Adjust the margin as needed
};

const center = {
  lat: 30.6375624,
  lng: -96.3232865
};

const MapComponent = () => {
  const { isLoaded } = useJsApiLoader({
    id: 'google-map-script',
    googleMapsApiKey: "AIzaSyDDJxEAi7kcIqXxKIFxHn19CbewQwMfuEg"
  });

  const [map, setMap] = useState(null);
  const [nodes, setNodes] = useState([]);
  const [mapCenter, setMapCenter] = useState(center);
  const [mapCenterManuallyChanged, setMapCenterManuallyChanged] = useState(false);

  useEffect(() => {
    const fetchNodes = async () => {
      try {
        const response = await fetch('http://localhost:3001/api/getNodes');
        const data = await response.json();
        setNodes(data);

        if (!mapCenterManuallyChanged) {
          const firstMasterNode = data.find(node => node.isMasterNode);
          if (firstMasterNode) {
            setMapCenter({ lat: firstMasterNode.latitude, lng: firstMasterNode.longitude });
          }
        }
      } catch (error) {
        console.error(error);
      }
    };

    const pollInterval = 1000;
    const pollData = () => {
      fetchNodes();
    };

    const pollIntervalId = setInterval(pollData, pollInterval);

    return () => clearInterval(pollIntervalId);
  }, [mapCenterManuallyChanged]);

  const onLoad = useCallback((map) => {
    setMap(map);
  }, []);

  const onUnmount = useCallback(() => {
    setMap(null);
  }, []);

  const handleMapDrag = () => {
    setMapCenterManuallyChanged(true);
  };

  return isLoaded ? (
    <GoogleMap
      mapContainerStyle={containerStyle}
      center={mapCenter}
      zoom={16}
      onLoad={onLoad}
      onUnmount={onUnmount}
      draggable={true}
      onDragEnd={handleMapDrag}
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
          isMasterNode={node.isMasterNode}
          fireDetected={node.fireDetected}
        />
      ))}
    </GoogleMap>
  ) : <></>;
};

export default React.memo(MapComponent);