using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuitButton : MonoBehaviour
{

    public Button myButton;

    // Start is called before the first frame update
    void Start()
    {
      myButton.onClick.AddListener(ExitApp);

    }

    void Update()
    {

    }

    void ExitApp()
    {
        Application.Quit();
    }
}
