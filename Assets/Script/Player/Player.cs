using System.Collections;
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
    [SerializeField] private bool camMove= true;
    // [SerializeField] private bool isTouchinWall = false;
    // [SerializeField] private bool secondJump = false;

    [Header("Components")]
    [SerializeField] private Rigidbody2D playerRb; 
    [SerializeField] private Animator playerAnim;
    [SerializeField] private LayerMask groundLayer; 
    [SerializeField] private Transform footPosition;


    [Header("radius")]
    [SerializeField] private float radiusSphere = 0.3f;

    [Header("Wall Jump")]
    [SerializeField] private Transform wallCheck; 
    [SerializeField] private bool isTouchinWall = false;
    [SerializeField] private bool isSliding= false;
    [SerializeField] private float wallSlidingSpeed;
    [SerializeField] private float wallJumpforce;
    [SerializeField] private Vector2 wallJumDirection; 


    void Start()
    {

    }

    void Update()
    {
        dir = Input.GetAxisRaw("Horizontal"); //1 ou -1 
        

        CheckCollsion();
        animUpdate();

    }   

    private void FixedUpdate() {
        checkInput();
        CheckWallSliding();

        // if(wallJumping){
        //     playerRb.velocity = new Vector2(-dir * wallJumpforce.x, wallJumpforce.y);
        // }
    }

#region checkCollision
    public void CheckCollsion(){

        isGround = Physics2D.OverlapCircle(footPosition.position, radiusSphere, groundLayer); //false se não tiver tocando true se tiver 
        // wall
        // isTouchinWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, groundLayer); 
        isTouchinWall = Physics2D.OverlapCircle(wallCheck.position, radiusSphere, groundLayer); 

    }
#endregion



#region  movimentação
    private void movePlayer(float dir){
        playerRb.velocity = new Vector2(dir * speed, playerRb.velocity.y);

        playerAnim.SetFloat("horizontal",Mathf.Abs(dir));

        if(dir > 0 && !facingRight || dir < 0 && facingRight) 
            flip();

        if (isSliding){
            if(playerRb.velocity.y < -wallSlidingSpeed){
                // playerRb.velocity = new Vector2(playerRb.velocity.x, -wallSlidingSpeed);
                playerRb.velocity = new Vector2(playerRb.velocity.x , Mathf.Clamp(playerRb.velocity.y, - wallSlidingSpeed, float.MaxValue));

            }
        
        }
    }
    
    void flip (){
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0 , facingRight ? 0 : 180 , 0 );
    }
#endregion

#region  jump

    private void checkInput(){

        if(camMove){
            movePlayer(dir);
        }

        if(isGround && !Input.GetKeyDown(KeyCode.Space))
            numJump = 1 ; 

        if(Input.GetKeyDown(KeyCode.Space)){
            Jump();
        }
        
      

    }

    public void Jump(){    
        if( numJump > 0 && !isSliding){
            
            numJump--;
            playerRb.velocity = Vector2.up * jumpForce;
        }else if(isSliding){

            Vector2 force = new Vector2 (wallJumpforce * wallJumDirection.x  * -dir, wallJumpforce * wallJumDirection.y);
            playerRb.velocity = Vector2.zero;
            playerRb.AddForce(force, ForceMode2D.Impulse);

            StartCoroutine(StopMove());

        }
    }

    IEnumerator StopMove(){
        camMove = false; 
        transform.localScale = transform.localScale.x == 1 ? new Vector2(-1 , 1) : Vector2.one;

        yield return new WaitForSeconds(.3f);

        transform.localScale = Vector2.one;
        camMove = true;
    }

#endregion


#region wallJump
    public void CheckWallSliding (){
        if(isTouchinWall && !isGround && playerRb.velocity.y < 0  && dir !=0){
            isSliding = true;
        }else{
            isSliding = false;
        }
     }

#endregion


#region  plataform
    private void OnCollisionEnter2D(Collision2D other) {
        
        if(other.gameObject.CompareTag("plataform")){
            Debug.Log("oi");
            this.transform.parent = other.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.CompareTag("plataform")){
            this.transform.parent = null;
        }
    }
#endregion 


    public void animUpdate(){
        playerAnim.SetBool("isGround", isGround);
        playerAnim.SetFloat("vertical", playerRb.velocity.y);
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(footPosition.position,radiusSphere);

        // Gizmos.color = Color.red;
        // Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }




    
}
