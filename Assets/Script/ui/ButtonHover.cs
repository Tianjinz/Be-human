using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Button restartButton;
    // speed of expansion
    public float expandSpeedPerSec;

    // maximum expansion of the button
    public float maxExpand;

    private float currExpansion;

    // shows if the button is expanding
    private bool expanding;

    // indicates if the pointer is currently on the button
    private bool pointerOn;

    // button image
    public Image image;

    public Color pointerOnColor;

    public Color pointerOffColor;

    private Vector3 defaultScale = new Vector3(1f, 1f, 1f);
    // Start is called before the first frame update
    void Start()
    {
        currExpansion = 1;
        expanding = true;
        restartButton.onClick.AddListener(restart);
    }

    void Update()
    {
        // button shrinks and expand when pointer not on
        if(!pointerOn){
            if(expanding){
                if(currExpansion < maxExpand){
                    currExpansion += expandSpeedPerSec * Time.deltaTime;
                    transform.localScale = defaultScale * currExpansion;   
                }else{
                    expanding = false;
                }
            }else{
                if(currExpansion > 1){
                    currExpansion -= expandSpeedPerSec * Time.deltaTime;  
                    transform.localScale = defaultScale * currExpansion;                    
                }else{
                    expanding = true;
                }                
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        pointerOn = true;
        image.color = pointerOnColor;

    }

     
    public void OnPointerExit(PointerEventData eventData)
    {
        pointerOn = false;
        image.color = pointerOffColor;
    }

    private void restart(){
        GameManager.instance.Resume();
        InGameUIManager.instance.deathScreen.SetActive(false);
        GameManager.instance.setInGameState(true);
    }

}
