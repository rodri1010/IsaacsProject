using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public static Dictionary<string, string> levels;
    public static Dictionary<string, string> xmlNames;
    public string[] levelsUnordered;
    public string[] levelsOrdered;
    public static int levelNumber = 0;
    // Start is called before the first frame update
    void Start()
    {
      levels = new Dictionary<string, string>();
      xmlNames = new Dictionary<string,string>();
      levelsUnordered = new string[] { "Level1","Level2","Level3","Level4","Level5","Level6",
                                      "Level7","Level8","Level9","Level10","Level11","Level12",
                                      "Level13","Level14","Level15","Level16","Level17","Level18",
                                      "Level19","Level20","Level21","Level22","Level23"};
      levelsOrdered = new string[] { "Level1","Level2","Level3","Level4","Level5","Level6",
                                      "Level7","Level8","Level9","Level10","Level11","Level12",
                                      "Level13","Level14","Level15","Level16","Level17","Level18",
                                      "Level19","Level20","Level21","Level22","Level23"};
      initializeDictionary();
      LoadXml("Assets/puzzles/ispy1.xml");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RegisterClick()
    {
      SceneManager.LoadScene("Level1");
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
      xmlNames.Add("Level14","mistery1.xml");
      xmlNames.Add("Level15","mistery2.xml");
      xmlNames.Add("Level16","mistery3.xml");
      xmlNames.Add("Level17","mistery4.xml");
      xmlNames.Add("Level18","mistery5.xml");
      xmlNames.Add("Level19","mistery6.xml");
      xmlNames.Add("Level20","mistery8.xml");
      xmlNames.Add("Level21","mistery9.xml");
      xmlNames.Add("Level22","mistery11.xml");
      xmlNames.Add("Level23","mistery12.xml");


      Shuffle();
      while(checkRepeated()){
        Shuffle();
      }
      for (int i = 0; i < levelsUnordered.Length; i++)
      {
        levels.Add(levelsOrdered[i],levelsUnordered[i]);
        Debug.Log(levelsOrdered[i] + " " + levelsUnordered[i]);
      }
    }

    public void Shuffle()
    {
         for (int i = 0; i < levelsUnordered.Length; i++) {
             int rnd = Random.Range(0, levelsUnordered.Length);
             while(i == rnd){
               rnd = Random.Range(0, levelsUnordered.Length);
             }
             var tempGO = levelsUnordered[rnd];
             levelsUnordered[rnd] = levelsUnordered[i];
             levelsUnordered[i] = tempGO;
         }
    }


    public bool checkRepeated(){
      for(int i = 0;i<levelsUnordered.Length; i++){
        if(levelsOrdered[i] == levelsUnordered[i]){
          return true;
        }
      }
      return false;
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
}














/*
Level 1 -> Level14 OK
Level 2 -> Level16 OK
Level 3 -> Level10
Level 4 -> Level19
Level 5 -> Level4
Level 6 -> Level13
Level 7 -> Level2 OK
Level 8 -> Level5
Level 9 -> Level8
Level 10 -> Level6
Level 11 -> Level9
Level 12 -> Level11
Level 13 -> Level18
Level 14 -> Level17 OK
Level 15 -> Level23 OK
Level 16 -> Level15 OK
Level 17 -> Level7 OK
Level 18 -> Level21
Level 19-> Level22
Level 20 -> Level1 OK
Level 21 -> Level3
Level 22 -> Level12
Level 23 -> Level20 OK

*/
