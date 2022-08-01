using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    public static PlayerItem instance;
    private List<Item> itemList = new List<Item>();

    public int currentItemNum = 0;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(itemList.Count > 0){

            displaySelectedItem();
            // check if the player has changed the item selected
            if(Input.GetAxis("Mouse ScrollWheel") != 0f){
                int previousItem = currentItemNum;

                // if the player has scrolled up, select last item
                if(Input.GetAxis("Mouse ScrollWheel") > 0f){
                    if(currentItemNum >= (itemList.Count - 1)){
                        currentItemNum = 0;
                    }else{
                        currentItemNum ++;
                    }
                }

                // if the player has scrolled down, select next item
                if(Input.GetAxis("Mouse ScrollWheel") < 0f){
                    if(currentItemNum <= 0){
                        currentItemNum = itemList.Count - 1;
                    }else{
                        currentItemNum --;
                    }
                }

                if(previousItem != currentItemNum){
                    displaySelectedItem();
                }
            }

            // used the current item(by pressing key F)
            KeyCode keyF = KeyCode.F;
            if(Input.GetKeyUp(keyF)){
                useCurrentItem();
                print("used");
            }
        }
    }

    public void addItem(Item newItem){
        ItemName name = newItem.itemName;
        print(name);

        // check if identical item exist in inventory
        for(int i = 0; i < itemList.Count; i++){
            if(itemList[i].itemName == name){
                itemList[i].addItem();
                return;
            }
        }

        // no identical item in inventory
        
        newItem.addItem();
        itemList.Add(newItem);

    }

    public void useCurrentItem(){
        Item currentItem = itemList[currentItemNum];
        currentItem.useItem();

        // play sound effect on player
        AudioSource sound = currentItem.getSound();
        sound.Play();
        print("sound");

        // remove the used item if there is none left
        if(currentItem.getItemNum() <= 0){
            itemList.Remove(currentItem);
        }

        // ensure the currentItem is not out of bound
        if((itemList.Count - 1) < currentItemNum){
            currentItemNum = 0;
        }

        // update item in ui
        displaySelectedItem();
    }

    // select the current item on ui
    private void displaySelectedItem(){
        if(itemList.Count <= 0){
            InGameUIManager.instance.emptyItemUI();
        }else{
            InGameUIManager.instance.displayItem(itemList[currentItemNum]);            
        }

    }
}
