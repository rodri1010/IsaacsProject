using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoZController : MonoBehaviour
{

    public GameManager GameManager;
    public GameObject TheGameController;
    // Start is called before the first frame update
    void Start()
    {
        TheGameController = GameObject.Find("GameManager");
        GameManager = TheGameController.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))       GameManager.SendIntent("context");
        else if (Input.GetKeyDown(KeyCode.Alpha2))  GameManager.SendIntent("color");
        else if (Input.GetKeyDown(KeyCode.Alpha3))  GameManager.SendIntent("shape");
        else if (Input.GetKeyDown(KeyCode.Alpha4))  GameManager.SendIntent("location");
        else if (Input.GetKeyDown(KeyCode.Alpha5))  GameManager.SendIntent("object");
        else if (Input.GetKeyDown(KeyCode.Q))       GameManager.SendIntent("greeting");
        else if (Input.GetKeyDown(KeyCode.W))       GameManager.SendIntent("thanks");
        else if (Input.GetKeyDown(KeyCode.E))       GameManager.SendIntent("hint");
        else if (Input.GetKeyDown(KeyCode.R))       GameManager.SendIntent("help");
        else if (Input.GetKeyDown(KeyCode.T))       GameManager.SendIntent("unknown");
        //else if (Input.GetKeyDown(KeyCode.A))       agentController.SendIntent("start");
        //else if (Input.GetKeyDown(KeyCode.S))       agentController.SendIntent("positive");
        //else if (Input.GetKeyDown(KeyCode.D))       agentController.SendIntent("negative");
        //else if (Input.GetKeyDown(KeyCode.F))       agentController.SendIntent("end");
        else if (Input.GetKeyDown(KeyCode.G))       GameManager.SendIntent("ready");
        //else if (Input.GetKeyDown(KeyCode.Space))   GameManager.TriggerAgentReady();
    }
}
