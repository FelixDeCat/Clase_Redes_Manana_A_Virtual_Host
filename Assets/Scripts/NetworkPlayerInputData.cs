using Fusion;
using UnityEngine;

public struct NetworkPlayerInputData : INetworkInput
{
    public Vector3 direction;
    public bool isFirePressed;

    public NetworkPlayerInputData(Vector3 _dir, bool _isFirePressed)
    {
        direction = _dir;
        isFirePressed = _isFirePressed;
    }

    public static NetworkPlayerInputData Default
    {
        get
        {
            return new NetworkPlayerInputData(Vector3.zero, false);
        }
    }
}
