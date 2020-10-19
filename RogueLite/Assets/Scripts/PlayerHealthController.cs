using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerHealthController instance;
    [SerializeField] int maxHealth;
    public int currentHealth {get; private set;}

    private void Awake() {
        instance = this;
    }
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void DamagePlayer(){
        currentHealth--;
        if(currentHealth <= 0){
            PlayerController.instance.gameObject.SetActive(false);
        }
    }
}
