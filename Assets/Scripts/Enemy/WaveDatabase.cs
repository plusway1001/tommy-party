using System.Collections.Generic;
using UnityEngine;

public class WaveDatabase : MonoBehaviour
{
    public static WaveDatabase Instance;

    public Dictionary<int, WaveData> waveDatabase = new ();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        LoadCSV();
    }

    void LoadCSV()
    {
        TextAsset waveCSV = Resources.Load<TextAsset>("WaveList");

        if (waveCSV == null)
        {
            return;
        }

        string[] waveRows = waveCSV.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None);

        for (int i = 1; i < waveRows.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(waveRows[i])) continue;

            string[] columns = waveRows[i].Split(',');

            int waveID = int.Parse(columns[0]);
            int enemyID = int.Parse(columns[1]);
            int spawnCount = int.Parse(columns[2]);
            float spawnInterval = float.Parse(columns[3]);

            if (!waveDatabase.ContainsKey(waveID))
            {
                waveDatabase[waveID] = new WaveData
                {
                    waveID = waveID
                };
            }

            waveDatabase[waveID].spawns.Add(
                new WaveSpawnData
                {
                    enemyID = enemyID,
                    spawnCount = spawnCount,
                    spawnInterval = spawnInterval
                });
        }
    }

    public WaveData GetWave(int waveID)
    {
        if (waveDatabase.TryGetValue(waveID, out WaveData wave))
        {
            return wave;
        }
        else
        {
            return null;
        }
    }

    public List<WaveData> GetAllWaves()
    {
        return new List<WaveData>(waveDatabase.Values);
    }
}
