using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class NextExcer : MonoBehaviour {
#if UNITY_STANDALONE
		public string CounterName = "FitCounter";

		void Start () {
			GameObject counter = GameObject.Find(CounterName);
			FitCounter counterSript = null;
			if(counter)
				counterSript = counter.GetComponent<FitCounter>();
			if(counterSript)
			{
				counterSript.nextComplete();
				Destroy(gameObject);
			}
			else
			{
				Debug.LogError(gameObject.name + " cannot find FitCounter script.");
			}

		}
#endif
	}
}
