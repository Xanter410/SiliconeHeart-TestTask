using UnityEngine;

public class Building : MonoBehaviour
{
    public BuildingData Data { get; private set; }

    public void Initialize(BuildingData data)
    {
        Data = data;
    }
}
