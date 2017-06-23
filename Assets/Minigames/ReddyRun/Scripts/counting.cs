using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Reddy
{
    public class counting : MonoBehaviour
    {

        private int count;
        public Text countText;
        public Text winText;
        Animator anim;

        private int miniJump;

        // Use this for initialization
        void Start()
        {
            count = 0;
            SetCountText();
            anim = GetComponent<Animator>();

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter(Collider other)
        {
            print("collider pickup detected");
            if (other.gameObject.CompareTag("Pick up"))
            {
                print("pickup tag detected");
                other.gameObject.SetActive(false);
                count++;
                SetCountText();
            }
            if (other.gameObject.CompareTag("Finished"))
            {

                anim.SetBool("finish", true);
            }

        }

        void SetCountText()
        {
            countText.text = count.ToString();
        }
    }

}
