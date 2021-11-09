using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SetUpScene : MonoBehaviour
{
    public Text changingText;
    public Text hintText;
    public Button shapeButton;
    public Button colorButton;
    public Button locationButton;
    public Button contextButton;
    public Dictionary<string, string> info;
    float time = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
      var path = Application.streamingAssetsPath;
      info = StartButton.LoadXml(path + "/" + StartButton.xmlNames[SceneManager.GetActiveScene().name]);
      shapeButton.onClick.AddListener(ShapeHint);
      colorButton.onClick.AddListener(ColorHint);
      locationButton.onClick.AddListener(LocationHint);
      contextButton.onClick.AddListener(ContextHint);
      TextChange();
    }

    void Update()
    {
      if(time > 0)
      {
        time -= Time.deltaTime;
      }
      else
      {
        hintText.text = "";
      }

    }

    public void TextChange()
    {
      changingText.text = info["description"];
    }

    public void ShapeHint()
    {
      time = 8.0f;
      hintText.text = info["shape"];
    }

    public void ColorHint()
    {
      time = 8.0f;
      hintText.text = info["color"];
    }

    public void ContextHint()
    {
      time = 8.0f;
      hintText.text = info["context"];
    }

    public void LocationHint()
    {
      time = 8.0f;
      hintText.text = info["location"];
    }
}
