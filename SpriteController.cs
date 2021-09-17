using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{   
    public Sprite baseSprite;
    public Sprite[] idleSprite;
    public Sprite[] landSprite;
    public Sprite[] movementSprite;
    public Sprite[] jumpSprite;
    public int spriteControllerID;
    public int spriteFrame;
    public SpriteRenderer unitSpriteRenderer;

    public float timer;
    public bool startTimer = false;
    public float frameTime;
    //int to set dir
    //1 - front 2 - back 3 - left 4 - right
    public int unitDir;

    //idle anim functions with dir
    // Start is called before the first frame update
    
    void Start()
    {
        unitSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        unitSpriteRenderer.sprite = baseSprite;
     
    }
 
    
    public void AnimateMove(){

         startTimer = true;


     if(timer >= frameTime){

            timer -= frameTime;
            spriteFrame = (spriteFrame + 1) % movementSprite.Length;
            unitSpriteRenderer.sprite = movementSprite[spriteFrame];
            unitDir = 3;
        }
    }
     public void AnimateIdle(){

         startTimer = true;


     if(timer >= frameTime){

            timer -= frameTime;
            spriteFrame = (spriteFrame + 1) % idleSprite.Length;
            unitSpriteRenderer.sprite = idleSprite[spriteFrame];
            unitDir = 3;
        }
    }
     public void AnimateLand(){

         startTimer = true;


     if(timer >= frameTime){

            timer -= frameTime;
            spriteFrame = (spriteFrame + 1) % landSprite.Length;
            unitSpriteRenderer.sprite = landSprite[spriteFrame];
            unitDir = 3;
        }
    }
    public void AnimateJump(){

         startTimer = true;


     if(timer >= frameTime){

            timer -= frameTime;
            spriteFrame = (spriteFrame + 1) % jumpSprite.Length;
            unitSpriteRenderer.sprite = jumpSprite[spriteFrame];
            unitDir = 3;
        }
    }
   
   
   
    // Update is called once per frame
    void Update()
    {
        
        if(startTimer){
            timer += Time.deltaTime;
        }
        else{
            timer = 0;
        }
        if(timer > 20){
            timer = 0;
        }

    }
    //call funcs for each anim
}
