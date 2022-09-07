using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrepScene : MonoBehaviour
{
    public GameObject pidInput;
    public GameObject groupInput;
    public GameObject genderInput;
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
        string pid = pidInput.GetComponent<TMP_InputField>().text;
        int group = groupInput.GetComponent<TMP_Dropdown>().value;
        int gender = genderInput.GetComponent<TMP_Dropdown>().value;
        if(!string.IsNullOrEmpty(pid))
        {
            GameManager.InitGame(pid,group,gender);
        }
    }
}
