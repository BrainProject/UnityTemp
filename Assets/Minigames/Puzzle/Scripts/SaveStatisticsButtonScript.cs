using UnityEngine;
using System.Collections;
using System.IO;
using System;

namespace Puzzle
{
    public class SaveStatisticsButtonScript : MonoBehaviour
    {
        private Color noActionColor = new Color32(0x33, 0x33, 0x33, 0xFF);

        private Color pointerOverColor = new Color32(0xFF, 0x77, 0x44, 0xFF);

        public string filename = "statistics.csv";

        private bool message = false;

        void Start()
        {
            GetComponent<SpriteRenderer>().color = noActionColor;
        }

        void OnMouseEnter()
        {
            GetComponent<SpriteRenderer>().color = pointerOverColor;
        }

        void OnMouseExit()
        {
            GetComponent<SpriteRenderer>().color = noActionColor;
        }

        void OnMouseDown()
        {
            SaveStatistics(filename);
            message = true;
        }

        void SaveStatistics(string filename)
        {
            // instead of filename, a path could be built there ...
            exportToCSV(filename);
            Invoke("HideMessage", 1.5f);
        }

        private void HideMessage()
        {
            message = false;
        }

        /// <summary>
        /// stores description of collumns to CSV file
        /// </summary>
        /// <param name="path">file to have the data saved</param>
        private void CSV_head(string path)
        {
            StreamWriter sw = new System.IO.StreamWriter(path, false, System.Text.Encoding.UTF8);
            sw.Write("Date;Image;Number of pieces;Total moves;Moves connecting components;Moves without connection");
            sw.WriteLine();
            sw.Close();
        }

        /// <summary>
        /// append statistics to CSV
        /// </summary>
        /// <param name="path"></param>
        private void exportToCSV(string path)
        {
            if (!File.Exists(path))
            {
                CSV_head(path);
            }
            StreamWriter sw = new System.IO.StreamWriter(path, true, System.Text.Encoding.UTF8);

            sw.Write(DateTime.Now.Date.ToString("dd.MM.yyyy") + ";");
            sw.Write(PuzzleStatistics.pictureName + ";");
            sw.Write(PuzzleStatistics.numberPieces + ";");
            sw.Write(PuzzleStatistics.GetNumberClicksTotal() + ";");
            sw.Write(PuzzleStatistics.numberClicksWithConnection + ";");
            sw.Write(PuzzleStatistics.numberClicksWithoutConnection + ";");
            sw.WriteLine();
            sw.Close();
        }

        void OnGUI()
        {
            if(message)
            {
                GUI.TextArea(new Rect((Screen.width / 2) - 75, (Screen.height / 2) - 10,150, 20), "Statistics saved");
            }
        }

    }
}
