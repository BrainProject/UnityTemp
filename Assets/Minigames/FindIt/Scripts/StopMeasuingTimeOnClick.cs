using UnityEngine;
using System.Collections;

namespace FindIt
{
	public class StopMeasuingTimeOnClick : MonoBehaviour
	{
		void OnMouseDown()
		{
			FindItStatistics.StopMeasuringTime();
		}
	}
}
