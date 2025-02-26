using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomSpawnStrategy : ISpawnStrategy
{
    List<Transform> unusedSpawnPoints;
    Transform[] spawnPoints;

    public  RandomSpawnStrategy(Transform[] spawnPoints)
    {
        this.spawnPoints = spawnPoints;
        unusedSpawnPoints = new List<Transform>(spawnPoints);
    }

    public Transform NextSpawnPoint()
    {
        if (!unusedSpawnPoints.Any())
        {
            unusedSpawnPoints = new List<Transform>(spawnPoints);
        }

        int _randomIndex = Random.Range(0, unusedSpawnPoints.Count);
        Transform _result = unusedSpawnPoints[_randomIndex];
        unusedSpawnPoints.RemoveAt(_randomIndex);
        return _result;
    }
}
