﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public static BossController instance;
    public BossAction[] actions;
    public Rigidbody2D rb;
    public Vector2 moveDirection;
    public int currentHealth;
    public GameObject deathEffect;
    public GameObject hitEffect;
    public GameObject levelExit;
    public BossSequence[] sequences;

    private int currentAction;
    private float actionTimer;
    private float shotTimer;
    private int currentSequence;
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        actions = sequences[currentSequence].actions;
        actionTimer = actions[currentAction].actionLength;
        UIController.instance.bossSlider.maxValue = currentHealth;
        UIController.instance.bossSlider.value = currentHealth;
    }
    // Update is called once per frame
    void Update()
    {
        if(actionTimer > 0)
        {
            actionTimer -= Time.deltaTime;
            moveDirection = Vector2.zero;
            if (actions[currentAction].shouldMove)
            {
                if (actions[currentAction].shouldChasePlayer)
                {
                    moveDirection = PlayerController.instance.transform.position - transform.position;
                    moveDirection.Normalize();
                }
                if(actions[currentAction].moveToPoints && Vector3.Distance(transform.position, actions[currentAction].pointToMove.position) > .5f)
                {
                    moveDirection = actions[currentAction].pointToMove.position - transform.position;
                    moveDirection.Normalize();
                }
            }


            rb.velocity = moveDirection * actions[currentAction].moveSpeed;

            if (actions[currentAction].shouldShoot)
            {
                shotTimer -= Time.deltaTime;
                if(shotTimer <= 0)
                {
                    shotTimer = actions[currentAction].timeBetweenShots;
                    foreach(Transform t in actions[currentAction].shotPoints)
                    {
                        Instantiate(actions[currentAction].itemToShoot, t.position, t.rotation);
                    }
                }
            }
        }
        else
        {
            currentAction++;
            if (currentAction >= actions.Length) currentAction = 0;
            actionTimer = actions[currentAction].actionLength;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if(currentHealth <= 0)
        {
            gameObject.SetActive(false);
            Instantiate(deathEffect, transform.position, transform.rotation);
            
            if(Vector3.Distance(PlayerController.instance.transform.position, levelExit.transform.position) < 2f)
            {
                levelExit.transform.position += new Vector3(4, 0, 0);
            }
            levelExit.SetActive(true);
            UIController.instance.bossHealthDisplay.gameObject.SetActive(false);
        }
        else
        {
            if(currentHealth <= sequences[currentSequence].endSequenceHealth && currentSequence < sequences.Length - 1)
            {
                currentSequence++;
                actions = sequences[currentSequence].actions;
                currentAction = 0;
                actionTimer = actions[currentAction].actionLength;
            }
        }
        UIController.instance.bossSlider.value = currentHealth;
    }
}

[System.Serializable]
public class BossAction
{
    [Header("Action")]
    public float actionLength;
    public bool shouldMove;
    public bool shouldChasePlayer;
    public bool moveToPoints;
    public float moveSpeed;
    public Transform pointToMove;

    public bool shouldShoot;
    public GameObject itemToShoot;
    public float timeBetweenShots;
    public Transform[] shotPoints;
}

[System.Serializable]
public class BossSequence
{
    [Header("Sequence")]
    public BossAction[] actions;
    public int endSequenceHealth;
}