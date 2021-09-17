using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [RequireComponent(typeof (SpriteController))]
public class PlayerOne : MonoBehaviour
{
    public int playerHealth;
    public int playerMaxHealth;
    public int absorbScore;
    public int MOVESPEED;
    public int DAMAGE;
    public bool isDead;
    public bool onPlatform;
    public bool gotHit;
    public GameObject hitEnemyCollider;
    public GameObject damagePlayerCollider;
    public Rigidbody2D rig;
    public SpriteRenderer sr;
    public SpriteController spriteController;
    public bool isMoving;
    public bool isJumping;
    public GameObject emitter;
    public bool canMove;
    public int playerPoints;
    public bool isPlayerOne;
    public CircleCollider2D circleCollider;

    // Start is called before the first frame update
    void Start()
    {
        spriteController = gameObject.GetComponent<SpriteController>();
        GameManager.instance.playersInGame.Add(this);
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
        absorbScore = 1;
    }
     void OnCollisionEnter2D(Collision2D collision)
    {
        //enable jump when we hit a platform
        if (collision.gameObject.CompareTag("Platform") && !isDead)
        {
            onPlatform = true;
            StartCoroutine(DoLand());

        IEnumerator DoLand()
        {
             spriteController.AnimateLand();
             yield return new WaitForSeconds(1f);
        }
           
        }
        //enable jump when we hit a platform
        if (collision.gameObject.CompareTag("Obstacle") && !gotHit && !isDead)
        {
            TakeDamage(collision.gameObject.GetComponent<Obstacle>().DAMAGETODEAL);
            rig.velocity = new Vector2 (MOVESPEED, rig.velocity.y);
  
        }
          if (collision.gameObject.CompareTag("Bucket") && !isDead)
        {
           if(collision.gameObject.GetComponent<Bucket>().isPlayerTwoBucket){
               PlayerOne playerToGivePoints = GameManager.instance.playersInGame.Find(x => !x.isPlayerOne);
                playerToGivePoints.playerPoints += collision.gameObject.GetComponent<Bucket>().POINTSTOADD * absorbScore;
                isDead = true;

           }
           else{
               PlayerOne playerToGivePoints = GameManager.instance.playersInGame.Find(x => x.isPlayerOne);
                playerToGivePoints.playerPoints += collision.gameObject.GetComponent<Bucket>().POINTSTOADD * absorbScore;
                isDead = true;
           }
  
        }
    } 
   /* void OnCollisionExit2D(Collision2D collision)
    {
        //enable jump when we hit a platform
        if (collision.gameObject.CompareTag("Platform"))
        {
            onPlatform = false;
        }
    }*/
     public void Move ()
    {
        if(isPlayerOne && !isDead)
     {   // get the horizontal and vertical inputs
         if (Input.GetKey (KeyCode.A)){
            float x = -1;
                rig.velocity = new Vector2 (x * MOVESPEED, rig.velocity.y);
                sr.flipX = true;
                isMoving = true;

         }
         if(Input.GetKey (KeyCode.D)){
             float x = 1;
                rig.velocity = new Vector2 (x * MOVESPEED, rig.velocity.y);
                sr.flipX = false;
               isMoving = true;
         }
       
        if(onPlatform &&  Input.GetKey (KeyCode.W))
            {
               
                     rig.velocity = new Vector2 (rig.velocity.x, MOVESPEED);
                      onPlatform = false;
                      isJumping = true;
                 
            }
       } else if(!isDead){
           // get the horizontal and vertical inputs
         if (Input.GetKey (KeyCode.LeftArrow)){
            float x = Input.GetAxis("Horizontal");
                rig.velocity = new Vector2 (x * MOVESPEED, rig.velocity.y);
                isMoving = true;
                sr.flipX = true;
       

         }
         if (Input.GetKey (KeyCode.RightArrow)){
            float x = Input.GetAxis("Horizontal");
                rig.velocity = new Vector2 (x * MOVESPEED, rig.velocity.y);
                isMoving = true;
                sr.flipX = false;
                

         }
       
        if(onPlatform && Input.GetKey (KeyCode.UpArrow))
            {
               
                     rig.velocity = new Vector2 (rig.velocity.x, MOVESPEED);
                      onPlatform = false;
                      isJumping = true;
                 
            }
       
      
        // apply that to our velocity
        
       }
      
        // apply that to our velocity
        
        
    }
    public void Revive(){
        isDead = false;
        absorbScore = 1;
           var tempColor = sr.color;
            tempColor.a = 255f;
            sr.color = tempColor;
        playerHealth = playerMaxHealth;
        spriteController.enabled = true;
        circleCollider.enabled = true;
        //change to spawn point 0
        if(isPlayerOne){
            transform.position = GameManager.instance.spawnPoints[0].transform.position;
        }
        else {
             transform.position = GameManager.instance.spawnPoints[1].transform.position;
        }
        

    }
    public void Die(){
         spriteController.enabled = false;
          var tempColor = sr.color;
            tempColor.a = 0f;
            sr.color = tempColor;
            isDead = true;
            GameManager.instance.deadPlayers.Add(this.gameObject);
             circleCollider.enabled = false;
            
           
    }
    public void TakeDamage(int damage){
      if(!gotHit && !isDead) {
       playerHealth -= damage;
        gotHit = true;
      }
        if(playerHealth <= 0){
          
              //disable graphics
                      isDead = true;
            if(isPlayerOne){
                 PlayerOne playerTwo = GameManager.instance.playersInGame.Find(x => !x.isPlayerOne);
                 playerTwo.absorbScore = 2;
             } else{
                  PlayerOne playerOne = GameManager.instance.playersInGame.Find(x => x.isPlayerOne);
                 playerOne.absorbScore = 2;
             }
           //Die();
            //disable triggers
        }
    }
    // Update is called once per frame
    void Update()
    {
     if(isDead){
         spriteController.enabled = false;
          var tempColor = sr.color;
            tempColor.a = 0f;
            sr.color = tempColor;
             circleCollider.enabled = false;
     }
        if (gotHit)
        {
          emitter.SetActive(true);   
        StartCoroutine(DamageFlash());

        IEnumerator DamageFlash()
        {
            canMove = false;
            sr.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            sr.color = Color.white;
            yield return new WaitForSeconds(1f);
            emitter.SetActive(false);
            gotHit = false;
            canMove = true;
        }
        }
         if (isMoving)
        {
        StartCoroutine(MoveAnimation());

        IEnumerator MoveAnimation()
        {
            spriteController.AnimateMove();
            yield return new WaitForSeconds(1f);
            isMoving = false;
        }
        }
          if (isJumping)
        {
        StartCoroutine(JumpAnimation());

        IEnumerator JumpAnimation()
        {
            spriteController.AnimateJump();
            yield return new WaitForSeconds(1f);
            isJumping = false;
        }
        }
            if (!onPlatform)
        {
        StartCoroutine(JumpAnimation());

        IEnumerator JumpAnimation()
        {
            spriteController.AnimateJump();
            yield return new WaitForSeconds(1f);
            isJumping = false;
        }
        }
        if(!isMoving && !isJumping){
            spriteController.AnimateIdle();
        }
        //movement
        if(canMove)
            Move();
        //check for colliders
    }
}
