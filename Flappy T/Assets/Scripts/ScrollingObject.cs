using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class ScrollingObject : MonoBehaviour
{
    public GameManager.ScrollObjectLayer scrollLayer;
    public bool tiling;
    public float xBoundary;
    public float xTarget;
    
    private Rigidbody2D rBody;

    void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        SetVelocity();
    }
   
    void Update()
    {
        if (GameManager.instance.GameOver)
        {
            rBody.velocity = Vector2.zero;
        }
        else if (tiling)
        {
            if (this.transform.position.x <= xBoundary)
            {
                float remainder = xBoundary - transform.position.x;
                this.transform.position = new Vector3(xTarget - remainder, transform.position.y, transform.position.z);
            }
        }
    }

    public void SetVelocity()
    {
        rBody.velocity = new Vector2(GameManager.instance.GetScrollSpeed(scrollLayer), 0);
    }
}
