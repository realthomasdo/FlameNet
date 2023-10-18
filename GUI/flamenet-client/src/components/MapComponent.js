import React from 'react';
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

const zach = {
  lat: 30.6210864,
  lng: -96.3403882
}

const polo = {
  lat: 30.6229681,
  lng: -96.3383515
}

function MapComponent() {
  const { isLoaded } = useJsApiLoader({
    id: 'google-map-script',
    googleMapsApiKey: "AIzaSyDDJxEAi7kcIqXxKIFxHn19CbewQwMfuEg" // do not expose this
  });

  const [map, setMap] = React.useState(null);

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
      <NodeMarker position={zach} />
      <NodeMarker position={polo} />
    </GoogleMap>
  ) : <></>;
}

export default React.memo(MapComponent);
