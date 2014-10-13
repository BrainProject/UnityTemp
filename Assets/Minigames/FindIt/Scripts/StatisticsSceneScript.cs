using UnityEngine;
using System.Collections;

namespace FindIt
{
	public class StatisticsSceneScript : MonoBehaviour
	{
		public GUIText totalNumberOfClicksText;
		public GUIText numberOfPicturesText;
		public GUIText leftGoodClicksText;
		public GUIText leftWrongClicksText;
		public GUIText rightGoodClicksText;
		public GUIText rightWrongClicksText;
		public GUIText leftAverageFindTimeText;
		public GUIText rightAverageFindTimeText;
		public GUIText totalAverageFindTimeText;
		
		// Use this for initialization
		void Start()
		{
			totalNumberOfClicksText.text = FindItStatistics.turnsPassed.ToString()  + "/" + FindItStatistics.expectedGameTurnsTotal.ToString();
			numberOfPicturesText.text = FindItStatistics.numberPieces.ToString();
			leftGoodClicksText.text = FindItStatistics.goodClicksLeft.ToString();
			leftWrongClicksText.text = FindItStatistics.wrongClicksLeft.ToString();
			rightGoodClicksText.text = FindItStatistics.goodClicksRight.ToString();
			rightWrongClicksText.text = FindItStatistics.wrongClicksRight.ToString();
			leftAverageFindTimeText.text = FindItStatistics.GetAverageClickFindTimeLeft().ToString() + " s";
			rightAverageFindTimeText.text = FindItStatistics.GetAverageClickFindTimeRight().ToString() + " s";
			totalAverageFindTimeText.text = FindItStatistics.GetAverageClickFindTimeTotal().ToString() + " s";

		}
	}
}
