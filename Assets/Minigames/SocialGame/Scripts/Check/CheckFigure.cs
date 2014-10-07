using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class CheckFigure : Check {
		public bool check;
		private bool checkedLastUpdate;

		protected override void Start()
		{
			base.Start();
			changeColorMaterial(Color.red);
		}


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
					changeColorMaterial(Color.red);
					//do samething
				}
				else
				{
					check = true;
					changeColorMaterial(Color.green);
					//do samething
				}
			}
			checkedLastUpdate = false;
		}

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
