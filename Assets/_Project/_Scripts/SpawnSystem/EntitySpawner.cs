using UnityEngine;
using UnityEngine.UI;

public class EntitySpawner<T> where T : Entity
{
    IEntityFactory<T> entityFactory;
    ISpawnStrategy spawnStrategy;

    public EntitySpawner(IEntityFactory<T> entityFactory, ISpawnStrategy spawnStrategy)
    {
        this.entityFactory = entityFactory;
        this.spawnStrategy = spawnStrategy;
    }
    
    public T Spawn(out Transform spawnPoint)
    {
        spawnPoint = spawnStrategy.NextSpawnPoint();
        return entityFactory.Create(spawnPoint);
    }
}
