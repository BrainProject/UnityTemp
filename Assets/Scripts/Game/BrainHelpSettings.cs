using UnityEngine;
using System.Collections;

namespace Game
{
    /// <summary>
    /// Help settings sets initial parameters of help bubble GUI texture.
    /// \author: Milan Doležal
    /// </summary>

	public class BrainHelpSettings : MonoBehaviour 
    {
        [Range(0.1f, 1f)]
		public float helpSize = 0.6f;
		public GameObject blockBorder;

		internal GameObject neuronHelp;
		internal GameObject blockBorderClone;

		private Color originalColor;
		private Color targetColor;
		private bool canControl;

		void Start () 
        {
			blockBorderClone = (GameObject)Instantiate (blockBorder);
			//neuronHelp.GetComponent<BrainHelp> ().helpExists = true;
			canControl = true;
			originalColor = this.guiTexture.color;
			targetColor = this.guiTexture.color;
			originalColor.a = 0;
			targetColor.a = 1;
			this.guiTexture.color = originalColor;
            this.guiTexture.pixelInset = new Rect(Screen.width * (0.5f * (1.0f - helpSize)), Screen.height * (0.5f * (1.0f - helpSize)), Screen.width * helpSize, Screen.height * helpSize);
			this.transform.position = new Vector2(0, 0);
			StartCoroutine("FadeIn");
		}

		void Update()
		{
			#if UNITY_STANDALONE
			if(Input.GetButtonDown("Horizontal")  || (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.L)) && canControl)
			#else
			if(Input.GetButtonDown("Horizontal") && canControl)
			#endif
			{
//				neuronHelp.GetComponent<BrainHelp> ().helpExists = false;
				if((Input.GetAxis("Horizontal") < 0) || (Input.GetKeyDown (KeyCode.J) && Input.GetMouseButton(0)))
					StartCoroutine (MoveAway(-1.0f));
				if((Input.GetAxis("Horizontal") > 0) || (Input.GetKeyDown (KeyCode.L) && Input.GetMouseButton(0)))
					StartCoroutine (MoveAway(1.0f));

				    
				StartCoroutine ("FadeOut");
			}
		}

		void OnMouseUp()
		{
//			neuronHelp.GetComponent<BrainHelp> ().helpExists = false;
			canControl = false;
			StopCoroutine ("FadeIn");
			StartCoroutine ("FadeOut");
		}

		IEnumerator FadeIn()
		{
			float startTime = Time.time;

			while(this.guiTexture.color.a < 0.5f)
			{
				this.guiTexture.color = Color.Lerp (originalColor, targetColor, (Time.time - startTime)/6);
				yield return null;
			}
			//Time.timeScale = 0;
		}

		IEnumerator FadeOut()
		{
			Destroy (blockBorderClone);
			float startTime = Time.time;
			StopCoroutine ("FadeIn");
			originalColor = this.guiTexture.color;
			targetColor.a = 0;

			while(this.guiTexture.color.a > 0.001f)
			{
				this.guiTexture.color = Color.Lerp (originalColor, targetColor, Time.time - startTime);
				//Time.timeScale = state;
				yield return null;
			}
			//Time.timeScale = 1.0f;
			Destroy (this.gameObject);
		}

		IEnumerator MoveAway(float direction)
		{
			int speed = 2; //the lower, the faster
			Vector2 texturePosition = this.transform.position;
			canControl = false;
			if(direction < 0) //move left
			{
				while(this.transform.position.x > -1)
				{
					texturePosition.x -= Time.deltaTime/speed;
					this.transform.position = texturePosition;
					yield return null;
				}
			}
			if(direction > 0) //move right
			{
				while(this.transform.position.x < 1)
				{
					texturePosition.x += Time.deltaTime/speed;
					this.transform.position = texturePosition;
					yield return null;
				}
			}
			Destroy (this.gameObject);
		}
	}
}