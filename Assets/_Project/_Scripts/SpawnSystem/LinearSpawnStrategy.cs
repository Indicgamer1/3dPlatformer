using UnityEngine;

public class LinearSpawnStrategy : ISpawnStrategy
{
    int index = 0;
    Transform[] spawnPoints;

    public LinearSpawnStrategy(Transform[] spawnPoints)
    {
        this.spawnPoints = spawnPoints;
    }

    public Transform NextSpawnPoint()
    {
        Transform _result = spawnPoints[index];
        index = (index + 1) % spawnPoints.Length;
        return _result;
    }
}
