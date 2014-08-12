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

    private static bool loggerFlag = false;

    //Initialize Logger class and create directory and file if necessary
    //this have to be in Awake() function - before any log entries are created in Start() functions
    void Awake()
    {
        if (!loggerFlag)
        {
            Logger.Initialize(path, filename);
            loggerFlag = true;
        }
    }

    void OnApplicationQuit()
    {
        //Debug.Log("On application quit");
        Logger.Stop();
    }

}
