using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;

    // Start is called before the first frame update
    [SerializeField] float moveSpeed = 5;
    [SerializeField] Rigidbody2D rigidbody;
    [SerializeField] Transform gunArm;
    [SerializeField] Animator anim;
    [SerializeField] GameObject ammo;
    [SerializeField] Transform firePoint;
    [SerializeField] float timeBetweenAttack = 1f;
    [SerializeField] public SpriteRenderer bodysr;
    [SerializeField] public float dashSpeed = 8f;
    [SerializeField] public float dashLength = 0.5f;
    [SerializeField] public float dashCooldown = 1f;
    [SerializeField] public float dashInvinsible = 0.5f;
    
    

    private float attackCounter;
    private float activeMoveSpeed;
    private Camera cam;
    private Vector2 moveInput;
    private float dashCounter;
    private float dashCooldownCounter;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        cam = Camera.main;
        attackCounter = timeBetweenAttack;
        activeMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        rigidbody.velocity = moveInput * activeMoveSpeed;


        Vector3 mousePosition = Input.mousePosition;
        //Position of camera in screen space
        Vector3 screenPoint = cam.WorldToScreenPoint(transform.localPosition);

        if(mousePosition.x < screenPoint.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            gunArm.localScale = new Vector3(-1f, -1f, 1f);
        }
        else
        {
            transform.localScale = Vector3.one;
            gunArm.localScale = Vector3.one;
        }

        //rotate gun arm
        Vector2 offset = new Vector2(mousePosition.x - screenPoint.x, mousePosition.y - screenPoint.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        gunArm.rotation = Quaternion.Euler(0f, 0f, angle);

        if (Input.GetMouseButtonDown(0))
        {
            GameObject.Instantiate(ammo, firePoint.position, firePoint.rotation);
            attackCounter = timeBetweenAttack;
        }

        if (Input.GetMouseButton(0))
        {
            attackCounter -= Time.deltaTime;
            if(attackCounter <= 0)
            {
                GameObject.Instantiate(ammo, firePoint.position, firePoint.rotation);
                attackCounter = timeBetweenAttack;
            }

        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownCounter <= 0){
            activeMoveSpeed = dashSpeed;
            dashCounter = dashLength;
            anim.SetTrigger("dash");
            PlayerHealthController.instance.SetInvinsible(dashInvinsible);
        }
        if(dashCounter > 0){
            dashCounter-=Time.deltaTime;
            if(dashCounter <= 0){
                activeMoveSpeed = moveSpeed;
                dashCooldownCounter = dashCooldown;
            }
        }
        if(dashCooldownCounter > 0){
            dashCooldownCounter-=Time.deltaTime;
        }

        anim.SetBool("isMoving", moveInput != Vector2.zero); 
        
    }
}
