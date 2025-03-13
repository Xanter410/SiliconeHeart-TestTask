using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData_", menuName = "Data/BuildingData")]
public class BuildingData : ScriptableObject
{
    public string ID;
    public GameObject Prefab;
    public Sprite BuildingGhostSprite;
    public Vector2Int Size;
    public Sprite Icon;
}
