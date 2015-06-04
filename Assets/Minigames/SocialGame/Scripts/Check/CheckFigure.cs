using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class CheckFigure : Check {
		public bool check;
		//private bool checkedLastUpdate;

		/// <summary>
		/// uncheck this check.
		/// </summary>
		public void UnCheck()
		{
			check = false;
			changeColorMaterial(Color.red);
		}

		protected override void Start()
		{
			base.Start();
			changeColorMaterial(Color.red);
		}


		public override void thisActivate()
		{
			check = true;
			changeColorMaterial(Color.green);
		}

		/// <summary>
		/// Changes the color material.
		/// </summary>
		/// <param name="color">Color.</param>
		void changeColorMaterial(Color color)
		{
			MeshRenderer  render = gameObject.GetComponent<MeshRenderer>();
			if(render)
			{
				render.material.color = color;	
			}
			else
			{
				SpriteRenderer spriteRender = gameObject.GetComponent<SpriteRenderer>();
				if(spriteRender)
				{
					spriteRender.color = color;
				}
			}
		}

	}
}
