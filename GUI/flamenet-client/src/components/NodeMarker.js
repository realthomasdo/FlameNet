import React, { useState } from 'react';
import { Marker, InfoWindow } from '@react-google-maps/api';

function NodeMarker({ position, info, temperature, humidity, timestamp, co2Level, ppm }) {
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
