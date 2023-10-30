using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalFactory : MonoBehaviour
{
    public static SignalController CreateSignal(Vector3 position, float maxDistance, Packet packet)
    {
        GameObject signalObject = Instantiate(GameAssets.i.SignalPrefab, position, Quaternion.identity);
        SignalController signal = signalObject.GetComponent<SignalController>();
        signal.SetValues(packet, maxDistance);
        return signal;
    }
    public static SignalController CreateSignal(Vector3 position, float maxDistance, SignalController signal)
    {
        GameObject signalObject = Instantiate(GameAssets.i.SignalPrefab, position, Quaternion.identity);
        SignalController signalPropogated = signalObject.GetComponent<SignalController>();
        Color color = signal.GetColor();
        color.b += 0.15f;
        Packet packet = signal.packet;
        packet.isPropogated = true;
        signalPropogated.SetValues(packet, maxDistance, color);
        return signal;
    }
}