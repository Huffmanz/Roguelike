using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] float speed = 7.5f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] ParticleSystem impactEffect;
    [SerializeField] int damage = 20;
    [SerializeField] int impactSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject.Instantiate(impactEffect, transform.position, transform.rotation);
        if(other.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyController>().DamageEnemy(damage);
        }
        if(other.tag == "Boss")
        {
            BossController.instance.TakeDamage(damage);
            Instantiate(BossController.instance.hitEffect, transform.position, transform.rotation);
        }
        else{
            AudioManager.instance.playSfx(impactSound);
        }
        Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
