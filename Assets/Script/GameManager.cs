using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]

// custom event that passes a string as parameter
public class MyStringEvent : UnityEvent<string>
{
}

// custom event that is used for items
// passes the name of the item used and position of player when item used.
public class MyItemEvent : UnityEvent<ItemName, Vector3>
{
}

// manages all events in this game
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool inGame;

    // events that will be subscribed to by classes
    public UnityEvent gameStarted = new UnityEvent();
    public UnityEvent gameLost = new UnityEvent();
    public MyStringEvent toNextLevel = new MyStringEvent();
    public MyItemEvent itemUsed = new MyItemEvent();

    public bool GamePaused;


    private void Awake()
    {
        instance = this;
        GamePaused = false;

        setInGameState(true);
        Resume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        gameStarted?.Invoke();
    }

    public void LoseGame()
    {
        gameLost?.Invoke();
    }

    public void NextLevel(string nextLevel)
    {
        toNextLevel?.Invoke(nextLevel);
    }

    public void useItem(ItemName itemName, Vector3 playerPos){
        itemUsed?.Invoke(itemName, playerPos);
    }

    
    public void Resume()
    {
        Time.timeScale = 1f;
        GamePaused = false;
        setInGameState(true);
        // Cursor.visible = false;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        GamePaused = true;
        setInGameState(false);
        // Cursor.visible = true;
    }

    // change if is in game
    public void setInGameState(bool ifInGame){
        if(ifInGame){
            inGame = true;
            lockCursor();
        }else{
            inGame = false;
            unlockCursor();
        }
        
    }

    public void lockCursor(){
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void unlockCursor(){
        Cursor.lockState = CursorLockMode.None;
    }

}
