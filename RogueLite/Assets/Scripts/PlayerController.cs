using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;

    // Start is called before the first frame update
    [SerializeField] float moveSpeed = 5;
    [SerializeField] Rigidbody2D rigidbody;
    [SerializeField] public Transform gunArm;
    [SerializeField] Animator anim;
   
    [SerializeField] public SpriteRenderer bodysr;
    [SerializeField] public float dashSpeed = 8f;
    [SerializeField] public float dashLength = 0.5f;
    [SerializeField] public float dashCooldown = 1f;
    [SerializeField] public float dashInvinsible = 0.5f;
    
    [SerializeField] public int dashSound;
    //[SerializeField] public int shootSound;
    [HideInInspector] public bool canMove = true;
    public List<Gun> availableGuns = new List<Gun>();
    private float activeMoveSpeed;
    private Vector2 moveInput;
    public float dashCounter {get; private set;}
    private float dashCooldownCounter;
    [HideInInspector] public int currentGun = 0;
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //cam = Camera.main;
        //attackCounter = timeBetweenAttack;
        activeMoveSpeed = moveSpeed;
        //UpdateGunUI();
    }

    // Update is called once per frame
    void Update()
    {
        if(!canMove || LevelManager.instance.isPaused){
          rigidbody.velocity = Vector2.zero;
          anim.SetBool("isMoving",false);
          return;   
        }
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        rigidbody.velocity = moveInput * activeMoveSpeed;


        Vector3 mousePosition = Input.mousePosition;
        //Position of camera in screen space
        Vector3 screenPoint = CameraController.instance.mainCamera.WorldToScreenPoint(transform.localPosition);

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

        if (Input.GetKeyDown(KeyCode.Tab))
        {           
            if (availableGuns.Count > 0)
            {
                currentGun++;
                if (currentGun >= availableGuns.Count) currentGun = 0;
                SwitchGun();
            }
            else
            {
                Debug.LogError("Player has no guns");
            }
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownCounter <= 0){
            activeMoveSpeed = dashSpeed;
            dashCounter = dashLength;
            anim.SetTrigger("dash");
            AudioManager.instance.playSfx(dashSound);
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
    public void SwitchGun()
    {
        foreach(Gun gun in availableGuns)
        {
            gun.gameObject.SetActive(false);
        }
        availableGuns[currentGun].gameObject.SetActive(true);
        UpdateGunUI();
    }

    private void UpdateGunUI()
    {
        UIController.instance.currentGun.sprite = availableGuns[currentGun].gunUI;
        UIController.instance.currentGunName.text = availableGuns[currentGun].weaponName;
    }
}
