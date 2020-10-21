using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerHealthController instance;
    [SerializeField] int maxHealth;
    [SerializeField] float damangeInvinsibleLength = 1f;
    public int currentHealth {get; private set;}


    private float invinsibleCounter;
    private void Awake() {
        instance = this;
    }
    void Start()
    {
        currentHealth = maxHealth;
        
        
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
            if(currentHealth <= 0){
                PlayerController.instance.gameObject.SetActive(false);
                UIController.instance.deathScreen.SetActive(true);
            }
            UpdateUI();
            invinsibleCounter = damangeInvinsibleLength;
            PlayerController.instance.bodysr.color = new Color(PlayerController.instance.bodysr.color.r, PlayerController.instance.bodysr.color.g, PlayerController.instance.bodysr.color.b, .5f);
        }
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
