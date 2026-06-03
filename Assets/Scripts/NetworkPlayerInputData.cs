using Fusion;
using UnityEngine;

public enum ButtonType
{
    isFirePressed
}
public struct NetworkPlayerInputData : INetworkInput
{
    public const byte IsFirePressed0 = 0;
    public const byte IsFirePressed1 = 1;
    public const byte IsJumping = 2;

    public Vector3 direction;
    public Vector3 dirToLook;

    public NetworkButtons buttons;
}
