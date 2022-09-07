using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionScene : MonoBehaviour
{


    public GameManager GameManager;
    public GameObject TheGameController;
    public Button continueButton;

    // Start is called before the first frame update
    void Start()
    {
        TheGameController = GameObject.Find("GameManager");
        GameManager = TheGameController.GetComponent<GameManager>();
        continueButton.onClick.AddListener(BackToLevels);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackToLevels()
    {
        GameManager.FromTransitionToLevel();
    }
}
