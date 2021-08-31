//GTG Emistro (C) 2021
//CODE CAN ONLY BE USED WITH PERMISSION FROM eavpsp@gmail.com
//NOT FOR REDISTRIBUTION
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blocks : MonoBehaviour
{
    public bool isLocked;
    public bool letDrop;
    public Text textMade;
    public string blockLetter;
    public bool isSelected;
    public bool deleteBlock;
    public Rigidbody2D rig;
    public Vector3 blockLocation;
    public bool letFall;
    public SpriteRenderer image;
    public bool canBeSelected;
    // Start is called before the first frame update
    void Start()
    {
        letFall = true;
        canBeSelected = true;
        image = gameObject.GetComponentInChildren<SpriteRenderer>();
        blockLocation = transform.position;
        rig = gameObject.GetComponent<Rigidbody2D>();
        BlockController.instance.blocksInGame.Add(this);
        textMade = BlockController.instance.textMadeUI;
        blockLetter = this.gameObject.GetComponentInChildren<Text>().text;
    }
    public void BlockClicked(){
        //clicked
        if(!isSelected && canBeSelected){
        textMade.text += blockLetter;
        image.color = Color.red;
        canBeSelected = false;
        BlockController.instance.selectedBlocks.Add(this);
        isSelected = true;
        }
        //unclicked
        else if(isSelected){
            BlockController.instance.selectedBlocks.Remove(this);
            BlockController.instance.ResetSelectedBlocks();
            BlockController.instance.ResetBlocksInGame();
            textMade.text = "";
            image.color = Color.white;
            canBeSelected = true;
            isSelected = false;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        //stop when we hit a block
        if (collision.gameObject.GetComponent<Blocks>() != null && collision.gameObject.GetComponent<Blocks>().letFall == false)
        {
            collision.gameObject.GetComponent<Blocks>().letFall = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(canBeSelected && !isSelected){
            image.color = Color.white;
        }
        if(!canBeSelected && !isSelected){
            image.color = Color.green;
        }
        //dont fall if at the bottom
        if(transform.position.y <= 0){
            transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0);
            letFall = false;
        }
        //trigger if the block should fall
        if(letFall){
            rig.isKinematic = false;
            
        }else {
            rig.isKinematic = true;
            transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0);
        }
        if(deleteBlock){
            Destroy(gameObject, 0);
        }
    }
}
