//GTG Emistro (C) 2021
//CODE CAN ONLY BE USED WITH PERMISSION FROM eavpsp@gmail.com
//NOT FOR REDISTRIBUTION
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class BlockController : MonoBehaviour
{
    public string[] letter;
    public string wordMade;
    public int gridSize;
    public GameObject blockToSpawn;
    public bool blocksGenerated;
    public bool updateBlock;
    public Vector3 blockLocation;
    public List<Blocks> newBlocks = new List<Blocks>();
    public List<Blocks> selectedBlocks = new List<Blocks>();
    public List<Blocks> blocksInGame = new List<Blocks>();
    public int blockChoice;
    public Text textMadeUI;
    public static BlockController instance;
    public Blocks currentBlock;
    public TextAsset listOfWords;
    private string fileAsString;
    public string[] wordToMake;


    void Awake ()
    {
        // set the instance to this script
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        fileAsString = listOfWords.text;
        //line below adds each word to the string array of words to make
        wordToMake = fileAsString.Split(char.Parse("\n"));
                
    }
    public void GenerateBlock(float x){
                        //loads the block above the grid in the same row
                        int y = gridSize + 1;
                        blockLocation = new Vector3(x,y,0f);
                        //sets the block letter
                        blockChoice = Random.Range(0,26);
                        blockToSpawn.gameObject.GetComponentInChildren<Text>().text = letter[blockChoice];
                       
                        //selects the proper prefab based on the index of the prefab in the array in the proper location
                        GameObject block = Instantiate(blockToSpawn, blockLocation, Quaternion.identity);
                        block.transform.parent = this.transform;
                         //enable gravity on new loaded block
                        block.gameObject.GetComponent<Blocks>().letFall = true;

    }
    public void ResetSelectedBlocks(){
        for (int p = 0; p < selectedBlocks.Count; p++)
        {
            selectedBlocks[p].image.color = Color.white;
            selectedBlocks[p].isSelected = false;
            selectedBlocks[p].canBeSelected = true;
            BlockController.instance.selectedBlocks.Remove(selectedBlocks[p]);
        }
      
    }
    public void ResetBlocksInGame(){
         for (int m = 0; m < blocksInGame.Count; m++)
        {
            textMadeUI.text = "";
            blocksInGame[m].image.color = Color.white;
            blocksInGame[m].canBeSelected = true;
            blocksInGame[m].isSelected = false;
            
        }

    }
    public void SubmitWord(){
        //loop through string of words
        
            //if the word you made is equal to any of the strings in the array of words
            wordMade = textMadeUI.text;
            
        if(wordMade != ""){
            foreach (string word in wordToMake)
            {   
                if(word.ToString() == wordMade){
                    Debug.Log("Made Word");
                    //loop through selected blocks
                    while(selectedBlocks.Count > 0){
                        for (int j = 0; j < selectedBlocks.Count; j++)
                        {
                            //get their positoion to spawn a tile in that same x(column) postion
                            StartCoroutine(UpdateBlocks());
                            IEnumerator UpdateBlocks(){
                                ResetBlocksInGame();
                                currentBlock = selectedBlocks[j];
                                selectedBlocks.Remove(currentBlock);
                                blocksInGame.Remove(currentBlock);
                                blockLocation = currentBlock.transform.position;
                                GenerateBlock(currentBlock.transform.position.x);
                                currentBlock.deleteBlock = true;
                                //loop through blocks in that row and make them fall
                                for (int z = 0; z < blocksInGame.Count; z++)
                                {
                                    //check the row
                                    if(blocksInGame[z].blockLocation.x == blockLocation.x && blocksInGame[z].blockLocation.y != 0){
                                        //let blocks drop
                                        blocksInGame[z].letFall = true;
                                        
                                    }
                                }
                                yield return new WaitForSeconds(10);
                                
                            }
                            
                        }
                        textMadeUI.text = "";
                    }
                }
                
            }
        }
    }
    // Update is called once per frame
    void Update(){       

            SubmitWord();
        
        if(selectedBlocks.Count >= 1){
            //set which blocks can be selected
                for (int f = 0; f < blocksInGame.Count; f++)
                {
                    Blocks nextBlock = blocksInGame.Find(x => x.transform.position ==  blocksInGame[f].transform.position);
                    if(Vector3.Distance(selectedBlocks[selectedBlocks.Count - 1].transform.position, nextBlock.transform.position) <= 1.5){
                        nextBlock.canBeSelected = true;
                    }
                    else {
                        nextBlock.canBeSelected = false;
                    }
                }
                
        }
                
            
        
        
       
    
        if(!blocksGenerated){
            int[,] blocks = new int[gridSize, gridSize];
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    //decides what letter the block will be
                    blockChoice = Random.Range(0,26);
                    blocks[x,y] = blockChoice;
                    //if the block at that location is equal to the value assigned for the letter, spawn that letter in that location
                    if(blocks[x,y] == blockChoice){
                        //spawn tile corresponding to the letter chose in that position
                        blockLocation = new Vector3(x,y,0f);
                        //sets the block letter
                        
                        blockToSpawn.gameObject.GetComponentInChildren<Text>().text = letter[blockChoice];
                        //selects the proper prefab based on the index of the prefab in the array in the proper location
                        GameObject block = Instantiate(blockToSpawn, blockLocation, Quaternion.identity);
                        block.transform.parent = this.transform;
                    }

                     
                }
            }
            blocksGenerated = true;
        }
    }
}
