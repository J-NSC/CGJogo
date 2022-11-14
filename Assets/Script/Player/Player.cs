using System.Collections;
using UnityEngine;

enum PlayerState {
    idle,
    run,
    jumping,
    falling,
    hurt
}

public class Player : MonoBehaviour
{
    private Enemy eagle;
    [SerializeField] private PlayerState playerState;
    [SerializeField] private float hurtForce;

    [Header("Horizontal Moviment")]
    [SerializeField] private float speed = 10f; 
    [SerializeField] private float dir;
    [SerializeField] private bool facingRight = true;

    [Header("vertical Moviment")]
    [SerializeField] private int numJump = 2;
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private bool isGround = true;
    [SerializeField] private bool camMove= true;

    [Header("Components")]
    [SerializeField] private Rigidbody2D playerRb; 
    [SerializeField] private Animator playerAnim;
    [SerializeField] private LayerMask groundLayer; 
    [SerializeField] private Transform footPosition;
    [SerializeField] private Transform wallCheck; 

    [Header("radius")]
    [SerializeField] private float radiusSphere = 0.3f;

    [Header("Wall Jump")]
    [SerializeField] private bool isTouchinWall = false;
    [SerializeField] private bool isSliding= false;
    [SerializeField] private float wallSlidingSpeed;
    [SerializeField] private float wallJumpforce;
    [SerializeField] private Vector2 wallJumDirection; 

    [Header("Collectebles")]
    private int _cherry = 0 ;

    
    public int cherry { get => _cherry; set => _cherry = value; }

    void Start()
    {
        playerState = PlayerState.idle;
    }

    void Update()
    {
        dir = Input.GetAxisRaw("Horizontal"); //1 ou -1 
        

        CheckCollsion();
        animUpdate();
        chanceStateMachine();

    }   

    private void FixedUpdate() {
        checkInput();
        CheckWallSliding();
    }




#region checkCollision
    public void CheckCollsion(){

        isGround = Physics2D.OverlapCircle(footPosition.position, radiusSphere, groundLayer); //false se não tiver tocando true se tiver 
        // wall
        isTouchinWall = Physics2D.OverlapCircle(wallCheck.position, radiusSphere, groundLayer); // false se não tiver tocando na parede 

    }
#endregion

#region  movimentação
    private void movePlayer(float dir){
        playerRb.velocity = new Vector2(dir * speed, playerRb.velocity.y);

        if(dir > 0 && !facingRight || dir < 0 && facingRight) 
            flip();

        if (isSliding){
            if(playerRb.velocity.y < -wallSlidingSpeed){
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

        if(camMove && playerState != PlayerState.hurt){
            movePlayer(dir);
        }

        if(isGround && !Input.GetKeyDown(KeyCode.Space))
            numJump = 1 ; 

        if(Input.GetKeyDown(KeyCode.Space) ){
            Jump();
            

        }
        
      

    }

    public void Jump(){    
 
        if(numJump > 0 && !isSliding){
           numJump--;  
           playerRb.velocity = Vector2.up * jumpForce;
           playerState = PlayerState.jumping;
        }else if(isSliding){

            Vector2 force = new Vector2 (wallJumpforce * wallJumDirection.x  * -dir, wallJumpforce * wallJumDirection.y);
            playerRb.velocity = Vector2.zero;
            playerRb.AddForce(force, ForceMode2D.Impulse);

            StartCoroutine(StopMove());

        }
    }

    public void JumpInMonster(){
        playerRb.velocity = Vector2.up * jumpForce;
        playerState = PlayerState.jumping;
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

#region animation
    public void animUpdate(){
        playerAnim.SetInteger("State" , (int)playerState);
    } 
#endregion

#region stateMachineChance
    public void chanceStateMachine(){
        if(playerState == PlayerState.jumping){
            if(playerRb.velocity.y < .1f){
                playerState = PlayerState.falling;
            }
        }else if(playerState == PlayerState.hurt){
            if(Mathf.Abs( playerRb.velocity.x)  < .1f){
                playerState = PlayerState.idle;
            }

        }else if(playerState == PlayerState.falling){
            if(isGround)
                playerState = PlayerState.idle;
        }else if(Mathf.Abs(playerRb.velocity.x) > 2f){
            playerState = PlayerState.run;
        }else {
            playerState = PlayerState.idle;
        }


    }
#endregion

#region Collision and Trigger


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Collecteble")){
            _cherry ++;
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        eagle = other.gameObject.GetComponent<Enemy>();

        if(other.gameObject.CompareTag("plataform")){
            this.transform.parent = other.transform;
        }

        if(other.gameObject.CompareTag("enemy")){

            if(playerState == PlayerState.falling){
                eagle.death();    
                Destroy(other.gameObject);
                JumpInMonster();
            }else {
                playerState = PlayerState.hurt;
                if(other.gameObject.transform.position.x > transform.position.x){
                    playerRb.velocity = new Vector2(-hurtForce, playerRb.velocity.y);
                }else{
                    playerRb.velocity = new Vector2(hurtForce, playerRb.velocity.y);
                }

            }
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.CompareTag("plataform")){
            this.transform.parent = null;
        }
    }

    
#endregion



    
}
