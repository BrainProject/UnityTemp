using UnityEngine;
using System.Collections;


/**
 *
 * Helper class to set-up logger
 * 
 * script should be attached to some enabled object in main scene - 
 *
 * \author Jiri Chmelik
 * \date 08-2014
 */
public class LoggerSetup : MonoBehaviour
{
    public string path = "Logs";
    public string filename = "NewronLog.txt";

    //set-up and initialize Logger class and create directory and file if necessary
    void Start()
    {
        Logger.Initialize(path, filename);
    }

    void OnApplicationQuit()
    {
        Debug.Log("On application quit");
        Logger.Stop();
    }

}
