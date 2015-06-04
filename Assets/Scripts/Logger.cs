using UnityEngine;
using System.Collections;
using System;
using System.IO;

/// <summary>
/// Class for logging in-game events into simple text file
/// </summary>
/// <example>
/// To use Logger, define a global variable:
/// <code>Logger logger;</code>
/// and (preferably in Start() function) get pointer via Master Game Controller:
/// <code>logger = MGC.Instance.Logger()</code>
/// Then, you can new entries by>
/// <code>Logger.addEntry("this is my entry");</code>
/// </example>
/// \author Jiri Chmelik
/// \date 07-2014
public class Logger : MonoBehaviour
{
    private string path;
    private string filename;

    private StreamWriter logfile;

    // in case of web build, logger can't be use, so there are only empty method definitions
	#if UNITY_WEBPLAYER || UNITY_ANDROID
        
        public void Initialize(string path, string filename)
        {

        }

        public void addEntry(string entry)
        {
            //Debug.Log("Logger not supported for webplayer");
            Debug.Log(entry);

        }

        public void Stop()
        {

        }

    #else

        public void Initialize(string ppath, string pfilename)
        {
            path = ppath;
            filename = pfilename;
        
            Debug.Log("Initialization of Logger...");

            //create directory if it don't exists already
            Directory.CreateDirectory(path);
            //Debug.Log("directory: " + dInfo.FullName + " should exists now");

            string logPath = path + "/" + filename;
            print("Newron Log will be saved to: '" + logPath + "PlayerActions.txt'");

            bool addCreatedEntry = false;
            
            //check if log already exists
            if (!File.Exists(logPath))
            {
                addCreatedEntry = true;
            }

            //create new stream writer - will create new or append to existing file
			try
			{
            	logfile = new System.IO.StreamWriter(logPath, true);
			}
			catch(IOException e)
			{
				Debug.LogWarning("Log file creation failed!");
                Debug.LogWarning(e);
			}

            if (addCreatedEntry)
            {
                addEntry("Log file created");
            }

            addEntry("New session started");
        }
        

        public void addEntry(string entry)
        {
            if (logfile != null)
            {
                logfile.WriteLine(Convert.ToString(DateTime.Now) + " || " + entry);
                logfile.Flush();
            }
            else
            {
                Debug.LogWarning("Logger is not initialized - entry will not be added to log file.");
            }
        }

        /// <summary>
        /// Finish logging.
        /// </summary>
        public void OnApplicationQuit()
        {  
            if (logfile != null)
            {
                print("Closing log file");
                addEntry("Session ended\r\n\r\n\r\n");

                logfile.Close();
            }
        }
    #endif 
}
