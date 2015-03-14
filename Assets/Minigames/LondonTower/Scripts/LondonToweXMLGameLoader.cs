using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Class for parsing XML file into data about level
/// </summary>
public class LondonToweXMLGameLoader
{

    List<LondonToweGameStartWinData> data = new List<LondonToweGameStartWinData>();
    LondonToweGameStartWinData pokus1;
    LondonToweGameStartWinData pokus2;

    /// <summary>
    /// Parse xml file and return list of all available levels in xml
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    /// 
    public List<LondonToweGameStartWinData> ParseXmlFile(string path)
    {
        using (XmlReader xr = XmlReader.Create(path))
        {
            ParseXml(xr);
        }
        return data;
    }

    /// <summary>
    /// Parse xmltextasset and return list of all available levels in xml
    /// </summary>
    /// <param name="textAsset"></param>
    /// <returns></returns>
    public List<LondonToweGameStartWinData> ParseXmlTextAsset(TextAsset textAsset)
    {
        using (XmlReader xr = XmlReader.Create(new StringReader(textAsset.text)))
        {
            ParseXml(xr);
        }
        return data;
    }

    private List<LondonToweGameStartWinData> ParseXml(XmlReader xr)
    {
        int idGame;
        int rodeCount = 0;
        bool start = true;
        while (xr.Read())
        {
            if (xr.NodeType == XmlNodeType.Element)
            {
                if (xr.Name == "level")
                {

                    idGame = int.Parse(xr.GetAttribute("lid"));
                    pokus1 = new LondonToweGameStartWinData(true, idGame);
                    pokus2 = new LondonToweGameStartWinData(false, idGame);
                    start = true;
                    rodeCount = 0;
                }

                else if (xr.Name == "rod")
                {
                    start = true;
                    rodeCount++;
                    int size = int.Parse(xr.GetAttribute("height"));

                    if (rodeCount == 1)
                    {
                        pokus1.Pole1Size = size;
                        pokus2.Pole1Size = size;
                    }
                    else if (rodeCount == 2)
                    {
                        pokus1.Pole2Size = size;
                        pokus2.Pole2Size = size;
                    }
                    else if (rodeCount == 3)
                    {
                        pokus1.Pole3Size = size;
                        pokus2.Pole3Size = size;
                    }

                }
                
                else if (xr.Name == "solution")
                {
                    start = false;
                }
                
                else if (xr.Name == "sphere")
                {
                    if (start)
                    {
                        if (rodeCount == 1)
                        {
                            pokus1.pole1.Add(xr.GetAttribute("color"));
                        }
                        else if (rodeCount == 2)
                        {
                            pokus1.pole2.Add(xr.GetAttribute("color"));
                        }
                        else if (rodeCount == 3)
                        {
                            pokus1.pole3.Add(xr.GetAttribute("color"));
                        }
                    }
                    else
                    {
                        if (rodeCount == 1)
                        {
                            pokus2.pole1.Add(xr.GetAttribute("color"));
                        }
                        else if (rodeCount == 2)
                        {
                            pokus2.pole2.Add(xr.GetAttribute("color"));
                        }
                        else if (rodeCount == 3)
                        {
                            pokus2.pole3.Add(xr.GetAttribute("color"));
                        }
                    }
                }
            }
            
                // loading element vaue
            else if ((xr.NodeType == XmlNodeType.EndElement) && (xr.Name == "level"))
            {
                if (pokus1 != null && pokus2 != null)
                {
                    data.Add(pokus1);
                    data.Add(pokus2);
                }
            }
        }
        return data;
    }
}
