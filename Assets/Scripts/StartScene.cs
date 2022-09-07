using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StartScene : MonoBehaviour
{
    public Button startButton;
    public GameManager GameManager;
    public GameObject TheGameController;
    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        TheGameController = GameObject.Find("GameManager");
        GameManager = TheGameController.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        GameManager.StartGame();
    }

}
