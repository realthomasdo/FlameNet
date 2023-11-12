import React, { useState, useEffect } from 'react';
import { Marker, InfoWindow } from '@react-google-maps/api';

import masterIcon from './house-solid.png'; 
import beaconIcon from './radio-solid.png'; 
import fireIcon from './fire.gif';

function NodeMarker({
  position,
  info,
  temperature,
  humidity,
  timestamp,
  co2Level,
  ppm,
  isMasterNode,
  fireDetected,
}) {
  const [isOpen, setIsOpen] = useState(false);

  const handleMarkerClick = () => {
    setIsOpen(!isOpen);
  };

  useEffect(() => {
    // Log any error related to image loading
    const handleImageError = (event) => {
      console.error('Error loading image:', event.target.src);
    };

    const imgElements = document.querySelectorAll('.gm-style-iw img');
    imgElements.forEach((img) => img.addEventListener('error', handleImageError));

    return () => {
      // Cleanup: Remove event listeners
      imgElements.forEach((img) => img.removeEventListener('error', handleImageError));
    };
  }, []);

  let markerIcon = null;

  if (fireDetected) {
    markerIcon = {
      url: fireIcon,
      scaledSize: new window.google.maps.Size(55, 55),
    };
  } else if (isMasterNode) {
    markerIcon = {
      url: masterIcon,
      scaledSize: new window.google.maps.Size(35, 35),
    };
  } else {
    markerIcon = {
      url: beaconIcon,
      scaledSize: new window.google.maps.Size(35, 35),
    };
  }

  return (
    <Marker position={position} icon={markerIcon} onClick={handleMarkerClick}>
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
            <p>Master Node: {isMasterNode ? 'true' : 'false'}</p>
            <p>Fire Detected: {fireDetected ? 'true' : 'false'}</p>
          </div>
        </InfoWindow>
      )}
    </Marker>
  );
}

export default NodeMarker;
