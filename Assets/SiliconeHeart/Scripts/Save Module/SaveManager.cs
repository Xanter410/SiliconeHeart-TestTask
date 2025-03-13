using UnityEngine;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public struct BuildingSaveData
{
    public string BuildingID;
    public Vector2 Position;

    public BuildingSaveData(string id, Vector2 position)
    {
        BuildingID = id;
        Position = position;
    }
}
public class SaveManager : MonoBehaviour
{
    private string _savePath;

    [System.Serializable]
    private class SaveDataWrapper
    {
        public List<BuildingSaveData> Buildings = new List<BuildingSaveData>();
    }

    private void Awake()
    {
        _savePath = Path.Combine(Application.persistentDataPath, "grid_save.json");
    }

    public void SaveBuildings(List<Building> buildings)
    {
        var wrapper = new SaveDataWrapper();

        foreach (var building in buildings)
        {
            wrapper.Buildings.Add(new BuildingSaveData(
                building.Data.ID,
                building.transform.position
            ));
        }

        string json = JsonUtility.ToJson(wrapper, true);

        try
        {
            File.WriteAllText(_savePath, json);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Save failed: {e.Message}");
        }
    }

    public List<BuildingSaveData> LoadData()
    {
        if (!File.Exists(_savePath))
        {
            return new List<BuildingSaveData>();
        }

        try
        {
            string json = File.ReadAllText(_savePath);
            SaveDataWrapper wrapper = JsonUtility.FromJson<SaveDataWrapper>(json);
            return wrapper.Buildings;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Load failed: {e.Message}");
            return new List<BuildingSaveData>();
        }
    }
}

