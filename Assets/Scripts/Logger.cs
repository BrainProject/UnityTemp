using UnityEngine;
using System.Collections;
using System;
using System.IO;

/*
 * static class for logging in-game events into simple text file
 * from any other class, you can simply call:
 *    Logger.addLogEntry("this is my entry");
 * to add new entry to log
 * 
 * Logger should be initialized in "main" scene
 * If you want to debug single scene (e.g. "myGreatGame.scene"), 
 * simply add "Logger" prefab to your scene hierarchy. 
 * Via this prefab, you can set variables of Logger - path and filename
 * 
 * \author Jiri Chmelik
 * \date 07-2014
 */
public static class Logger : object 
{
    
    private static StreamWriter logfile;

    
    // in case of web build, logger can't be use, so there are only empty method definitions
    #if UNITY_WEBPLAYER
        
        public static void Initialize(string path, string filename)
        {

        }

        public static void addLogEntry(string entry)
        {
            //Debug.Log("Logger not supported for webplayer");
            Debug.Log(entry);

        }

        public static void Stop()
        {

        }

#else

    public static void Initialize(string path, string filename)
        {
            
            string logPath = path + "/" + filename;
            Debug.Log("Newron log path: " + logPath);
            //Debug.Log("Application data path: " + Application.dataPath);
            //Debug.Log("Application persistent data path: " + Application.persistentDataPath);

            //create directory if it don't exists already
            Directory.CreateDirectory(path);
            //Debug.Log("directory: " + dInfo.FullName + " should exists now");

            bool addCreatedEntry = false;
            //check if log already exists
            if (!File.Exists(logPath))
            {
                addCreatedEntry = true;
            }

            //create new stream writer - will create new or append to existing file
            logfile = new System.IO.StreamWriter(logPath, true);

            if (addCreatedEntry)
            {
                addLogEntry("Log file created");
            }

            addLogEntry("New session started");
        }
        

        public static void addLogEntry(string entry)
        {
            if (logfile != null)
            {
                logfile.WriteLine(Convert.ToString(DateTime.Now) + " || " + entry);
                logfile.Flush();
            }
            else
            {
                Debug.LogWarning("Logger is not initialized - entry will not be added to log file. Do you have active 'Logger' prefab in your scene?");
            }
        }

        public static void Stop()
        {
            //Debug.Log("On application quit");
            addLogEntry("Session ended\r\n\r\n\r\n");
            logfile.Close();
        }
    #endif 
}
