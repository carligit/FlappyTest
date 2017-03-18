using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPoolAutoSpawner : MonoBehaviour
{
    [SerializeField] private SpawnPositionOptions posOptions;
    [SerializeField] private SpawnTimerOptions timerOptions;
    [SerializeField] private BaseObjectPool pool;
    [SerializeField] private int maxUsedItems;

    public int MaxUsedItems { get { return maxUsedItems; } set { maxUsedItems = Mathf.Min(value, pool.Count); } }

    public event System.Action<BaseObjectPoolItem> ObjectSpawned;
    
    private int currentAutoSpawnItem;
    private float timeSinceLastSpawn;
    private float nextSpawnTime;
    private Queue<BaseObjectPoolItem> spawnedItems;

    void Start()
    {
        MaxUsedItems = maxUsedItems;
        spawnedItems = new Queue<BaseObjectPoolItem>(pool.Count);
        nextSpawnTime = timerOptions.spawnRate + Random.Range(-timerOptions.timerOffset, timerOptions.timerOffset);
    }
    
    void Update()
    {
        if (GameManager.instance.GameOver || !GameManager.instance.GameStarted)
            return;

        timeSinceLastSpawn += Time.deltaTime;

        if (timerOptions.spawnRate != 0 && timeSinceLastSpawn >= nextSpawnTime)
        {
            timeSinceLastSpawn = 0;
            nextSpawnTime = timerOptions.spawnRate + Random.Range(-timerOptions.timerOffset, timerOptions.timerOffset);
            
            if (spawnedItems.Count >= MaxUsedItems && spawnedItems.Count > 0)
            {
                spawnedItems.Dequeue().Return();
            }

            if (spawnedItems.Count < pool.Count)
            {
                SpawnItem(pool.GetItem());
            }
        }
    }


    void SpawnItem(BaseObjectPoolItem item)
    {
        float xOffset = Random.Range(posOptions.xMinOffset, posOptions.xMaxOffset);
        float yOffset = Random.Range(posOptions.yMinOffset, posOptions.yMaxOffset);
        var spawnPos = pool.transform.position + new Vector3(xOffset, yOffset, 0);
        var rot = Quaternion.identity;
        item.transform.position = spawnPos;
        item.transform.rotation = Quaternion.identity;

        if (ObjectSpawned != null)
        {
            ObjectSpawned(item);
        }

        spawnedItems.Enqueue(item);
    }

    [System.Serializable]
    public class SpawnPositionOptions
    {
        public float xMinOffset;
        public float xMaxOffset;
        public float yMinOffset;
        public float yMaxOffset;
    }

    [System.Serializable]
    public class SpawnTimerOptions
    {
        public float spawnRate;
        public float timerOffset;
    }
}
