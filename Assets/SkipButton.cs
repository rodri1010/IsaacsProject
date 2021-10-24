using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SkipButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SkipLevel()
    {
      StartButton.levelNumber += 1;
      Log(SceneManager.GetActiveScene().name,StartButton.xmlNames[SceneManager.GetActiveScene().name],"Skipped");
      SceneManager.LoadScene(StartButton.levels[SceneManager.GetActiveScene().name]);
    }

    private void Log(params object[] args)
    {
        string line = string.Join(", ", args);
        line = DateTime.Now.ToString("ddd MMM dd yyyy HH:mm:ss") + ", " + line;
        Debug.Log(line);

        if (StartButton.logWriter != null)
        {
            StartButton.logWriter.WriteLine(line);
            StartButton.logWriter.Flush();
        }
    }

    private void CloseLog()
    {
        if (StartButton.logWriter != null)
        {
            StartButton.logWriter.Flush();
            StartButton.logWriter.Close();
            StartButton.logWriter = null;
        }
    }
}
