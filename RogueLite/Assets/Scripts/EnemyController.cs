using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float moveSpeed;
    [SerializeField] float chaseRange;
    [SerializeField] int health = 150;
    [SerializeField] GameObject[] deathSplatters;
    [SerializeField] GameObject hurtEffect;
    [SerializeField] bool canShoot;
    [SerializeField] GameObject ammo;
    [SerializeField] GameObject firePoint;
    [SerializeField] float fireRate;
    [SerializeField] SpriteRenderer theBody;
    [SerializeField] float shootRange = 3f;
    [SerializeField] int deathSound=1;
    [SerializeField] int hurtSound=2;
    [SerializeField] int shootSound;

    private float fireCounter;
    private Vector3 moveDirection;
    private GameObject player;
    private Animator anim;

    private float distanceToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        fireCounter = fireRate;
        
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, PlayerController.instance.transform.position); 
        FacePlayer();
        if(theBody.isVisible && PlayerController.instance.gameObject.activeInHierarchy){
            if(canShoot && distanceToPlayer <= shootRange ){
                fireCounter-=Time.deltaTime;
                if(fireCounter <= 0){
                    fireCounter = fireRate;
                    AudioManager.instance.playSfx(shootSound);
                    GameObject.Instantiate(ammo, firePoint.transform.position, firePoint.transform.rotation);
                }
            }
            if(distanceToPlayer <= chaseRange)
            {
                moveDirection = PlayerController.instance.transform.position - transform.position;

            }
            else
            {
                moveDirection = Vector3.zero;
            }

            moveDirection.Normalize();
            anim.SetBool("isMoving", moveDirection != Vector3.zero);
            rb.velocity = moveDirection * moveSpeed;
        }else{
            rb.velocity = Vector2.zero;
        }
    }

    public void DamageEnemy(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            int rotation = Random.Range(0,4);

            GameObject deathSplatter = deathSplatters[Random.Range(0, deathSplatters.Length)];
            GameObject.Instantiate(deathSplatter, transform.position, Quaternion.Euler(0,0,rotation * 90));
            AudioManager.instance.playSfx(deathSound);
            Destroy(gameObject);
        }else{
            AudioManager.instance.playSfx(hurtSound);
            GameObject.Instantiate(hurtEffect, transform.position, transform.rotation);
        }
    }

    void FacePlayer(){
        if(PlayerController.instance.gameObject.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            firePoint.transform.localScale = new Vector3(-1f, -1f, 1f);
        }
        else
        {
            transform.localScale = Vector3.one;
            firePoint.transform.localScale = Vector3.one;
        }
    }
}
