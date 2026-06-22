using System.Collections.Generic;

[System.Serializable]
public class WaveData
{
    public int waveID;
    public List<WaveSpawnData> spawns = new();
}

[System.Serializable]
public class WaveSpawnData
{
    public int enemyID;
    public int spawnCount;
    public float spawnInterval;
    public int maxAlive;
}
