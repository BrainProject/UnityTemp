using UnityEngine;
using System.Collections;

/// <summary>
/// Help settings sets initial parameters of help bubble GUI texture.
/// \author: Milan Doležal
/// </summary>
namespace Game
{
	public class BrainHelpSettings : MonoBehaviour {
		private Color originalColor;
		private Color targetColor;
		private bool canControl;

		void Start () {
			GameObject.Find ("Neuron").GetComponent<Game.BrainHelp> ().helpExists = true;
			canControl = true;
			originalColor = this.guiTexture.color;
			targetColor = this.guiTexture.color;
			originalColor.a = 0;
			targetColor.a = 1;
			this.guiTexture.color = originalColor;
			this.guiTexture.pixelInset = new Rect (Screen.width / 4, Screen.height / 4, Screen.width / 2, Screen.height / 2);
			this.transform.position = new Vector2(0, 0);
			StartCoroutine("FadeIn");
		}

		void Update()
		{
			if(Input.GetButtonDown("Horizontal") && canControl)
			{
				GameObject.Find ("Neuron").GetComponent<Game.BrainHelp> ().helpExists = false;
				StartCoroutine (MoveAway(Input.GetAxis("Horizontal")));
				StartCoroutine ("FadeOut");
			}
		}

		IEnumerator FadeIn()
		{
			float startTime = Time.time;

			while(this.guiTexture.color.a < 0.5f)
			{
				this.guiTexture.color = Color.Lerp (originalColor, targetColor,(Time.time - startTime)/6);
				yield return null;
			}
		}

		IEnumerator FadeOut()
		{
			float startTime = Time.time;
			StopCoroutine ("FadeIn");
			originalColor = this.guiTexture.color;
			targetColor.a = 0;

			while(this.guiTexture.color.a > 0.001f)
			{
				this.guiTexture.color = Color.Lerp (originalColor, targetColor,(Time.time - startTime));
				yield return null;
			}
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