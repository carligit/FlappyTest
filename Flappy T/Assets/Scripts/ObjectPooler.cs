using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Represents a GameObject pool for BaseObjectPoolItems
/// </summary>
public class ObjectPooler : BaseObjectPool 
{
    [SerializeField] private BaseObjectPoolItem objectP;
    [SerializeField] private int size;
    [SerializeField] private Vector3 defaultSpawn = new Vector3(-100, 0, 0);
    [SerializeField] private bool detachFromParent;

    public bool HasAvailable { get { return pool.Count() > 0; } }
    public override int Count { get { return size; } }

    private Queue<BaseObjectPoolItem> pool;

    void Awake()
    {
        if (detachFromParent)
            this.gameObject.transform.parent = null;
    }

	void Start()
    {
        pool = new Queue<BaseObjectPoolItem>(size);

        // Initialize the pool
        for (int i = 0; i < size; i++)
        {
            var offset = defaultSpawn;
            var rot = Quaternion.identity;
            var parent = this.transform;
            var gO = (GameObject)Instantiate(objectP.gameObject, offset, rot, parent);
            var poolItem = gO.GetComponent<BaseObjectPoolItem>();
            poolItem.sourcePool = this;
            poolItem.gameObject.SetActive(false);
            pool.Enqueue(poolItem);
        }
    }

    /// <summary>
    /// Gets an unused item from the pool and sets the parent gameobject to active
    /// </summary>
    /// <returns>Either an unused object or null if the pool is empty</returns>
    public override BaseObjectPoolItem GetItem()
    {
        if (HasAvailable)
        {
            var item = pool.Dequeue();
            item.gameObject.SetActive(true);
            item.Reset();
            return item;
        }
        else
        {
            Debug.LogWarning("Trying to get item from empty pool " + gameObject.name);
            return null;
        }
    }

    /// <summary>
    /// Adds an object back to the pool.
    /// </summary>
    /// <param name="item">Item retreived from this pool</param>
    public void ReturnItem(BaseObjectPoolItem item)
    {
        if (pool.Count == size)
        {
            Debug.LogWarning("Added item to already maxed pool " + gameObject.name);
        }

        item.gameObject.SetActive(false);
        pool.Enqueue(item);
    }
}

public abstract class BaseObjectPoolItem : MonoBehaviour
{
    [HideInInspector]
    public ObjectPooler sourcePool;
    public abstract void Reset();

    /// <summary>
    /// Returns the item back to the source pool
    /// </summary>
    public void Return()
    {
        sourcePool.ReturnItem(this);
    }
}

public abstract class BaseObjectPool : MonoBehaviour
{
    public abstract int Count { get; }
    abstract public BaseObjectPoolItem GetItem();
}