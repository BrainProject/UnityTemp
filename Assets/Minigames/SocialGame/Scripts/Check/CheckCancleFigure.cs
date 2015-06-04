using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class CheckCancleFigure : Check {
		#if UNITY_STANDALONE

		public GestCheckerFigure figure;

		public ChangeSprites anim;
		public int numOfParts;
		public float delayPerPart=0.1f;

		private bool handOnIt;
		private bool corutineRuning;

		public override bool Checked (Transform target)
		{
			handOnIt = true;
			if(!corutineRuning)
			{
				StartCoroutine(Stay());
			}
			return false;
		}

		/// <summary>
		/// Stay this until timer gone out.
		/// </summary>
		IEnumerator Stay()
		{
			corutineRuning = true;
			for (int i = 0; i<numOfParts; i++) 
			{
				if(handOnIt)
				{
					handOnIt = false;
					Timer(i);
					yield return new WaitForSeconds(delayPerPart);
				}
				else
				{
					TimerReset();
					yield break;
				}
			}
			EndTimer();
		}

		public override void thisActivate()
		{
			activated = false;
			show ();
			if (figure) 
			{
				figure.DestroyChecker();
			}
		}

		/// <summary>
		/// Ends the timer.
		/// </summary>
		private void EndTimer()
		{
			TimerReset ();
			activated = false;
			show ();
			if (figure) 
			{
				figure.DestroyChecker();
			}
			foreach(Check check in next)
			{
				check.activate();
			}

		}

		/// <summary>
		/// Reset timers .
		/// </summary>
		private void TimerReset()
		{
			if(anim)
			{
				anim.ResetSprites();
			}
			corutineRuning = false;
		}

		/// <summary>
		/// Timer, vusal control.
		/// </summary>
		/// <param name="part">which parts of timer will be show.</param>
		private void Timer(int part)
		{
			if(anim)
			{
				anim.SetImage(part);
			}
		}
		#endif
	}
}
