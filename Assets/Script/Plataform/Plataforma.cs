using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Plataforma : MonoBehaviour
{
    public float speed = 3f;
    public Transform pointA;
    public PlatformEffector2D efeito;
    public Transform pointB;

    public bool moveDown = true;
    public bool moveRight = true;
    public bool TypePlataform = false;
    void Start()
    {
        
    }

    void Update()
    {
        if(TypePlataform){
            PlataformVertical();
        }else{
            PlataformHorizontal();
        }
        

    }

    private void PlataformVertical(){
        if(transform.position.y > pointA.position.y){
            moveDown = true;
        }if(transform.position.y < pointB.position.y)
            moveDown = false;

        if(moveDown)
            transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime);
        else    
            transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);  
    }

    public void PlataformHorizontal(){
        if(transform.position.x < pointA.position.x)
            moveRight = true;
        if(transform.position.x > pointB.position.x)
            moveRight = false;

        if(moveRight)
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime , transform.position.y );
        else    
            transform.position = new Vector2(transform.position.x - speed * Time.deltaTime,  transform.position.y );
    }


    // private void OnTriggerEnter2D(Collider2D other) {
    //     if(other.CompareTag("Player")){
    //         StartCoroutine(ChangeSide());
    //         Debug.Log("x");
    //     }
    // }

    // private IEnumerator ChangeSide(){
    //     yield return new WaitForSeconds(2);
    //     efeito.rotationalOffset = 180;
    // }
}
