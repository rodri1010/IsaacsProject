using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using UnityEngine.UI;

public class ImportXML : MonoBehaviour
{
    public TextAsset xmlRawFile;
    // Start is called before the first frame update
    void Start()
    {
      string data = xmlRawFile.text;
      parseXMLFile(data);
    }

    void parseXMLFile(string path)
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(path);
      string description = doc.SelectSingleNode("StreamingAssets/description/text()").Value;
      Debug.Log(description);


    }
}
