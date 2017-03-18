using UnityEngine;
using System.Collections;

public class Shield : PowerUp
{
    private Vector3 startingScale;

    void Awake()
    {
        startingScale = transform.localScale;
        this.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
        transform.localScale = Vector3.Lerp(startingScale, Vector3.zero, currentDuration / duration);
    }
}

