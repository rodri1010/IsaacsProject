using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public bool transitionSceneOn;
    public int level;
    public string[] levels;
    public int section;
    public int gender;  // 0: Male, 1: Female;
    public int group;
    public string pid;
    public static StreamWriter logWriter = null;
    public string rootLogPath = "./";
    public string logFilename = "";
    public UdpSocket TheScript;
    public GameObject TheGameController;
    public Canvas prepScene;
    public Canvas startScene;
    public Canvas practiceScene;
    public Canvas levelScene;
    public Canvas transitionScene;
    public Canvas endScene;
    public GameObject panel;
    public Queue<string> messagesQueue = new Queue<string>();
    public float timeBetweenMessages = 0.2f;
    public bool deactivatePanel = false;
    
    public LevelScene LevelManager;
    public GameObject TheLevelController;

    // Start is called before the first frame update
    void Start()
    {
        levels = new string[] {"mystery2","ispy3","mystery3","ispy2","mystery5",
                                "mystery11","ispy4","ispy1","ispy9","ispy6",
                                "mystery1","mystery12","ispy11","mystery6","ispy8",
                                "mystery8","ispy5","ispy13","mystery4","ispy12"};
        transitionSceneOn = true;
        TheGameController = GameObject.Find("Client");
        TheScript = TheGameController.GetComponent<UdpSocket>();
        prepScene.gameObject.SetActive(true);
        startScene.gameObject.SetActive(false);
        practiceScene.gameObject.SetActive(false);
        levelScene.gameObject.SetActive(false);
        transitionScene.gameObject.SetActive(false);
        endScene.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        timeBetweenMessages -= Time.deltaTime;
        if(deactivatePanel)
        {
            TheLevelController = GameObject.Find("LevelScene");
            LevelManager = TheLevelController.GetComponent<LevelScene>();
            LevelManager.StartLevel();
            deactivatePanel = false;
        }
        if(messagesQueue.Count != 0 && timeBetweenMessages <= 0)
        {
            string tempString = messagesQueue.Dequeue();
            timeBetweenMessages = 0.2f;
            TheScript.SendData(tempString);
        }

    }


    public void InitGame(string pid, int group, int gender){
        this.level = 0;
        this.group = group;
        this.gender = gender;
        this.pid = pid;
        AddMessage("{\"type\":\"updateGame\", \"pid\":\"" + pid + "\",  \"level\":" + level +", \"group\":" + group + ", \"gender\":" + gender + "}");
        // TheScript.SendData("{\"type\":\"updateGame\", \"pid\":\"" + pid + "\",  \"level\":" + level +", \"group\":" + group + ", \"gender\":" + gender + "}");
        prepScene.gameObject.SetActive(false);
        startScene.gameObject.SetActive(true);
        InitLog();
    }

    public void AddMessage(string message)
    {
        messagesQueue.Enqueue(message);
    }

    public void StartGame()
    {
        startScene.gameObject.SetActive(false);
        practiceScene.gameObject.SetActive(true);
        //Send message to server that game has started with time and other information. MAYBE since first scene is practice.
    }

    public void TransitionToGame()
    {
        practiceScene.gameObject.SetActive(false);
        levelScene.gameObject.SetActive(true);
        this.transitionSceneOn = false;
        AddMessage("{\"type\":\"updateGame\", \"pid\":\"" + this.pid + "\",  \"level\":" + this.level +", \"group\":" + this.group + ", \"gender\":" + this.gender + "}");
        //TheScript.SendData("{\"type\":\"updateGame\", \"pid\":\"" + this.pid + "\",  \"level\":" + this.level +", \"group\":" + this.group + ", \"gender\":" + this.gender + "}");
        SendIntent("greeting");
        SendIntent("start");
        //Send message to server
    }

    public void TransitionToTransitionScene()
    {
        levelScene.gameObject.SetActive(false);
        transitionScene.gameObject.SetActive(true);

        // Probably send message or log here
    }


    public void FromTransitionToLevel()
    {
        AddMessage("{\"type\":\"updateGame\", \"pid\":\"" + pid + "\",  \"level\":" + level +", \"group\":" + group + ", \"gender\":" + gender + "}");
        //TheScript.SendData("{\"type\":\"updateGame\", \"pid\":\"" + pid + "\",  \"level\":" + level +", \"group\":" + group + ", \"gender\":" + gender + "}");

        if(this.level < 20)
        {
            levelScene.gameObject.SetActive(true);
            transitionScene.gameObject.SetActive(false);
        }
        SendIntent("greeting");
        SendIntent("start");
        
        // Probably send message or log here

    }

    public void TransitionToEndScene()
    {
        endScene.gameObject.SetActive(true);
        levelScene.gameObject.SetActive(false);
    }

    public string GetFirstIntent(string intents)
    {
        return intents.Split(',')[0];

    }

    public void SendIntent(string intent)
    {
        var path = Application.streamingAssetsPath;
        Dictionary<string, string> info = LoadXml(path + "/" + this.levels[this.level] + ".xml");
        string message = "";
        string otherIntent = GetFirstIntent(intent);
        switch (otherIntent)
        {
            case "context":
            case "color":
            case "shape":
            case "location":
                message = info[otherIntent];
                break;
            default:
                message = "";
                break;
        }
        AddMessage("{\"type\":\"updateAgent\", \"intent\":\"" + intent + "\", \"message\":\"" + message + "\", \"name\":\"" + info["name"] + "\"}");
        // TheScript.SendData("{\"type\":\"updateAgent\", \"intent\":\"" + intent + "\", \"message\":\"" + message + "\", \"name\":\"" + info["name"] + "\"}");
    }


    public void ReceiveMessage(string message)
    {
        if(message == "finish")
        {
            deactivatePanel = true;
        }
        
    }


    //Group 0: Female1, Female2, Female3, Female4
    //Group 1: Female2, Female4, Female1, Female3
    //Group 2: Female4, Female3, Female2, Female1
    //Group 3: Female3, Female1, Female4, Female2

    // female1;   Looking at small
    // female2;   Looking with small
    // female3;   Looking with sitting
    // female4;   Looking with standing

    public string GetCurAgentName()
    {
        if(this.group == 0)
        {
            string[] agents = this.gender == 1? new string[] {"LookingAtFemale","LookingWithSmallFemale","LookingWithStandingFemale", "LookingWithSittingFemale"} : new string[] {"LookingAtMale","LookingWithSmallMale","LookingWithStandingMale","LookingWithSittingMale"};
            return agents[this.level/5];
        }
        else if(this.group == 1)
        {
            string[] agents = this.gender == 1? new string[] {"LookingWithSmallFemale","LookingWithSittingFemale","LookingAtFemale","LookingWithStandingFemale"} : new string[] {"LookingWithSmallMale","LookingWithSittingMale","LookingAtMale","LookingWithStandingMale"};
            return agents[this.level/5];

        }
        else if(this.group == 2)
        {
            string[] agents = this.gender == 1? new string[] {"LookingWithSittingFemale","LookingWithStandingFemale","LookingWithSmallFemale","LookingAtFemale"} : new string[] {"LookingWithSittingMale","LookingWithStandingMale","LookingWithSmallMale","LookingAtMale"};
            return agents[this.level/5];

        }
        else if(this.group == 3)
        {
            string[] agents = this.gender == 1? new string[] {"LookingWithStandingFemale","LookingAtFemale","LookingWithSittingFemale","LookingWithSmallFemale"} : new string[] {"LookingWithStandingMale","LookingAtMale","LookingWithSittingMale","LookingWithSmallMale"};
            return agents[this.level/5];
        }
        else
        {
            return "Not found";
        }
    }

    public void InitLog()
    {
        if(logWriter == null)
        {
            logFilename = "SideBySideAgents_PID_" + this.pid.ToString() + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
            string logPath = Path.Combine(rootLogPath, logFilename);
            logWriter = new StreamWriter(logPath);

            // Write headers
            logWriter.WriteLine("date, level, image, agent, timeToComplete, skipped");
        }
    }

    public void Log(params object[] args)
     {
         string line = string.Join(", ", args);
         line = DateTime.Now.ToString("ddd MMM dd yyyy HH:mm:ss") + ", " + line;

         if (logWriter != null)
         {
             logWriter.WriteLine(line);
             logWriter.Flush();
         }
     }

     public void CloseLog()
     {
         if (logWriter != null)
         {
             logWriter.Flush();
             logWriter.Close();
             logWriter = null;
         }
     }


    public static Dictionary<string, string> LoadXml(string path)
    {
      string name = Path.GetFileNameWithoutExtension(path);

      // Load xml document
      XmlDocument doc = new XmlDocument();
      doc.Load(path);

      // Get attributes
      string description = doc.SelectSingleNode("puzzle/description/text()").Value;
      string objectName = doc.SelectSingleNode("puzzle/objectName/text()").Value;
      // Get hints
      XmlNodeList hintNodes = doc.SelectNodes("puzzle/hints/hint");
      Dictionary<string, string> hints = new Dictionary<string, string>();
      foreach (XmlNode node in hintNodes)
      {
          string hintType = node.Attributes.GetNamedItem("type").Value.ToLower();
          string hintText = node.SelectSingleNode("text()").Value;
          hints[hintType] = hintText;
      }
      hints["description"] = description;
      hints["name"] = objectName;

      return hints;

    }
}
