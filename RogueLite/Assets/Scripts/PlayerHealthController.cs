﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerHealthController instance;
    [SerializeField] public int maxHealth;
    [SerializeField] float damangeInvinsibleLength = 1f;
    [SerializeField] int playerDeathSound;
    [SerializeField] int playerHurtSound;

    [SerializeField] public int currentHealth;


    private float invinsibleCounter;
    private void Awake() {
        instance = this;
    }
    void Start()
    {
        maxHealth = CharacterTracker.instance.maxHealth;
        currentHealth = CharacterTracker.instance.currentHealth; ;
        
        
        UpdateUI();
    }

    void Update(){
        if(invinsibleCounter > 0){
            invinsibleCounter-=Time.deltaTime;
            if(invinsibleCounter <0 ){
                PlayerController.instance.bodysr.color = new Color(PlayerController.instance.bodysr.color.r, PlayerController.instance.bodysr.color.g, PlayerController.instance.bodysr.color.b, 1f);
            }
        }
    }

    public void DamagePlayer(){
        if(invinsibleCounter <= 0)
        {
            currentHealth--;
            AudioManager.instance.playSfx(playerHurtSound);
            if(currentHealth <= 0){
                AudioManager.instance.playSfx(playerDeathSound);
                PlayerController.instance.gameObject.SetActive(false);
                UIController.instance.deathScreen.SetActive(true);
                AudioManager.instance.playGameOver();
            }
            UpdateUI();
            invinsibleCounter = damangeInvinsibleLength;
            PlayerController.instance.bodysr.color = new Color(PlayerController.instance.bodysr.color.r, PlayerController.instance.bodysr.color.g, PlayerController.instance.bodysr.color.b, .5f);
        }
    }

    public void HealPlayer(int healAmount){
        currentHealth+=healAmount;
        if(currentHealth > maxHealth){
            currentHealth=maxHealth;
        }
        UpdateUI();
    }

    public void IncreaseMaxHealth(int amount){
        maxHealth+=amount;
        currentHealth=maxHealth;
        UpdateUI();
    }

    private void UpdateUI(){
        UIController.instance.healthBar.maxValue = maxHealth;
        UIController.instance.healthBar.value = currentHealth; 
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString(); 
    }

    public void SetInvinsible(float amount){
        invinsibleCounter = amount;  
    }
}
