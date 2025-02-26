using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnStrategy
{
    Transform NextSpawnPoint();
}
