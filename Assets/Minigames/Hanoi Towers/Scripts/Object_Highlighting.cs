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

            startcolor = GetComponent<Renderer>().material.color;
            GetComponent<Renderer>().material.color = new Color(255, 255, 0, 0.5f);

        }

        void OnMouseExit()
        {
            GetComponent<Renderer>().material.color = startcolor;
        }


    }

}