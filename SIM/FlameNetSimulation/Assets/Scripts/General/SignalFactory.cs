using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalFactory : MonoBehaviour
{
    private static float maxDistance = 20;
    public static SignalController CreateSignal(Vector3 position, Packet packet)
    {
        GameObject signalObject = Instantiate(GameAssets.i.SignalPrefab, position, Quaternion.identity);
        SignalController signal = signalObject.GetComponent<SignalController>();
        signal.SetValues(packet, maxDistance);
        return signal;
    }
    public static SignalController CreateSignal(Vector3 position, SignalController signal)
    {
        GameObject signalObject = Instantiate(GameAssets.i.SignalPrefab, position, Quaternion.identity);
        SignalController signalPropogated = signalObject.GetComponent<SignalController>();
        Color color = signal.GetColor();
        color.b += 0.15f;
        Packet packet = signal.packet;
        signalPropogated.SetValues(packet, maxDistance, color);
        return signal;
    }
    public static DirectSignalController CreateDirectSignal(BeaconController start, BeaconController end, Packet packet)
    {
        GameObject signalObject = Instantiate(GameAssets.i.DirectSignalPrefab, start.transform.position, Quaternion.identity);
        DirectSignalController signal = signalObject.GetComponent<DirectSignalController>();
        signal.SetValues(start, end, packet);
        return signal;
    }
}