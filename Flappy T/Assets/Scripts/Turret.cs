using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Turret : MonoBehaviour
{
    [SerializeField] private ObjectPooler bulletPool;
    public float bulletSpeed;
    public bool proximityFire;
    public bool followPlayer;
    public float leadingDistance;
    public float fireRate;
    public CircleCollider2D proximity;  
    public Rigidbody2D obstacleRigidBody;

    private float lastShotTime;

    void Start()
    {
        lastShotTime = fireRate;
    }

    void Update()
    {
        if (!proximityFire && !GameManager.instance.GameOver)
            Fire(GameManager.instance.GetPlayer());
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (proximityFire && !GameManager.instance.GameOver)
            Fire(col.gameObject);
    }

    void Fire(GameObject player)
    {
        if (player == null)
            return;

        lastShotTime += Time.deltaTime;

        if (lastShotTime >= fireRate)
        {
            lastShotTime = 0;

            var bullet = bulletPool.GetItem();

            if (bullet != null)
            {
                bullet.transform.position = transform.position;
                
                var playerRBody = player.GetComponent<Rigidbody2D>();
                var bulletRBody = bullet.GetComponent<Rigidbody2D>();
                
                if (followPlayer)
                {
                    Vector3 leadingOffset = (obstacleRigidBody.velocity) * leadingDistance;
                    var leadingPosition = player.transform.position - leadingOffset;
                    var targetDir = (leadingPosition - transform.position).normalized;
                    bulletRBody.velocity = targetDir * bulletSpeed;
                    
                }
                else
                {
                    bulletRBody.velocity = transform.up;  
                }

                bulletRBody.velocity += obstacleRigidBody.velocity;
                var bulletDir = bulletRBody.velocity.normalized;
                var targetRot = Quaternion.FromToRotation(Vector3.left, bulletDir);
                bullet.transform.rotation = targetRot;
            }
        }
    }
}
