using UnityEngine;
public class GameAssets : MonoBehaviour
{
    public GameObject SignalPrefab;
    public GameObject DirectSignalPrefab;

    private static GameAssets _i;

    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return _i;
        }
    }
}