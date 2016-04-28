using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using System;


namespace Reddy
{
    public class TrophyManager : MonoBehaviour
    {
        private Trophy[] trophies = new Trophy[6];
        private int mainTrophy = 0;
        private int mainSprite = 0;
        private int countTrophies;
        private System.String textReference;
        private const float transitionFrameCount = 20;
        private float movingLeft = transitionFrameCount + 1;
        private float movingRight = transitionFrameCount + 1;

        private const int SpritesCount = 4;

        private float scaleBig = 14;
        private float scaleSmall = 10;
        private float scaleZero = 0;

        private int xL = -140;
        private int yL = -45;

        private int xM = 0;
        private int yM = -25;

        private int xR = 140;
        private int yR = -45;


        public string getMainText()
        {
            return trophies[mainTrophy].getText();
        }

        private void initializeTrophies()
        {

            trophies[0] = new Trophy("Úspešne zvládnutý prvý level.", (Sprite)AssetDatabase.LoadAssetAtPath("Assets/UI/trophies/firstLevel-08.png", typeof(Sprite)));
            trophies[1] = new Trophy("Prvý kyslík.", (Sprite)AssetDatabase.LoadAssetAtPath("Assets/UI/trophies/firstOxygen-07.png", typeof(Sprite)));
            trophies[2] = new Trophy("Všetky životy na konci levelu.", (Sprite)AssetDatabase.LoadAssetAtPath("Assets/UI/trophies/allLives-09.png", typeof(Sprite)));
            trophies[3] = new Trophy("Pozbierané všetky kyslíky v prvom leveli.", (Sprite)AssetDatabase.LoadAssetAtPath("Assets/UI/trophies/10_oxygens-6.png", typeof(Sprite)));
            trophies[4] = new Trophy("Posledný život na konci levelu.", (Sprite)AssetDatabase.LoadAssetAtPath("Assets/UI/trophies/one_heart-05.png", typeof(Sprite)));
            trophies[5] = new Trophy("Úspešne zvládnuté prvé tri levely.", (Sprite)AssetDatabase.LoadAssetAtPath("Assets/UI/trophies/threeLevels-10.png", typeof(Sprite)));

            setMainTrophy();
            trophies[mainTrophy].getSprite().GetComponent<SpriteRenderer>().sprite = trophies[mainTrophy].getImage();
            arrayMINUSTrophy().getSprite().GetComponent<SpriteRenderer>().sprite = trophies[trophies.Length - 1].getImage();
            arrayPLUSTrophy().getSprite().GetComponent<SpriteRenderer>().sprite = trophies[mainTrophy + 1].getImage();

            countTrophies = trophies.Length;



        }

        private Trophy arrayMINUSTrophy()
        {
            if (mainTrophy >= 1)
            {
                return trophies[mainTrophy - 1];
            }
            else
            {
                return trophies[trophies.Length - 1];
            }

        }

        private Trophy arrayPLUSTrophy()
        {
            if (mainTrophy < trophies.Length - 1)
            {
                return trophies[mainTrophy + 1];
            }
            else
            {
                return trophies[0];
            }

        }

        public void setMainTrophy()
        {
            trophies[mainTrophy].setSprite(GameObject.Find("Sprite0"));

            arrayMINUSTrophy().setSprite(GameObject.Find("Sprite3"));

            arrayPLUSTrophy().setSprite(GameObject.Find("Sprite1"));
        }

        public void newRightTrophy()
        {
            trophies[(mainTrophy + 2) % countTrophies].setSprite(GameObject.Find("Sprite" + (mainSprite + 2) % SpritesCount));
            trophies[(mainTrophy + 2) % countTrophies].getSprite().GetComponent<SpriteRenderer>().sprite = trophies[(mainTrophy + 2) % countTrophies].getImage();
        }

        public void newLeftTrophy()
        {
            trophies[(mainTrophy - 2 + countTrophies) % countTrophies].setSprite(GameObject.Find("Sprite" + (mainSprite - 2 + SpritesCount) % SpritesCount));
            trophies[(mainTrophy - 2 + countTrophies) % countTrophies].getSprite().GetComponent<SpriteRenderer>().sprite = trophies[(mainTrophy - 2 + countTrophies) % countTrophies].getImage();
        }




        void Start()
        {
            mainTrophy = 0;

            initializeTrophies();
            textReference = trophies[mainTrophy].getText();
            GameObject.Find("TextExactTrophy").GetComponent<Text>().text = textReference;

            GameObject.Find("Sprite" + ((mainSprite + 2) % SpritesCount)).transform.localScale = new Vector3(scaleZero, scaleZero, scaleZero);



        }




        void Update()
        {

            if (Input.GetButtonDown("Horizontal"))
            {
                if (Input.GetAxis("Horizontal") > 0 && movingRight == transitionFrameCount + 1 && movingLeft == transitionFrameCount + 1) // Right
                {
                    newLeftTrophy();
                    if (mainTrophy == 0)
                    {
                        mainTrophy = countTrophies - 1;
                    }
                    else
                    {
                        mainTrophy -= 1;
                    }
                    movingRight = 1;
                    GameObject.Find("Sprite" + ((mainSprite + 2) % SpritesCount)).transform.localPosition = new Vector3(xL, yL, 0);

                    GameObject.Find("Sprite" + mainSprite).GetComponent<SpriteRenderer>().sortingLayerName = "Back";
                    GameObject.Find("Sprite" + (mainSprite - 1 + SpritesCount) % SpritesCount).GetComponent<SpriteRenderer>().sortingLayerName = "Front";
                    GameObject.Find("Sprite" + (mainSprite + 1) % SpritesCount).GetComponent<SpriteRenderer>().sortingLayerName = "Invisible";
                }
                if (Input.GetAxis("Horizontal") < 0 && movingLeft == transitionFrameCount + 1 && movingRight == transitionFrameCount + 1) // Left
                {
                    newRightTrophy();
                    if (mainTrophy == countTrophies - 1)
                    {
                        mainTrophy = 0;
                    }
                    else
                    {
                        mainTrophy += 1;
                    }
                    movingLeft = 1;
                    GameObject.Find("Sprite" + ((mainSprite + 2) % SpritesCount)).transform.localPosition = new Vector3(xR, yR, 0);

                    GameObject.Find("Sprite" + mainSprite).GetComponent<SpriteRenderer>().sortingLayerName = "Back";
                    GameObject.Find("Sprite" + (mainSprite - 1 + SpritesCount) % SpritesCount).GetComponent<SpriteRenderer>().sortingLayerName = "Invisible";
                    GameObject.Find("Sprite" + (mainSprite + 1) % SpritesCount).GetComponent<SpriteRenderer>().sortingLayerName = "Front";
                }

                setMainTrophy();



                //change text of exact trophy text
                textReference = trophies[mainTrophy].getText();
                GameObject.Find("TextExactTrophy").GetComponent<Text>().text = textReference;


            }

            if (movingLeft < transitionFrameCount)
            {
                float coeficient = (1 - 1 * ((movingLeft + 1) / transitionFrameCount)) / (movingLeft);
                float newScale = scaleSmall + (scaleBig - scaleSmall) * coeficient;
                GameObject.Find("Sprite" + mainSprite).transform.localScale = new Vector3(newScale, newScale, newScale);
                float newX = xL + (xM - xL) * coeficient;
                float newY = yL + (yM - yL) * coeficient;
                GameObject.Find("Sprite" + mainSprite).transform.localPosition = new Vector3(newX, newY, 0);

                movingLeft++;

                newScale = scaleZero + (scaleSmall) * coeficient;
                GameObject.Find("Sprite" + ((mainSprite - 1 + SpritesCount) % SpritesCount)).transform.localScale = new Vector3(newScale, newScale, newScale);

                newScale = scaleBig + (1 - coeficient);
                GameObject.Find("Sprite" + ((mainSprite + 1) % SpritesCount)).transform.localScale = new Vector3(newScale, newScale, newScale);
                newX = xM + (xR - xM) * coeficient;
                newY = yM + (yR - yM) * coeficient;
                GameObject.Find("Sprite" + ((mainSprite + 1) % SpritesCount)).transform.localPosition = new Vector3(newX, newY, 0);

                newScale = scaleSmall + (1 - coeficient);
                GameObject.Find("Sprite" + ((mainSprite + 2) % SpritesCount)).transform.localScale = new Vector3(newScale, newScale, newScale);
            }
            else if (movingLeft == transitionFrameCount)
            {
                movingLeft++;
                mainSprite = (mainSprite + 1) % SpritesCount;
            }

            if (movingRight < transitionFrameCount)
            {
                float coeficient = (1 - 1 * ((movingRight + 1) / transitionFrameCount)) / (movingRight);
                float newScale = scaleSmall + (scaleBig - scaleSmall) * coeficient;
                GameObject.Find("Sprite" + mainSprite).transform.localScale = new Vector3(newScale, newScale, newScale);
                float newX = xR + (xM - xR) * coeficient;
                float newY = yL + (yR - yR) * coeficient;
                GameObject.Find("Sprite" + mainSprite).transform.localPosition = new Vector3(newX, newY, 0);

                movingRight++;

                newScale = scaleZero + (scaleSmall) * coeficient;
                GameObject.Find("Sprite" + ((mainSprite + 1) % SpritesCount)).transform.localScale = new Vector3(newScale, newScale, newScale);

                newScale = scaleBig + (1 - coeficient);
                GameObject.Find("Sprite" + ((mainSprite - 1 + SpritesCount) % SpritesCount)).transform.localScale = new Vector3(newScale, newScale, newScale);
                newX = xM + (xL - xM) * coeficient;
                newY = yM + (yL - yM) * coeficient;
                GameObject.Find("Sprite" + ((mainSprite - 1 + SpritesCount) % SpritesCount)).transform.localPosition = new Vector3(newX, newY, 0);

                newScale = scaleSmall + (1 - coeficient);
                GameObject.Find("Sprite" + ((mainSprite + 2) % SpritesCount)).transform.localScale = new Vector3(newScale, newScale, newScale);
            }
            else if (movingRight == transitionFrameCount)
            {
                movingRight++;
                mainSprite = (mainSprite - 1 + SpritesCount) % SpritesCount;
            }
        }
    }

}


//gameObject.layer = 2;