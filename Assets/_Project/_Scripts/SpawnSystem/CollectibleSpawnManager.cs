using System.Collections.Generic;
using Platformer;
using TMPro;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleSpawnManager : EntitySpawnManager
{
    [SerializeField] Button spawnButton;
    [SerializeField] CollectibleData[] collectibleData;
    [SerializeField] float spawnDelay = 1.0f;

    EntitySpawner<Collectible> spawner;
    List<Entity> spawnedEntities = new();
        
    CountdownTimer spawnTimer;
    int counter;

    protected override void Awake()
    {
        base.Awake();

        spawner = new EntitySpawner<Collectible>(new EntityFactory<Collectible>(collectibleData), spawnStrategy);

        spawnTimer = new CountdownTimer(spawnDelay);
        spawnTimer.OnTimerStopped += () =>
        {
            if( counter++ >= spawnPoints.Length)
            {
                spawnTimer.Stop();
                return;
            }

            Spawn();
            spawnTimer.Start();
        };
        
        spawnButton.onClick.AddListener(StartSpawning);
    }

    private void StartSpawning()
    {
        if (spawnedEntities.Count > 0)
        {
            foreach (var item in spawnedEntities)
            {
                if(item != null)
                    Destroy(item.gameObject);
            }

            foreach (var spawnPoint in spawnPoints)
            {
                var image = spawnPoint.GetComponentInChildren<Image>();
                if (image != null) 
                    image.enabled = true;
            }
            spawnedEntities.Clear();
        }
        counter = 0;
        spawnTimer.Start();
    }


    private void Update()
    {
        spawnTimer.Tick(Time.deltaTime);
    }

    protected override void SwitchStrategy()
    {
        base.SwitchStrategy();
        spawner = new EntitySpawner<Collectible>(new EntityFactory<Collectible>(collectibleData), spawnStrategy);
    }

    public override void Spawn()
    {
        Entity spawnedEntity = spawner.Spawn(out Transform spawnPoint);
        var image = spawnPoint.GetComponentInChildren<Image>();
        if (image != null) 
            image.enabled = false;
        spawnedEntities.Add(spawnedEntity);
    }
}
