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
            //System.IO.File.AppendAllText(logPath, Convert.ToString(DateTime.Now) + " || " + entry + "\r\n");
            logfile.WriteLine(Convert.ToString(DateTime.Now) + " || " + entry);
            logfile.Flush();
        }

        public static void Stop()
        {
            //Debug.Log("On application quit");
            addLogEntry("Session ended\r\n\r\n\r\n");
            logfile.Close();
        }
    #endif 
}
