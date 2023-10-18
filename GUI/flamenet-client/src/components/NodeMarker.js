import React, { useState } from 'react';
import { Marker, InfoWindow } from '@react-google-maps/api';

function NodeMarker({ position, info }) {
  const [isOpen, setIsOpen] = useState(false);

  const handleMarkerClick = () => {
    setIsOpen(!isOpen);
  };

  return (
    <Marker position={position} onClick={handleMarkerClick}>
      {isOpen && (
        <InfoWindow onCloseClick={() => setIsOpen(false)}>
          <div>
            <p>Latitude: {position.lat}</p>
            <p>Longitude: {position.lng}</p>
            {info && <p>{info}</p>}
          </div>
        </InfoWindow>
      )}
    </Marker>
  );
}

export default NodeMarker;
