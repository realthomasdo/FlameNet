import React, { useState } from 'react';
import { Marker, InfoWindow } from '@react-google-maps/api';

import globeIcon from './globe.png';

function NodeMarker({ position, info, temperature, humidity, timestamp, co2Level, ppm }) {
  const [isOpen, setIsOpen] = useState(false);

  const handleMarkerClick = () => {
    setIsOpen(!isOpen);
  };

  // TODO: set to only master
  const masterMarkerIcon = {
    url: globeIcon, 
    scaledSize: new window.google.maps.Size(35, 35), // Adjust the size as needed
  };

  return (
    <Marker position={position} icon={masterMarkerIcon} onClick={handleMarkerClick}>
      {isOpen && (
        <InfoWindow onCloseClick={() => setIsOpen(false)}>
          <div>
            <p>Latitude: {position.lat}</p>
            <p>Longitude: {position.lng}</p>
            {info && <p>{info}</p>}
            {temperature && <p>Temperature: {temperature} °C</p>}
            {humidity && <p>Humidity: {humidity}%</p>}
            {timestamp && <p>Timestamp: {timestamp}</p>}
            {co2Level && <p>CO2 Level: {co2Level} ppm</p>}
            {ppm && <p>PPM: {ppm}</p>}
          </div>
        </InfoWindow>
      )}
    </Marker>
  );
}

export default NodeMarker;
