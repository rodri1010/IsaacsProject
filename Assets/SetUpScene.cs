using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SetUpScene : MonoBehaviour
{
    public Text changingText;
    public Dictionary<string, string> info;
    // Start is called before the first frame update
    void Start()
    {
      var path = "Assets/puzzles/";
      info = StartButton.LoadXml(path + StartButton.xmlNames[SceneManager.GetActiveScene().name]);
      TextChange();
    }

    public void TextChange()
    {
      changingText.text = info["description"];
    }
}
