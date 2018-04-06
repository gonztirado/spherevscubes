using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    public float lifeTime;
    public int damage;
    public LayerMask layermask;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(transform.forward * bulletSpeed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("entra1");
        if (layermask == (layermask | (1 << collision.gameObject.layer)))
        {
            Debug.Log("entra2");
            
            if (collision.GetComponentInParent<Health>() != null)
            {
                Debug.Log("entra3");
                collision.GetComponentInParent<Health>().TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}