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
    [SerializeField] private PlayerState playerState;

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

    [Header("sound effect")]
    [SerializeField] private AudioSource footstep;
    

    internal PlayerState PlayerState { get => playerState; set => playerState = value; }
    public Rigidbody2D PlayerRb { get => playerRb; set => playerRb = value; }

    void Start()
    {
        PlayerState = PlayerState.idle;
        footstep = GetComponent<AudioSource>();
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
        PlayerRb.velocity = new Vector2(dir * speed, PlayerRb.velocity.y);

        if(dir > 0 && !facingRight || dir < 0 && facingRight) 
            flip();

        if (isSliding){
            if(PlayerRb.velocity.y < -wallSlidingSpeed){
                PlayerRb.velocity = new Vector2(PlayerRb.velocity.x , Mathf.Clamp(PlayerRb.velocity.y, - wallSlidingSpeed, float.MaxValue));
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

        if(camMove && PlayerState != PlayerState.hurt){
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
           PlayerRb.velocity = Vector2.up * jumpForce;
           PlayerState = PlayerState.jumping;
        }else if(isSliding){

            Vector2 force = new Vector2 (wallJumpforce * wallJumDirection.x  * -dir, wallJumpforce * wallJumDirection.y);
            PlayerRb.velocity = Vector2.zero;
            PlayerRb.AddForce(force, ForceMode2D.Impulse);

            StartCoroutine(StopMove());

        }
    }

    public void JumpInMonster(){
        PlayerRb.velocity = Vector2.up * jumpForce;
        PlayerState = PlayerState.jumping;
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
        if(isTouchinWall && !isGround && PlayerRb.velocity.y < 0  && dir !=0){
            isSliding = true;
        }else{
            isSliding = false;
        }
     }

#endregion

#region animation
    public void animUpdate(){
        playerAnim.SetInteger("State" , (int)PlayerState);
    } 
#endregion

#region stateMachineChance
    public void chanceStateMachine(){
        if(PlayerState == PlayerState.jumping){
            if(PlayerRb.velocity.y < .1f){
                PlayerState = PlayerState.falling;
            }
        }else if(PlayerState == PlayerState.hurt){
            if(Mathf.Abs( PlayerRb.velocity.x)  < .1f){
                PlayerState = PlayerState.idle;
            }

        }else if(PlayerState == PlayerState.falling){
            if(isGround)
                PlayerState = PlayerState.idle;
        }else if(Mathf.Abs(PlayerRb.velocity.x) > 2f){
            PlayerState = PlayerState.run;
        }else {
            PlayerState = PlayerState.idle;
        }


    }
#endregion


    private void FootStep (){
        footstep.Play();
    }


}
