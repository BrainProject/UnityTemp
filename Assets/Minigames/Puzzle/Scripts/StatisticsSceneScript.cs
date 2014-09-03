using UnityEngine;
using System.Collections;

namespace Puzzle
{
    public class StatisticsSceneScript : MonoBehaviour
    {
        public GUIText timeText;
        public GUIText piecesText;
        public GUIText totalText;
        public GUIText connectionText;
        public GUIText noconnectionText;

        // Use this for initialization
        void Start()
        {
            /*((GUIText)GUI.Find("TimeGUIText_Value")).text = PuzzleStatistics.gameTime + " s";
            ((GUIText)GameObject.Find("NumberPiecesGUIText_Value")).text = PuzzleStatistics.numberPieces;
            ((GUIText)GameObject.Find("NumberClicksTotalGUIText_Value")).text = PuzzleStatistics.GetNumberClicksTotal();
            ((GUIText)GameObject.Find("WithConnectionGUIText_Value")).text = PuzzleStatistics.numberClicksWithConnection;
            ((GUIText)GameObject.Find("WithoutConnectionGUIText_Value")).text = PuzzleStatistics.numberClicksWithoutConnection;
        */
            timeText.text = PuzzleStatistics.gameTime.ToString() + " s";
            piecesText.text = PuzzleStatistics.numberPieces.ToString();
            totalText.text = PuzzleStatistics.GetNumberClicksTotal().ToString();
            connectionText.text = PuzzleStatistics.numberClicksWithConnection.ToString();
            noconnectionText.text = PuzzleStatistics.numberClicksWithoutConnection.ToString();
        }
    }
}
