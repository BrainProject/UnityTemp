using UnityEngine;
using System.Collections;

namespace HanoiTowers
{

    public class Object_Highlighting : MonoBehaviour
    {

        private Color startcolor;

        void Start()
        {
            //renderer.material.color = Color.red;
        }

        void OnMouseEnter()
        {

            startcolor = renderer.material.color;
            renderer.material.color = new Color(255, 255, 0, 0.5f);

        }

        void OnMouseExit()
        {
            renderer.material.color = startcolor;
        }


    }

}