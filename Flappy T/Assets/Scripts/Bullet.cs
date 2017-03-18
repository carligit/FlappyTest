using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : BaseObjectPoolItem
{
    public float aliveTime;
    
    private float currentAliveTime;
    private Rigidbody2D rBody;

    void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        currentAliveTime += Time.deltaTime;

        if (currentAliveTime >= aliveTime)
        {
            this.Return();
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        Return();
    }

    public override void Reset()
    {
        currentAliveTime = 0;
    }
}
