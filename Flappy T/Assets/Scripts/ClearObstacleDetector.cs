using UnityEngine;
using System.Collections;

/// <summary>
/// Attached to a collider to award points
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ClearObstacleDetector : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        GameManager.instance.AddToScore();
    }
}
