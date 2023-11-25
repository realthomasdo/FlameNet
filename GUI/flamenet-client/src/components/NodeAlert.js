import React, { useEffect, useState } from 'react';
import Alert from '@mui/material/Alert';
import AlertTitle from '@mui/material/AlertTitle';
import IconButton from '@mui/material/IconButton';
import CloseIcon from '@mui/icons-material/Close';

const NodeAlert = () => {
    const [nodes, setNodes] = useState([]);
    const [fireDetected, setFireDetected] = useState([]);
    const [highTemp, setHighTemp] = useState([]);
    const [highHumidity, setHighHumidity] = useState([]);
    const [highAvg, setHighAvg] = useState([]);
    const [showFireAlr, setShowFireAlr] = useState(true);
    const [showTempAlr, setShowTempAlr] = useState(true);
    const [showHumidAlr, setShowHumidAlr] = useState(true);
    const [showAvgAlr, setShowAvgAlr] = useState(true);

    useEffect(() => {
        const fetchData = async () => {
            setShowFireAlr(true);
            try {
                const response = await fetch('https://flamenet-server.onrender.com/api/getNodes');
                const data = await response.json();

                const onFire = data.filter((node) => node.fireDetected === true).map((node) => node.nodeId);
                if (onFire.length > 0) {
                    setFireDetected(new Set(onFire));
                }
            } catch (error) {
                console.error('Error fetching nodes, finding fireDetected:', error);
            }
        };

        fetchData();
        const pollInterval = setInterval(() => {
            fetchData();
        }, 7000);

        return () => clearInterval(pollInterval);
    }, []);

    useEffect(() => {
        const alertCalc = async () => {
            setShowAvgAlr(true);
            setShowHumidAlr(true);
            setShowTempAlr(true);
            try {
                const response = await fetch('http://localhost:3001/api/getNodeLogsSorted');
                const data = await response.json();
        
                const newHighHumiditySet = new Set();
                const newHighTempSet = new Set();
                const newHighAvgSet = new Set();
        
                data.forEach((node) => {
                    const nodeId = node[0];
                    const temperatureData = node.slice(1);
        
                    for (let i = 1; i < temperatureData.length; i++) {
                        const currentHumidity = temperatureData[i].humidity;
                        const previousHumidity = temperatureData[i - 1].humidity;
        
                        if (currentHumidity < previousHumidity) {
                            newHighHumiditySet.add(nodeId);
                        } else {
                            newHighHumiditySet.delete(nodeId);
                        }
        
                        const currentTemp = temperatureData[i].temperature;
                        const previousTemp = temperatureData[i - 1].temperature;
        
                        if (currentTemp > previousTemp) {
                            if (currentTemp >= 57) {
                                newHighTempSet.add(nodeId);
                            } else {
                                newHighTempSet.delete(nodeId);
                            }
                        }
                    }
        
                    // avg temperature calculation
                    const temperatures = temperatureData.map((entry) => entry.temperature);
                    const averageTemperature =
                        temperatures.reduce((sum, temp) => sum + temp, 0) / temperatures.length;
        
                    if (averageTemperature >= 57) {
                        newHighAvgSet.add(nodeId);
                    } else {
                        newHighAvgSet.delete(nodeId);
                    }
                });
        
                // Update states after the loop
                setHighHumidity((prevNodes) => new Set([...prevNodes, ...newHighHumiditySet]));
setHighTemp((prevNodes) => new Set([...prevNodes, ...newHighTempSet]));
setHighAvg((prevNodes) => new Set([...prevNodes, ...newHighAvgSet]));
        
            } catch (error) {
                console.error('Error fetching nodes, getting sorted NodeLogs', error);
            }
        };
    

        alertCalc();
        const pollInterval = setInterval(() => {
            alertCalc();
        }, 7000);

        return () => clearInterval(pollInterval);
    }, []);

    return (
        <div>
            {fireDetected.size > 0 && showFireAlr && (
                <Alert
                    severity="error"
                    variant="filled"
                    action={
                        <IconButton
                            aria-label="close"
                            color="inherit"
                            size="small"
                            onClick={() => setShowFireAlr(false)}
                        >
                            <CloseIcon fontSize="inherit" />
                        </IconButton>
                    }
                >
                    <AlertTitle>Fire!</AlertTitle>
                    <div>Alert: Fire detected in the following nodes: </div>
                    <ul>
                        {[...fireDetected].map((node) => (
                            <li key={node}>{node}</li>
                        ))}
                    </ul>
                </Alert>
            )}
    
            {highTemp.size > 0 && showTempAlr && (
                <Alert
                    severity="error"
                    variant="filled"
                    action={
                        <IconButton
                            aria-label="close"
                            color="inherit"
                            size="small"
                            onClick={() => setShowTempAlr(false)}
                        >
                            <CloseIcon fontSize="inherit" />
                        </IconButton>
                    }
                >
                    <AlertTitle>High Temperature!</AlertTitle>
                    <div>Warning: Rising temperatures above 57 degrees detected in the following nodes:</div>
                    <ul>
                        {[...highTemp].map((node) => (
                            <li key={node}>{node}</li>
                        ))}
                    </ul>
                </Alert>
            )}
    
            {highHumidity.size > 0 && showHumidAlr && (
                <Alert
                    severity="info"
                    variant="filled"
                    action={
                        <IconButton
                            aria-label="close"
                            color="inherit"
                            size="small"
                            onClick={() => setShowHumidAlr(false)}
                        >
                            <CloseIcon fontSize="inherit" />
                        </IconButton>
                    }
                >
                    <div>Humidity dropping in the following nodes:</div>
                    <ul>
                        {[...highHumidity].map((node) => (
                            <li key={node}>{node}</li>
                        ))}
                    </ul>
                </Alert>
            )}
    
            {highAvg.size > 0 && showAvgAlr && (
                <Alert
                    severity="warning"
                    variant="filled"
                    action={
                        <IconButton
                            aria-label="close"
                            color="inherit"
                            size="small"
                            onClick={() => setShowAvgAlr(false)}
                        >
                            <CloseIcon fontSize="inherit" />
                        </IconButton>
                    }
                >
                    <AlertTitle>High average temperature</AlertTitle>
                    <div>Environment around the following nodes experiencing high average temperatures:</div>
                    <ul>
                        {[...highAvg].map((node) => (
                            <li key={node}>{node}</li>
                        ))}
                    </ul>
                </Alert>
            )}
        </div>
    );
};

export default NodeAlert;