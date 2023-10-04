using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalFactory : MonoBehaviour
{
    public static SignalController CreateSignal(Vector3 position, Packet packet, float maxDistance)
    {
        GameObject signalObject = Instantiate(GameAssets.i.SignalPrefab, position, Quaternion.identity);
        SignalController signal = signalObject.GetComponent<SignalController>();
        signal.SetValues(packet, maxDistance);
        return signal;
    }
}