using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemName {
    Whistle,
    Item2
}
public class Item{
    public ItemName itemName;
    private int itemNum = 0;

    private Sprite sprite;

    private AudioSource sound;

    public Item(ItemName name, Sprite newSpr, AudioSource newSound){
        this.itemName = name;
        this.sprite = newSpr;
        this.sound = newSound;
    }

    public int getItemNum(){
        return itemNum;
    }

    public Sprite getItemSprite(){
        return sprite;
    }

    public void addItem(){
        this.itemNum ++;
    }

    public AudioSource getSound(){
        return sound;
    }

    public void useItem(){
        this.itemNum --;
        if(this.itemName == ItemName.Whistle){
            // lure the closest enemy
            EnemyAIFighting temp = EnemyManager.skList[0];
            float distance = 100;
            foreach (var item in EnemyManager.skList)
            {
                if(Vector3.Distance(item.transform.position, PlayerMovement.instance.transform.position) < distance)
                {
                    temp = item;
                    distance = Vector3.Distance(temp.transform.position, PlayerMovement.instance.transform.position);
                }               
            }
            temp.SwitchState(EnemyAIFighting.SkeletionState.Fight);
        }
    }
}

public class ItemScript : MonoBehaviour
{
    public ItemName itemName;
    public Sprite itemSprite;
    public AudioSource sound;

    // void Start()
    // {
    //                 sound.Play();
    // }

    // when collected by player, the item is added to the inventory and destroyed from scene.
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            Item newItem = new Item(this.itemName, this.itemSprite, this.sound);
            PlayerItem.instance.addItem(newItem);
            Destroy(this.gameObject);
        }
    }
}
