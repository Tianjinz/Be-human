using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyUIScript : MonoBehaviour
{
    public Image image;

    public static KeyUIScript instance;

    public Text numKeyPos;

    public Text numKeyLeft;

    void Start()
    {
        changeQuantity(0);
        KeyUIScript.instance = this;
    }

    // display key on ui
    public void changeQuantity(int quantity)
    {

        // change the quantity displayed
        numKeyPos.text = quantity.ToString();

        // change the num key left displayed
        numKeyLeft.text = "Number of keys left : " + (exitScript.ogKeyAmount - quantity).ToString();
    }

    // // empty the ui used to display item
    // public void emptyItemUI(){
    //     // item ui is empty
    //     image = this.GetComponent<Image>();
    //     image.color = Color.clear;

    //     Text text = GetComponentInChildren<Text>();
    //     text.text = "";
    // }
}
