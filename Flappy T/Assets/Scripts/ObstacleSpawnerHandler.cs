using UnityEngine;
using System.Collections;

/// <summary>
/// Sets up obstacles as they are spawned from the pool auto spawner
/// </summary>
public class ObstacleSpawnerHandler : MonoBehaviour
{
    public ObjectPoolAutoSpawner spawner;
 
    void Start()
    {
        spawner.ObjectSpawned += OnObjectSpawned;
    }

    void OnObjectSpawned(BaseObjectPoolItem obstacle)
    {
        ((Obstacle)obstacle).Setup();
    }
}
