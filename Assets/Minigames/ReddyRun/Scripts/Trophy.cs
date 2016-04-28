using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Reddy
{
    public class Trophy : MonoBehaviour
    {
        private string text;
        private bool isUnlocked;
        private Sprite imageTrophy;
        private GameObject currentSprite;

        public string getText()
        {
            return text;
        }
        public void setText(string inputText)
        {
            this.text = inputText;
        }

        public bool getIsUnlocked()
        {
            return isUnlocked;
        }
        public void setIsUnlocked(bool lockSwitch)
        {
            this.isUnlocked = lockSwitch;
        }

        public void setImage(Sprite imageTrophy)
        {
            this.imageTrophy = imageTrophy;
        }

        public Sprite getImage()
        {
            return imageTrophy;
        }

        public GameObject getSprite()
        {
            return currentSprite;
        }
        public void setSprite(GameObject currentSprite)
        {
            this.currentSprite = currentSprite;
        }

        public Trophy(string text, Sprite imageTrophy)
        {
            this.text = text;
            this.imageTrophy = imageTrophy;
            isUnlocked = false;
        }

        public Trophy(string text, Sprite imageTrophy, GameObject currentSprite)
        {
            this.text = text;
            this.imageTrophy = imageTrophy;
            this.currentSprite = currentSprite;
            isUnlocked = false;
        }

        public Trophy(string text)
        {
            this.text = text;
            isUnlocked = false;
        }
    }

}

