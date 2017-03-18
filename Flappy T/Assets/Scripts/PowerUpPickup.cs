using UnityEngine;
using System.Collections;

public class PowerUpPickup : BaseObjectPoolItem
{
    public ScrollingObject scrollObject;

    public override void Reset()
    {
        scrollObject.SetVelocity();
    }
}
