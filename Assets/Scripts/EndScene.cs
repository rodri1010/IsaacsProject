using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{


    public GameManager GameManager;
    public GameObject TheGameController;
    public Button endButton;

    // Start is called before the first frame update
    void Start()
    {
        TheGameController = GameObject.Find("GameManager");
        GameManager = TheGameController.GetComponent<GameManager>();
        endButton.onClick.AddListener(FinishGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FinishGame()
    {
        Application.Quit();
    }
}
