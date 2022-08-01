using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIScript : MonoBehaviour
{
    public Image image;

    public static ItemUIScript instance;

    void Start()
    {
        emptyItemUI();
        ItemUIScript.instance = this;
    }

    // display item on ui
    public void displayItem(Item item){
        // change the sprite displayed
        Sprite displaySprite = item.getItemSprite();
        image.sprite = displaySprite;
        image.color = Color.white;

        // change the quantity displayed
        Text text = GetComponentInChildren<Text>();
        text.text = item.getItemNum().ToString();
    }

    // empty the ui used to display item
    public void emptyItemUI(){
        // item ui is empty
        image = this.GetComponent<Image>();
        image.color = Color.clear;

        Text text = GetComponentInChildren<Text>();
        text.text = "";
    }
}
