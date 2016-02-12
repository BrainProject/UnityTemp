using UnityEngine;
using System.Collections;

namespace ButterflyRush
{
    public class PlayerControls : MonoBehaviour
    {
        private RaycastHit hit;

        // Update is called once per frame
        void Update()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
		for (int i=0; i<Input.touchCount; ++i)
		{
			if (Physics.Raycast((Camera.main.ScreenPointToRay(Input.GetTouch(i).position)), out hit))
			{
				if(hit.transform.tag == "Cocoon")
				{
					hit.transform.GetComponent<Cocoon>().HitReaction();
				}
			}
		}
#else
            //if (Input.GetMouseButton(0))
            {
                if (Physics.Raycast((Camera.main.ScreenPointToRay(Input.mousePosition)), out hit))
                {
                    if (hit.transform.tag == "Cocoon")
                    {
                        hit.transform.GetComponent<Cocoon>().HitReaction();
                    }
                }
            }
#endif
        }
    }
}