using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Horizontal Moviment")]
    [SerializeField] private float speed = 10f; 
    [SerializeField] private float dir;
    [SerializeField] private bool facingRight = true;

    [Header("vertical Moviment")]
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private int numJump = 2;
    [SerializeField] private bool isGround = true;
    [SerializeField] private bool isTouchinWall = false;
    [SerializeField] private bool secondJump = false;

    [Header("Components")]
    [SerializeField] private Rigidbody2D playerRb; 
    [SerializeField] private Animator playerAnim;
    [SerializeField] private LayerMask groundLayer; 
    [SerializeField] private Transform footPosition;

    [Header("radius")]
    [SerializeField] private float radiusSphere = 0.3f;


    [Header("dash")]
    public float dashAtual;
    public bool canDash;
    public bool isDashing;
    public float dashSpeed;
    public float duracaoDash;
    public float dashCooldown;



    void Start()
    {

    }

    void Update()
    {
        dir = Input.GetAxisRaw("Horizontal"); //1 ou -1 
        
        isGround = Physics2D.OverlapCircle(footPosition.position, radiusSphere, groundLayer); //false se não tiver tocando true se tiver 
        
        checkInput();
        animUpdate();
        Dash();
    }   

    private void FixedUpdate() {
        movePlayer(dir);
    }

    private void movePlayer(float dir){
        playerRb.velocity = new Vector2(dir * speed, playerRb.velocity.y);

        playerAnim.SetFloat("horizontal",Mathf.Abs(dir));

        if(dir > 0 && !facingRight || dir < 0 && facingRight) 
            flip();
    }
    
    void flip (){
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0 , facingRight ? 0 : 180 , 0 );
    }

    private void checkInput(){

        if(isGround && !Input.GetKeyDown(KeyCode.Space))
            numJump = 1 ; 

        if(Input.GetKeyDown(KeyCode.Space) && numJump > 0 ){
            Jump();
        }


    }

    public void Jump(){    
        
        playerRb.velocity = Vector2.up * jumpForce;
        numJump--;
        
    }

    public void Dash (){
        if(Input.GetKeyDown(KeyCode.E) && isGround && canDash){
            if(dashAtual <= 0 )
                StopDash();
            else{
                isDashing = true;
                dashAtual -= Time.deltaTime;

                // createParticle();

                if(facingRight)
                    playerRb.velocity = Vector2.right * dashSpeed;
                else
                    playerRb.velocity = Vector2.left * dashSpeed;
            }
        }

        if(Input.GetKeyUp(KeyCode.E)){
            isDashing = false;
            canDash = true;
            dashAtual = duracaoDash;
        }
    }

    public void StopDash(){
        playerRb.velocity = Vector2.zero;
        StartCoroutine(DashCooldown());
    }

    IEnumerator DashCooldown(){
        dashAtual = duracaoDash;
        isDashing = false;
        canDash = false;

        yield return new WaitForSeconds(dashCooldown);
    }

    public void createParticle(){

    }

    public void animUpdate(){
        playerAnim.SetBool("isGround", isGround);
        playerAnim.SetFloat("vertical", playerRb.velocity.y);
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(footPosition.position,radiusSphere);
    }
    




    
}
