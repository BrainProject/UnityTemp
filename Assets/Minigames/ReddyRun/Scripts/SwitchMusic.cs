using UnityEngine;
using System.Collections;
using UnityEditor;
using Image = UnityEngine.UI.Image;


namespace Reddy
{
    public class SwitchMusic : MonoBehaviour
    {

        public GameObject soundButton;
        public Sprite soundOn;
        public Sprite soundOff;
        public Sprite soundOnHigh;
        public Sprite soundOffHigh;
        bool sound = true;

        void Start()
        {

        }

        public void switchSprite()
        {
            sound = !sound;
            switch (sound)
            {
                case true:
                    soundButton.GetComponent<Image>().sprite = soundOn;
                    break;

                case false:
                    soundButton.GetComponent<Image>().sprite = soundOff;
                    break;

            }
        }

        void Update()
        {
            //OnMouseOver();

        }


        void OnMouseOver()
        {
            print(sound);

            switch (sound)
            {
                case true:
                    soundButton.GetComponent<Image>().sprite = soundOnHigh;
                    break;

                case false:
                    soundButton.GetComponent<Image>().sprite = soundOffHigh;
                    break;

            }
        }

    }

}
