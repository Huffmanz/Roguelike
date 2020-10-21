using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    public static UIController instance;
    [SerializeField] 
    public Slider healthBar;
    [SerializeField] 
    public Text healthText;
    [SerializeField] public GameObject deathScreen;

    
    
    private void Awake() {
        instance=this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
