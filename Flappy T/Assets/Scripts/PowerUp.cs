using UnityEngine;
using System.Collections;


public abstract class PowerUp : MonoBehaviour
{
    public float duration;
    
    protected float currentDuration;

    public void Reset()
    {
        currentDuration = 0;
    }

    protected virtual void Update()
    {
        if (currentDuration >= duration)
        {
            currentDuration = 0;
            this.gameObject.SetActive(false);
        }

        currentDuration += Time.deltaTime;
    }
}