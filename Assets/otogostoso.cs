using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class otogostoso : MonoBehaviour
{
    [SerializeField] private float speed = 10f; 
    [SerializeField] private float dir;
    [SerializeField] private Rigidbody2D playerRb; 
    [SerializeField] private Animator playerAnim; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
           dir = Input.GetAxisRaw("Horizontal"); //1 ou -1 
    }

        private void FixedUpdate() {
        movePlayer(dir);
    }

    private void movePlayer(float dir){
        playerRb.velocity = new Vector2(dir * speed, playerRb.velocity.y);

        playerAnim.SetFloat("horizontal",Mathf.Abs(dir));

    }
}
