using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public static Dictionary<string, string> xmlNames;
    public static string[] levels;
    public static int levelNumber = 0;
    public static StreamWriter logWriter = null;
    public string rootLogPath = "./";
    public string logFilename = "";

    // Start is called before the first frame update
    void Start()
    {
      xmlNames = new Dictionary<string,string>();
      levels = new string[] { "Level1","Level2","Level3","Level4","Level5","Level8","Level10","Level11","Level12",
                                      "Level13","Level14","Level15","Level16","Level17","Level18",
                                      "Level19","Level20","Level22"};
      initializeDictionary();
      InitLog();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RegisterClick()
    {
      SceneManager.LoadScene(levels[0]);
    }

    public void initializeDictionary()
    {
      xmlNames.Add("Level1","ispy1.xml");
      xmlNames.Add("Level2","ispy2.xml");
      xmlNames.Add("Level3","ispy3.xml");
      xmlNames.Add("Level4","ispy4.xml");
      xmlNames.Add("Level5","ispy5.xml");
      xmlNames.Add("Level6","ispy6.xml");
      xmlNames.Add("Level7","ispy7.xml");
      xmlNames.Add("Level8","ispy8.xml");
      xmlNames.Add("Level9","ispy9.xml");
      xmlNames.Add("Level10","ispy10.xml");
      xmlNames.Add("Level11","ispy11.xml");
      xmlNames.Add("Level12","ispy12.xml");
      xmlNames.Add("Level13","ispy13.xml");
      xmlNames.Add("Level14","mystery1.xml");
      xmlNames.Add("Level15","mystery2.xml");
      xmlNames.Add("Level16","mystery3.xml");
      xmlNames.Add("Level17","mystery4.xml");
      xmlNames.Add("Level18","mystery5.xml");
      xmlNames.Add("Level19","mystery6.xml");
      xmlNames.Add("Level20","mystery8.xml");
      xmlNames.Add("Level21","mystery9.xml");
      xmlNames.Add("Level22","mystery11.xml");
      xmlNames.Add("Level23","mystery12.xml");



      Shuffle();

    }

    public void Shuffle()
    {
         for (int i = 0; i < levels.Length; i++) {
             int rnd = UnityEngine.Random.Range(0, levels.Length);
             while(i == rnd){
               rnd = UnityEngine.Random.Range(0, levels.Length);
             }
             var tempGO = levels[rnd];
             levels[rnd] = levels[i];
             levels[i] = tempGO;
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

      return hints;

    }

    private void InitLog()
    {
        if(logWriter == null)
        {
            logFilename = "IssacsProject_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
            string logPath = Path.Combine(rootLogPath, logFilename);
            logWriter = new StreamWriter(logPath);

            // Write headers
            logWriter.WriteLine("date, level, image, time");
        }
    }
}
