using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] public string weaponName;
    [SerializeField] GameObject ammo;
    [SerializeField] Transform firePoint;
    [SerializeField] float timeBetweenAttack = 1f;
    [SerializeField] public int shootSound;
    [SerializeField] public Sprite gunUI;
    [SerializeField] public int price;
    [SerializeField] public Sprite shopSprite;
    // Update is called once per frame
    private float attackCounter;
    void Start()
    {
        attackCounter = timeBetweenAttack;
    }

    void Update()
    {
        if(PlayerController.instance.canMove && !LevelManager.instance.isPaused) { 
            if(attackCounter > 0)
            {
                attackCounter -= Time.deltaTime;
            }
            else
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
                {
                    GameObject.Instantiate(ammo, firePoint.position, firePoint.rotation);
                    attackCounter = timeBetweenAttack;
                    AudioManager.instance.playSfx(shootSound);
                }
            }
        }
    }
}
