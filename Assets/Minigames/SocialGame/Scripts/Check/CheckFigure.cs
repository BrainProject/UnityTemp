using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class CheckFigure : Check {
		public bool check;
		private bool checkedLastUpdate;
		public MonoBehaviour halo;

		public override void thisActivate()
		{
			checkedLastUpdate = true;
		}

		void LateUpdate()
		{
			if(checkedLastUpdate !=  check) // xor if is check = true and *lastupdete = false, check = false and *lastupdate =  
			{
				if(check)
				{
					check = false;
					//do samething
				}
				else
				{
					check = true;
					//do samething
				}
			}
			checkedLastUpdate = false;
		}

	}
}
