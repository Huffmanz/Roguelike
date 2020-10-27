using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPieces : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float deceleration = 5f;

    [SerializeField] float lifeTime = 3f;
    [SerializeField] float fadeSpeed = 1f;
    private Vector3 moveDirection;
    private SpriteRenderer spriteRenderer;

    void Start(){
        moveDirection.x = Random.Range(-speed, speed);
        moveDirection.y = Random.Range(-speed, speed);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update(){
        
        transform.position += moveDirection * Time.deltaTime;
        moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, deceleration * Time.deltaTime);
        lifeTime-=Time.deltaTime;
        if(lifeTime <= 0){
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.MoveTowards(spriteRenderer.color.a, 0f,fadeSpeed*Time.deltaTime));
            if(spriteRenderer.color.a == 0){
                Destroy(gameObject);
            }
        } 
    }
}
