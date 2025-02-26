using KBCore.Refs;
using System.Transactions;
using UnityEngine;

public interface IEntityFactory<T> where T : Entity
{
    T Create(Transform spawnPoint);
}

public class EntityFactory<T> : IEntityFactory<T> where T : Entity
{
    EntityData[] data;

    public EntityFactory(EntityData[] entityData)
    {
        this.data = entityData;
    }

    public T Create(Transform spawnPoint)
    {
        EntityData entityData = data[Random.Range(0, data.Length)];
        GameObject instance = GameObject.Instantiate(entityData.prefab, spawnPoint.position, spawnPoint.rotation);
        return instance.GetComponent<T>();
    }
}
