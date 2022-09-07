using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class LevelScene : MonoBehaviour
{

    public GameManager GameManager;
    public GameObject TheGameController;
    public UdpSocket Client;
    public GameObject TheClientController;
    public static Dictionary<string, string> puzzles;
    public Image image;
    public Image imageKey;
    public Sprite curImage;
    public GameObject title;
    public GameObject level;
    public GameObject timeText;
    public GameObject panel;
    public Button skipButton;
    public Dictionary<string, string> info;
    public string[] levels;
    public float skipTimeThreshold = 120;
    public float skipBlinkPeriod = 1f;

    private float curLevelTime = 0;
    private bool timerActive = true;
    public TMP_Text skipText;


    // Start is called before the first frame update
    void Start()
    {
        levels = new string[] {"mystery2","ispy3","mystery3","ispy2","mystery5",
                                "mystery11","ispy4","ispy1","ispy9","ispy6",
                                "mystery9","mystery12","ispy11","mystery6","ispy8",
                                "mystery4","ispy5","ispy13","ispy10","ispy12"};

        TheClientController = GameObject.Find("Client");
        Client = TheClientController.GetComponent<UdpSocket>();
        var path = Application.streamingAssetsPath;
        info = GameManager.LoadXml(path + "/" + levels[0] + ".xml");
        title.GetComponent<TMP_Text>().text = info["description"];
        curImage = Resources.Load<Sprite>("images/" + levels[0]);
        image.GetComponent<Image>().sprite = curImage;
        imageKey.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/keys/" + levels[0]);
        TheGameController = GameObject.Find("GameManager");
        GameManager = TheGameController.GetComponent<GameManager>();
        level.GetComponent<TMP_Text>().text = "Level 1" ;
        skipButton.onClick.AddListener(SkipLevel);
        panel.SetActive(true);
        timerActive = false;
        curLevelTime = 0;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
            curLevelTime += Time.deltaTime;

            // Check if need to blink the skip button
            if (curLevelTime > skipTimeThreshold)
            {
                skipText.fontStyle = FontStyles.Bold;
                float skipTime = curLevelTime - skipTimeThreshold;
                float skipValue = skipTime % skipBlinkPeriod;
                skipText.color = (skipValue > skipBlinkPeriod / 2) ? Color.black : Color.red;
            }
            else
            {
                skipText.fontStyle = FontStyles.Normal;
                skipText.color = Color.black;
            }
        }

        // Update timer text
        TimeSpan curTime = TimeSpan.FromSeconds(curLevelTime);
        timeText.GetComponent<TMP_Text>().text = $"{curTime.Minutes}:{curTime.Seconds:D2}";

    }

    public void CorrectClick(bool skippedLevel = false)
    {
        if(GameManager.level < 20)
        {
            if(skippedLevel)
            {   
                GameManager.SendIntent("negative");
            }
            else
            {
                GameManager.SendIntent("correct,positive");
            }
            
            GameManager.level = GameManager.level + 1;
            imageKey.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/keys/" + levels[GameManager.level]);
            image.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/" + levels[GameManager.level]);
            level.GetComponent<TMP_Text>().text = "Level " + (GameManager.level + 1) ;
            var path = Application.streamingAssetsPath;
            info = GameManager.LoadXml(path + "/" + levels[GameManager.level] + ".xml");
            title.GetComponent<TMP_Text>().text = info["description"];
            curLevelTime = 0f;

            if(GameManager.level % 5 != 0 && GameManager.level != 20)
            {
                GameManager.SendIntent("start");
            }
            panel.SetActive(true);
            timerActive = false;
            curLevelTime = 0;

            

            

        }
        
        if(GameManager.level % 5 == 0 && GameManager.level != 20)
        {
            GameManager.TransitionToTransitionScene();
        }
        else if(GameManager.level == 19)
        {
            GameManager.TransitionToEndScene();
        }

        // Send info to server or to log file.

    }

    public void WrongClick()
    {
        GameManager.SendIntent("incorrect");
    }

    private void SkipLevel()
    {
        CorrectClick(true);
    }

    public void StartLevel()
    {
        panel.SetActive(false);
        timerActive = true;
    }
}
