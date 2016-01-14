using UnityEngine;
using System.Collections;
using System.IO;

namespace EmotionRecognition
{
    public class GameSetup : MonoBehaviour
    {

        public enum GameType { Learning, GameWithHint, GameWithoutHint };

        public enum Emotions { Anger, Fear, Sadness, Hapiness, Disgust, Surprise };

        public GameType currentGame;

        // GameScript
        public GameScript gameScript;

        public Canvas setupCanvas;
        void Start()
        {
            int diff = MGC.Instance.selectedMiniGameDiff;

            switch (diff)
            {
                case 0:
                    SelectedLearning();
                    break;

                case 1:
                    SelectedGameWithHint();
                    break;

                case 2:
                    SelectedGameWithoutHint();
                    break;

            }
                
        }

        
        void Update()
        {

        }

        public void SelectedLearning()
        {
            currentGame = GameType.Learning;
            gameScript.gameType = currentGame;
            setupCanvas.enabled = false;
            gameScript.enabled = true;
        }

        public void SelectedGameWithHint()
        {
            currentGame = GameType.GameWithHint;
            gameScript.gameType = currentGame;
            setupCanvas.enabled = false;
            gameScript.enabled = true;
        }

        public void SelectedGameWithoutHint()
        {
            currentGame = GameType.GameWithoutHint;
            gameScript.gameType = currentGame;
            setupCanvas.enabled = false;
            gameScript.enabled = true;
        }

        //private IEnumerator LoadFaceImages()
        //{

        //    string[] faceImages = System.IO.Directory.GetFiles(faceImagesPath, "*.jpg");
        //    faceTextures = new Texture2D[faceImages.Length];

        //    gameObj = GameObject.FindGameObjectsWithTag("Pics");

        //    for (int i = 0; i < faceImages.Length; i++)
        //    {
        //        string pathTemp = pathPreFix + faceImages[i];
        //        WWW www = new WWW(pathTemp);
        //        yield return www;
        //        Texture2D texTmp = new Texture2D(1024, 1024, TextureFormat.DXT1, false);
        //        www.LoadImageIntoTexture(texTmp);
        //        faceTextures[i] = texTmp;
        //        gameObj[i].GetComponent<Renderer>().material.SetTexture("_MainTex", texTmp);
        //    }
        //}
    }
}
