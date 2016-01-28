using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LondonTower
{
    /// <summary>
    /// containst start/win configuration for level
    /// work with 3 poles
    /// </summary>
    public class LondonToweGameStartWinData
    {

        /// <summary>
        /// for matching win-start configuration in list
        /// </summary>
        int gameID;



        bool isStart;

        public List<string> pole1 = new List<string>();
        public List<string> pole2 = new List<string>();
        public List<string> pole3 = new List<string>();
        int pole1Size, pole2Size, pole3Size;

        public int GameID
        {
            get { return gameID; }
        }

        public int Pole3Size
        {
            get { return pole3Size; }
            set { pole3Size = value; }
        }

        public int Pole2Size
        {
            get { return pole2Size; }
            set { pole2Size = value; }
        }

        public int Pole1Size
        {
            get { return pole1Size; }
            set { pole1Size = value; }
        }

        public LondonToweGameStartWinData(bool isStart, int gameId)
        {
            this.isStart = isStart;
            this.gameID = gameId;
        }

        public void AddBallOnPole(int pole, string ballId)
        {
            switch (pole)
            {
                case 1: pole1.Add(ballId); break;
                case 2: pole2.Add(ballId); break;
                case 3: pole3.Add(ballId); break;
            }

        }

        public bool IsStart()
        {
            return isStart;
        }

        public override string ToString()
        {
            string data = gameID.ToString() + "==";
            foreach (string s in pole1)
            {
                data = data + s + ";";
            }
            data = data + "::";
            foreach (string s in pole2)
            {
                data = data + s + ";";
            }
            data = data + "::";
            foreach (string s in pole3)
            {
                data = data + s + ";";
            }

            return data + "__" + pole1Size.ToString() + "__" + pole2Size.ToString() + "__" + pole3Size.ToString() + ";;" + isStart.ToString();
        }
    }
}