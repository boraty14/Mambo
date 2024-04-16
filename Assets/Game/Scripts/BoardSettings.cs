using UnityEngine;

[CreateAssetMenu]
public class BoardSettings : ScriptableObject
{
    [Range(1, 10)] public int Height;
    [Range(1, 10)] public int Width;
}
