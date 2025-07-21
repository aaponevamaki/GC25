using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class GameData
{
    public PlayerData playerData;
    public List<EnemyData> enemyDataList;
    public float time;
}

[Serializable]
public class PlayerData
{
    public Vector3 position;
    public int health;
}

[Serializable]
public class EnemyData
{
    public int id;
    public Vector3 position;
    public int health;
}

public static class SaveGame
{
    private static readonly string gameName = "CluckOffZombies";
    private static readonly string fileName = "GameData.json";

    private static string GetFilePath()
    {
        string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), gameName);
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
        return Path.Combine(folderPath, fileName);
    }

    public static void SaveGameData(GameObject playerObject, List<GameObject> enemyObjects, float time)
    {
        PlayerData player = new()
        {
            position = playerObject.transform.position,
            health = playerObject.GetComponent<HealthManager>().GetHealth()
        };

        List<EnemyData> enemies = new();
        foreach (GameObject enemy in enemyObjects)
        {
            enemies.Add(new()
            {
                id = enemy.GetComponent<Enemy>().id,
                position = enemy.transform.position,
                health = enemy.GetComponent<HealthManager>().GetHealth()
            });
        }

        GameData gameData = new() { playerData = player, enemyDataList = enemies, time = time };
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(GetFilePath(), json);
    }

    public static GameData LoadGameData()
    {
        if (!File.Exists(GetFilePath())) return null;

        string json = File.ReadAllText(GetFilePath());
        GameData gameData = JsonUtility.FromJson<GameData>(json);
        return gameData;
    }

    public static void ClearGameData()
    {
        if (File.Exists(GetFilePath())) File.Delete(GetFilePath());
    }
}