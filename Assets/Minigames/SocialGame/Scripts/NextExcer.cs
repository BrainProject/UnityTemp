using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class NextExcer : MonoBehaviour {
		public string CounterName = "FitCounter";
		// Use this for initialization
		void Start () {
			GameObject counter = GameObject.Find(CounterName);
			FitCounter counterSript = null;
			if(counter)
				counterSript = counter.GetComponent<FitCounter>();
			if(counterSript)
			{
				counterSript.nextComplete();
				Destroy(this);
			}
			else
			{
				Debug.LogError(gameObject.name + " cannot find FitCounter script.");
			}

		}
	}
}
