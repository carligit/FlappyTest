using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(ScrollingObject))]
public class Obstacle : BaseObjectPoolItem
{
    public bool heightOffset;
    public bool Flippable;
    public float heightOffsetMax;
  
    public UnityEvent OnReset;
    
    private ScrollingObject scrollObject;

    // gets flipped for every flippable tower spawned
    private static bool currentFlip;

    void Awake()
    {
        scrollObject = GetComponent<ScrollingObject>();
    }

    public void Setup()
    {
        if (heightOffset)
            SetHeightOffset();

        if (Flippable)
        {
            if (currentFlip)
                Flip();

            currentFlip = !currentFlip;
        }
    }

    public override void Reset()
    {
        scrollObject.SetVelocity();
        OnReset.Invoke();
    }

    private void SetHeightOffset()
    {
        transform.position = new Vector3(transform.position.x, Random.Range(GameManager.instance.bottomSpawnHeight, GameManager.instance.bottomSpawnHeight + heightOffsetMax), 0);
    }

    private void Flip()
    {
        transform.rotation = Quaternion.Euler(new Vector3(180, 0, 0));
        transform.position = new Vector3(transform.position.x, Random.Range(GameManager.instance.topSpawnHeight, GameManager.instance.topSpawnHeight - heightOffsetMax), 0);
    }
}
