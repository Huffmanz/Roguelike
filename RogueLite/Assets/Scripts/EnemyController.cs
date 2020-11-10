using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    
    [SerializeField] float moveSpeed;
    [Header("Chase Player")]
    [SerializeField] bool shouldChase;
    [SerializeField] float chaseRange;

    [Header("Run Away")]
    [SerializeField] bool shouldRun;
    [SerializeField] float runRange;

    [Header("Wander")]
    [SerializeField] bool shouldWander;
    [SerializeField] float wanderLength;
    [SerializeField] float pauseLength;
    private Vector3 wanderDirection = Vector3.zero;
    private float wanderCounter;

    [Header("Patrol")]
    private float pauseCounter;
    [SerializeField] bool shouldPatrol;
    [SerializeField] Transform[] patrolPoints;
    private int currentPatrolPoint = 0;
    private Vector3 lastPos;
    private float idleTimer=.2f;

    [Header("Variables")]
    [SerializeField] int health = 150;
    [SerializeField] GameObject[] deathSplatters;
    [SerializeField] GameObject hurtEffect;

    [Header("Attack")]
    [SerializeField] bool canShoot;
    [SerializeField] GameObject ammo;
    [SerializeField] GameObject firePoint;
    [SerializeField] float shootRange = 3f;
    [SerializeField] float fireRate;
    [SerializeField] SpriteRenderer theBody;

    [Header("SFX")]
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
        if(shouldWander){
            pauseCounter = Random.Range(pauseLength *.25f, pauseLength * 1.25f);
        }
        lastPos = Vector3.zero;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        distanceToPlayer = Vector3.Distance(transform.position, PlayerController.instance.transform.position); 
        FacePlayer();
        if(theBody.isVisible && PlayerController.instance.gameObject.activeInHierarchy){
            moveDirection = Vector3.zero;
            if(canShoot && distanceToPlayer <= shootRange ){
                fireCounter-=Time.deltaTime;
                if(fireCounter <= 0){
                    fireCounter = fireRate;
                    AudioManager.instance.playSfx(shootSound);
                    GameObject.Instantiate(ammo, firePoint.transform.position, firePoint.transform.rotation);
                }
            }
            if(distanceToPlayer <= chaseRange && shouldChase)
            {
                moveDirection = PlayerController.instance.transform.position - transform.position;
            }
            else{
                if(shouldWander){
                    if(wanderCounter > 0){
                        moveDirection = wanderDirection;
                        wanderCounter-=Time.deltaTime;
                        if(wanderCounter <= 0){
                            pauseCounter = Random.Range(pauseLength *.25f, pauseLength * 1.25f);
                        }
                    }
                    if(pauseCounter > 0){
                        pauseCounter-=Time.deltaTime;
                        if(pauseCounter <= 0){
                            wanderCounter = Random.Range(wanderLength *.75f, wanderLength*1.25f);
                            wanderDirection = new Vector3(Random.Range(-1,1), Random.Range(-1,1), 0);
                        }
                    }
                }
                if(shouldPatrol){
                    moveDirection = patrolPoints[currentPatrolPoint].position - transform.position;
                    float distanceToPoint = Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position);
                    float distanceMoved = Vector3.Distance(lastPos, transform.position);
                    if( Mathf.Approximately(distanceMoved, 0)){
                        if(idleTimer > 0){
                            idleTimer-=Time.deltaTime;
                        }else{
                            idleTimer=.2f;
                            idleTimer-=Time.deltaTime;
                        }
                    }
                    else{
                        idleTimer = .2f;
                    }
                    if(distanceToPoint < 0.2f || idleTimer <= 0){
                        currentPatrolPoint ++;
                        if(currentPatrolPoint >= patrolPoints.Length ){
                            currentPatrolPoint = 0;
                        }
                    }
                    
                }

            }
            if(shouldRun && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < runRange){
                moveDirection = transform.position - PlayerController.instance.transform.position;
            }
            


            moveDirection.Normalize();
            anim.SetBool("isMoving", moveDirection != Vector3.zero);
            rb.velocity = moveDirection * moveSpeed;
        }else{
            rb.velocity = Vector2.zero;
        }
        lastPos = transform.position;
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
