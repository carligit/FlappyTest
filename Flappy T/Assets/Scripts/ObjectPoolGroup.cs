using UnityEngine;
using System.Linq;
using System.Collections;

/// <summary>
/// Represents a collection of pools. Getting items can be done through defined
/// randomized distribution tables called spreads
/// </summary>
public class ObjectPoolGroup : BaseObjectPool
{
    public ObjectPooler[] pools = new ObjectPooler[0];
    public Spreads[] spreads = new Spreads[0];
    public int size;

    public int currentSpread;

    public override int Count { get { return size; } }

    private static string invalidSpreadIndexErrMsg = "Attempting to get item from pool group {0} but spread index is not valid: {1}. Should be from 0 to {2}";

    void Awake()
    {
        for (int i = 0; i < spreads.Length; i++)
        {
            spreads[i].totalSum = spreads[i].distribution.Sum(d => d);
        }
    }

    /// <summary>
    /// Gets a random item from the pools. the randomization depends
    /// on if there are declared distribution spreads.
    /// </summary>
    public override BaseObjectPoolItem GetItem()
    {
        if (spreads.Length == 0)
            return pools[Random.Range(0, pools.Length)].GetItem();
        else
        {
            return GetItemFromSpread(Mathf.Clamp(currentSpread, 0, spreads.Length));
        }
    }

    public BaseObjectPoolItem GetIten(int poolIndex)
    {
        return pools[poolIndex].GetItem();
    }

    public BaseObjectPoolItem GetItemFromSpread(int spreadIndex)
    {
        if (spreadIndex >= spreads.Length || spreadIndex < 0)
        {
            var errorMsg = string.Format("Attempting to get item from pool group {0} " + 
                "but spread index is not valid: {1}. Should be from 0 to {2}", 
                this.gameObject.name, spreadIndex, spreads.Length);
            Debug.LogError(errorMsg);
            return null;
        }
        // Random.Range with int is min incluseive and max exclusive so adding 1 to max
        int random = Random.Range(0, spreads[spreadIndex].totalSum + 1);
        int currentTotal = 0;
        BaseObjectPoolItem returnItem = null;

        for (int i = 0; i < spreads[spreadIndex].distribution.Length; i++)
        {
            if (spreads[spreadIndex].distribution[i] <= 0)
                continue;

            currentTotal += spreads[spreadIndex].distribution[i];

            if (random <= currentTotal)
            {
                returnItem = pools[i].GetItem();
                break;
            }
        }

        return returnItem;
    }

    /// <summary>
    /// Represents a randomized distribution based on the number of pools.
    /// </summary>
    [System.Serializable]
    public struct Spreads
    {
        public int[] distribution;
        public int totalSum;
    }
}
