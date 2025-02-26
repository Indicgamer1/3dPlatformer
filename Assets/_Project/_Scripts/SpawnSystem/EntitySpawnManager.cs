using System;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;

public abstract class EntitySpawnManager : MonoBehaviour
{
    
    [SerializeField] protected Button switchSpawnStrategyButton;
    [SerializeField] protected TextMeshProUGUI switchSpawnStrategyText;
    [SerializeField] protected SpawnPointStrategyType spawnPointStrategyType = SpawnPointStrategyType.Linear;
    [SerializeField] protected Transform[] spawnPoints;

    protected SpawnPointStrategyType currentSpawnPointStrategyType; 
    protected ISpawnStrategy spawnStrategy;
    protected LinearSpawnStrategy linearSpawnStrategy;
    protected RandomSpawnStrategy randomSpawnStrategy;
    protected enum SpawnPointStrategyType
    {
        Linear,
        Random
    }

    protected virtual void Awake()
    {
        linearSpawnStrategy = new LinearSpawnStrategy(spawnPoints);
        randomSpawnStrategy = new RandomSpawnStrategy(spawnPoints);
        SetStrategyType();
        
        switchSpawnStrategyButton.onClick.AddListener(SwitchStrategy);
    }

    protected virtual void SwitchStrategy()
    {
        if (spawnPointStrategyType == SpawnPointStrategyType.Linear)
        {
            spawnPointStrategyType = SpawnPointStrategyType.Random;
            switchSpawnStrategyText.text = "Random";
        }
        else
        {
            spawnPointStrategyType = SpawnPointStrategyType.Linear;
            switchSpawnStrategyText.text = "Linear";
        }
        SetStrategyType();
    }

    private void SetStrategyType()
    {
        currentSpawnPointStrategyType = spawnPointStrategyType;
        if(currentSpawnPointStrategyType == SpawnPointStrategyType.Linear)
            spawnStrategy = linearSpawnStrategy;
        else 
            spawnStrategy = randomSpawnStrategy;
    }

    public abstract void Spawn();
}
