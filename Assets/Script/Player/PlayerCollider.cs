using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private Enemy enemy;
    private Player player;

    [SerializeField] private float hurtForce;

    
    [Header("Collectebles")]
    private int _cherry = 0 ;

    // get and set
    public int cherry { get => _cherry; set => _cherry = value; }

    void Start()
    {
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Collecteble")){
            _cherry ++;
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        enemy = other.gameObject.GetComponent<Enemy>();

        if(other.gameObject.CompareTag("plataform")){
            this.transform.parent = other.transform;
        }

        if(other.gameObject.CompareTag("enemy")){

            if(player.PlayerState == PlayerState.falling){
                enemy.AnimDeath();    
                player.JumpInMonster();
            }else {
                player.PlayerState = PlayerState.hurt;
                if(other.gameObject.transform.position.x > transform.position.x){
                    player.PlayerRb.velocity = new Vector2(-hurtForce, player.PlayerRb.velocity.y);
                }else{
                    player.PlayerRb.velocity = new Vector2(hurtForce, player.PlayerRb.velocity.y);
                }

            }
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.CompareTag("plataform")){
            this.transform.parent = null;
        }
    }
}
