using UnityEngine;
using System.Collections;
using System;
using System.IO;


/*
 * class for logging in-game events into simple text file
 * designed as a singleton - single GameObject 'Logger' should exists in all scenes
 * from any other class, you can simply call:
 *    Logger.addLogEntry("this is my entry");
 * to add new entry to log
 * 
 * \author Jiri Chmelik
 * \date 07-2014
 */
public class Logger : ScriptableObject 
{
    public static string logPath = "Logs/NewronLog.txt";

    #if UNITY_STANDALONE_WIN
    
        void Start()
        {
            //check if log already exists
            if (!File.Exists(logPath))
            {
                addLogEntry("Log file created");
            }	

            addLogEntry("New session started");
        }

        void OnApplicationQuit()
        {
            addLogEntry("Session ended\r\n\r\n\r\n");
        }

        public static void addLogEntry(string entry)
        {
            File.AppendAllText(logPath, Convert.ToString(DateTime.Now) + " || " + entry + "\r\n");
        }
    
    #elif UNITY_WEBPLAYER
        public static void addLogEntry(string entry)
        {
            //Debug.Log("Logger not supported for webplayer");
            Debug.Log(entry);

        }

    #endif 
}
