/**
 * @file StopMeasuingTimeOnClick.cs
 * @author Ján Bella
 */
using UnityEngine;
using System.Collections;

namespace FindIt
{
    /**
     * Callback that stops statistics stopwatch on click
     */
	public class StopMeasuingTimeOnClick : MonoBehaviour
	{
		void OnMouseDown()
		{
			FindItStatistics.StopMeasuringTime();
		}
	}
}
