using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public Vector2 inputMovement;
    public NetworkBool canMine;
    public NetworkBool canRocket;
}