using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour 
{
    public float jumpForce = 200;
    public UnityEvent OnDeath;
    public Shield shield;
    
    private Rigidbody2D rBody;
    private bool isDead;

	void Start () 
    {
        rBody = GetComponent<Rigidbody2D>();
        rBody.isKinematic = true;
	}
	
    void OnCollisionEnter2D(Collision2D col)
    {
        isDead = true;
        rBody.velocity = Vector2.zero;
        OnDeath.Invoke();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("PowerUp"))
        {
            col.GetComponent<PowerUpPickup>().Return();
            shield.Reset();
            shield.gameObject.SetActive(true);
        }
    }

	// Update is called once per frame
	void Update () 
    {
        if (isDead)
        { 
            return;
        }

        if (Input.GetMouseButtonDown(0) && !isDead)
        {
            if (!GameManager.instance.GameStarted)
                GameManager.instance.StartGame();
            
            rBody.isKinematic = false;
            rBody.velocity = Vector2.zero;
            rBody.AddForce(new Vector2(1, jumpForce));
        }
	}
}
