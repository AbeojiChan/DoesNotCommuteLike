using UnityEngine;

[System.Serializable]
public struct MoveSnapshot
{
    public Vector3 position;
    public Quaternion rotation;

    public MoveSnapshot(Vector3 pos, Quaternion rot)
    {
        position = pos;
        rotation = rot;
    }
}