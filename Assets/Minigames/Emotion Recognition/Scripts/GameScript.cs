using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.UI;


namespace EmotionRecognition
{
    public class GameScript : MonoBehaviour
    {

        public EmotionRecognition.GameSetup.GameType gameType;

        // Textures of images with emotional expressions
        //Texture2D[] faceTextures;

        // Emotions from image EXIF data
        //string[] faceEmotions;

        // Path of the images 
        //private string faceImagesPath;

        //Piktogramy emócii
        public Sprite anger;
        public Sprite fear;
        public Sprite sadness;
        public Sprite happiness;
        public Sprite disgust;
        public Sprite surprise;


        private int currentFace = 0;

        //Dictionary that stores faces as sprites and amotions as string
        private Dictionary<Sprite, string> faces;

        private Dictionary<Sprite, string> used;

        //Canvas of learning phase of the game
        public Canvas learningCanvas;

        //Canvas of game phase
        public Canvas gameCanvas;

        //Represents name of the correct button
        private string correctButton;

        //Stores whether correct button is pressed first
        public bool first;

        //Sprites of star ratings
        public Sprite[] starSprites;

        //Temp
        public Canvas ratingCanvas;


        private Sprite ObjectToSprite(Object obj)
        {
            Texture2D temp = obj as Texture2D;
            return Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0.5f, 0.5f));
        }

        private void LoadFaceImages()
        {
            faces = new Dictionary<Sprite, string>();

            Object[] tempA = Resources.LoadAll("Faces/Anger");
            foreach (Object temp in tempA)
            {
                faces.Add(ObjectToSprite(temp), "Anger");
            }
            Object[] tempD = Resources.LoadAll("Faces/Disgust");
            foreach (Object temp in tempD)
            {
                faces.Add(ObjectToSprite(temp), "Disgust");
            }
            Object[] tempF = Resources.LoadAll("Faces/Fear");
            foreach (Object temp in tempF)
            {
                faces.Add(ObjectToSprite(temp), "Fear");
            }
            Object[] tempSu = Resources.LoadAll("Faces/Surprise");
            foreach (Object temp in tempSu)
            {
                faces.Add(ObjectToSprite(temp), "Surprise");
            }
            Object[] tempSa = Resources.LoadAll("Faces/Sadness");
            foreach (Object temp in tempSa)
            {
                faces.Add(ObjectToSprite(temp), "Sadness");
            }
            Object[] tempH = Resources.LoadAll("Faces/Happiness");
            foreach (Object temp in tempH)
            {
                faces.Add(ObjectToSprite(temp), "Happiness");
            }            

            //faceImagesPath = "C:\\Users\\Wrath\\Desktop\\faces";
            //string[] faceImages = System.IO.Directory.GetFiles(faceImagesPath, "*.jpg");
            //Object[] faceImages = Resources.LoadAll("Faces/");

            //faceTextures = new Texture2D[faceImages.Length];
            //faceEmotions = new string[faceImages.Length];
            
            //for (int i = 0; i < faceImages.Length; i++)
            //{
                
                //Getting Exif info from image
                //ImageFile file = ImageFile.FromFile(Directory.GetCurrentDirectory() + "/Assets/Resources/Faces/" + faceImages[i].name + ".jpg");
                
                //foreach (ExifProperty item in file.Properties.Values)
                //{
                //    if (item.Name.Equals("UserComment"))
                //    {
                //        faceEmotions[i] = item.Value.ToString();
                //    }
                //}
                
                // Getting texture from image

                //WWW www = new WWW("file://" + Application.dataPath + "/Assets/Resources/Faces/" + faceImages[i].name + ".jpg");
                //yield return www;
                //Texture2D texTmp = new Texture2D(1024, 1024, TextureFormat.DXT1, false);
                //www.LoadImageIntoTexture(texTmp);
                //faceTextures[i] = texTmp;
            //    faceTextures[i] = faceImages[i] as Texture2D;
            //}

            Debug.Log(faces.Count + " images loaded");
        }
        void ShuffleDictionary()
        {
            System.Random rand = new System.Random();
            faces = faces.OrderBy(x => rand.Next()).ToDictionary(item => item.Key, item => item.Value);
        }

        public string GetCorrectButton()
        {
            return correctButton;
        }

        bool CheckEndGame() 
        {
            if (faces.Count < 2)
                return true;
            else if (faces.Count == 2 && (string.Equals(faces.ElementAt(0).Value, faces.ElementAt(1).Value)))
                return true;
            else return false;
        }

        void AssignFaceImage(Image[] images, int number, string buttonName)
        {
            foreach (Image img in images)
            {
                if (img.name == buttonName)
                {
                    img.sprite = faces.ElementAt(number).Key;
                }
            }
        }

        void AssignPictogramAndText(Image[] images, int number, Canvas canvas)
        {
            foreach (Image img in images)
            {
                switch (faces.ElementAt(number).Value)
                {
                    case "Anger":
                        {
                            if (img.name == "PictogramImage")
                            {
                                img.sprite = anger;
                            }
                            break;
                        }
                    case "Fear":
                        {
                            if (img.name == "PictogramImage")
                            {
                                img.sprite = fear;
                            }
                            break;
                        }
                    case "Happiness":
                        {
                            if (img.name == "PictogramImage")
                            {
                                img.sprite = happiness;
                            }
                            break;
                        }
                    case "Sadness":
                        {
                            if (img.name == "PictogramImage")
                            {
                                img.sprite = sadness;
                            }
                            break;
                        }
                    case "Surprise":
                        {
                            if (img.name == "PictogramImage")
                            {
                                img.sprite = surprise;
                            }
                            break;
                        }
                    case "Disgust":
                        {
                            if (img.name == "PictogramImage")
                            {
                                img.sprite = disgust;
                            }
                            break;
                        }
                }
            }
            canvas.GetComponentInChildren<Text>().text = faces.ElementAt(number).Value;
        }

        void AssignColor(RawImage[] images,int number, string rawImageName)
        {
            foreach (RawImage img in images)
            {
                if (img.name == rawImageName)
                {
                    switch (faces.ElementAt(number).Value)
                    {
                        case "Anger":
                            {
                                img.color = new Color32(185, 20, 20, 255);
                                break;
                            }
                        case "Fear":
                            {
                                img.color = new Color32(0, 23, 175, 255);
                                break;
                            }
                        case "Happiness":
                            {
                                img.color = new Color32(253, 201, 120, 255);
                                break;
                            }
                        case "Sadness":
                            {
                                img.color = Color.black;
                                break;
                            }
                        case "Surprise":
                            {
                                img.color = new Color32(175, 141, 217, 255);
                                break;
                            }
                        case "Disgust":
                            {
                                img.color = new Color32(126, 75, 28, 255);
                                break;
                            }
                    }
                }
            }
        }

        IEnumerator ShowRating(Image img,int stars)
        {
            gameCanvas.enabled = false;
            ratingCanvas.enabled = true;
            for (int i = 0; i < stars + 1; i++)
            {
                img.sprite = starSprites[i];
                yield return new WaitForSeconds(0.1f);
            }
            GameStatistics.CorrectGameTurns = 0;  
        }
        
        public void GameTurn()
        {

            if (currentFace < faces.Count - 1)
            {
                currentFace++;
            }
            else
            {
                currentFace = 0;
            }
          
            if (gameType == GameSetup.GameType.Learning)
            {
                ChangeImage(currentFace);
            }
            else
            {
                if (!CheckEndGame())
                {
                    used = new Dictionary<Sprite, string>();
                    System.Random random = new System.Random();
                    int chosenone = random.Next(0, faces.Count);
                    int chosentwo = random.Next(0, faces.Count);
                    first = true;

                    while (string.Equals(faces.ElementAt(chosenone).Value, faces.ElementAt(chosentwo).Value))
                    {
                        //chosenone = random.Next(0, faces.Count);
                        chosenone++;
                        if (chosenone == faces.Count) chosenone = 0;
                    }
                    //Debug.Log(chosenone + "  " + chosentwo);

                    AssignFaceImage(gameCanvas.GetComponentsInChildren<Image>(), chosenone, "GameButton1");
                    AssignFaceImage(gameCanvas.GetComponentsInChildren<Image>(), chosentwo, "GameButton2");
                    AssignColor(gameCanvas.GetComponentsInChildren<RawImage>(), chosenone, "ColorImage1");
                    AssignColor(gameCanvas.GetComponentsInChildren<RawImage>(), chosentwo, "ColorImage2");

                    int correct = ChooseCorrect(chosenone, chosentwo);


                    if(correct == chosenone)
                    {
                        correctButton = "GameButton1";
                    }
                    else correctButton = "GameButton2";
 
                    AssignPictogramAndText(gameCanvas.GetComponentsInChildren<Image>(), correct, gameCanvas);

                    used.Add(faces.ElementAt(chosenone).Key, faces.ElementAt(chosenone).Value);
                    used.Add(faces.ElementAt(chosentwo).Key, faces.ElementAt(chosentwo).Value);
                    faces.Remove(faces.ElementAt(chosenone).Key);
                    faces.Remove(faces.ElementAt(chosentwo).Key);
                    if (GameStatistics.GameTurns % 10 == 0 && GameStatistics.GameTurns > 0)
                    {
                        StartCoroutine(ShowRating(ratingCanvas.GetComponentInChildren<Image>(), GameStatistics.CorrectGameTurns));
                    }
                    GameStatistics.GameTurns += 1;
                }
            }
        }

        public int ChooseCorrect(int left,int right)
        {
            System.Random random = new System.Random();
            if (random.Next(2) == 0)
            {
                return left;
            }
            else return right;
        }

        public void ChangeImage(int current)
        {
            AssignColor(learningCanvas.GetComponentsInChildren<RawImage>(), current, "ColorImage");
            AssignPictogramAndText(learningCanvas.GetComponentsInChildren<Image>(), current, learningCanvas);
            AssignFaceImage(learningCanvas.GetComponentsInChildren<Image>(), current, "LearningButton");
        }

        void Awake()
        {
           //StartCoroutine(LoadFaceImages());
           LoadFaceImages();
        }
      
        void Start()
        {
            ShuffleDictionary();
            SelectedPhase();
            GameTurn();
        }

        public void SelectedPhase()
        {
            switch(gameType)
            {
                case GameSetup.GameType.Learning:
                    {
                        learningCanvas.enabled = true;
                        break;
                    }
                case GameSetup.GameType.GameWithHint:
                    {
                        gameCanvas.enabled = true;
                        break;
                    }
                case GameSetup.GameType.GameWithoutHint:
                    {
                        foreach (RawImage img in gameCanvas.GetComponentsInChildren<RawImage>())
                        {
                            img.enabled = false;
                        }
                        gameCanvas.enabled = true;
                        break;
                    }
            }
        }

        void OnGUI()
        {
            //if (Input.GetMouseButtonDown(0))
            //{
            //    if (currentFace < faces.Count - 1)
            //    {
            //        currentFace++;
            //        ChangeImage(currentFace);
            //    }
            //    else
            //    {
            //        currentFace = 0;
            //        ChangeImage(currentFace);
            //    }
            //}
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(ratingCanvas.enabled == true)
                {
                    ratingCanvas.enabled = false;
                    gameCanvas.enabled = true;
                }
            }
        }
    }
}
