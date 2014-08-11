using UnityEngine;
using System.Collections;


/**
 *
 * Helper class to set-up logger
 * 
 * You should not use this script alone. Use 'Logger' prefab instead.
 *
 * \author Jiri Chmelik
 * \date 08-2014
 */
public class LoggerSetup : MonoBehaviour
{
    public string path = "Logs";
    public string filename = "NewronLog.txt";

    //Initialize Logger class and create directory and file if necessary
    //this have to be in Awake() function - before any log entries are created in Start() functions
    void Awake()
    {
        Logger.Initialize(path, filename);
    }

    void OnApplicationQuit()
    {
        //Debug.Log("On application quit");
        Logger.Stop();
    }

}
