/**
 * @file ChoosePicturesSetScript.cs
 * @author Ján Bella
 */
using UnityEngine;
using System.Collections;
using System.IO;
using System;

namespace FindIt
{
    /*
     * Logic for choosing set of images for Find It.
     */
	public class ChoosePicturesSetScript : MonoBehaviour
	{
		const int BIG_SIZE = 44;
		const int SMALL_SIZE = 20;

		public string resourcePackName = "Animals";

        private string customResPackPath = "\\CustomImages\\";
        public string[] resPacksNames; 


        public int[] GetMenuDimensions(int elementsCount)
        {
            if (elementsCount == 1)
            {
                return new int[] { 1, 1 };
            }
            else if (elementsCount == 2)
            {
                return new int[] { 1, 2 };
            }
            else if (elementsCount <= 4)
            {
                return new int[] { 2, 2 };
            }
            else if (elementsCount <= 6)
            {
                return new int[] { 2, 3 };
            }
            else if (elementsCount <= 8)
            {
                return new int[] { 2, 4 };
            }
            else if (elementsCount <= 9)
            {
                return new int[] { 3, 3 };
            }
            else //if (elementsCount <= 12)
            {
                return new int[] { 3, 4 };
            }
            //else
            //{
            //    return new int[] { 4, 4 };
            //}
        }

        private int GetCustomResroucePacksCount()
        {
            //sort of tests, if directories exists
            Directory.CreateDirectory(Environment.CurrentDirectory + customResPackPath);
            Directory.CreateDirectory(Environment.CurrentDirectory + customResPackPath + "\\FindIt" );



            return Directory.GetDirectories(Environment.CurrentDirectory + customResPackPath + "FindIt\\").Length;
        }

        private IEnumerator LoadCustomResourcePacks(int count)
        {
            string[] customResourcePacks = Directory.GetDirectories(Environment.CurrentDirectory + customResPackPath + "FindIt\\");

            //Run from 0 to number of menu items minus default resource packs
            for (int i = 0; i < count; i++)
            {
                //while we have some resource packs left
                if (i < customResourcePacks.Length)
                {
                    //set name of game-object (will be used later as chosen resource-pack identifier]
                    string[] s = customResourcePacks[i].Split('\\');
                    Debug.Log("Custom resource pack name: '" + s[s.Length - 1] + "'");
                    gameTiles[i + resPacksNames.Length].name = "[CUSTOM]" + s[s.Length - 1];

                    //use first image in pack as tile texture
                    Debug.Log("file://" + customResourcePacks[i] + "\\00.png");
                    WWW www = new WWW("file://" + customResourcePacks[i] + "\\00.png");
                    yield return www;
                    gameTiles[i + resPacksNames.Length].transform.GetChild(0).renderer.material.mainTexture = www.texture;
                }
                //destroy tiles without resource packs 
                else
                {
                    Destroy(gameTiles[i + resPacksNames.Length]);
                }
            }
        }

        private bool directoryHasEnoughImages(string directory, string demandedCount)
        {
            return true;
        }

        void Start()
        {
            string resPackPath = "Textures/Pictures/FindIt/";

            //Number of default resource packs
            int menuLength = resPacksNames.Length;

            //Check for custom resource packs  
            #if UNITY_STANDALONE_WIN
                menuLength += GetCustomResroucePacksCount();
                Debug.Log("Running on WIN, found " + menuLength + " resrouce packs (" + resPacksNames.Length + " are default)");
            #endif

            //Calculate number of menu items
            int[] menu = GetMenuDimensions(menuLength);
            Debug.Log("Menu dimensions:" + menu[0] + "x" + menu[1]);
            menuRows = menu[0];
            menuColumns = menu[1];

            //Create menu
            CreateResourcePacksIcons();
        }

        public void CreateResourcePacksIcons()
        {
            gameTiles = GameTiles.createTiles(menuRows, menuColumns, gameTilePrefab, "PicMenuItem");

            if (mainGameScript.enabled)
            {
                mainGameScript.enabled = false;
            }

            int resPackCount = resPacksNames.Length;
            print("Number of standard res. packs: " + resPackCount);
            print("Number of game tiles: " + gameTiles.Length);

            for (int i = 0; i < menuColumns * menuRows; i++)
            {
                //while we have some resource packs left
                if (i < resPackCount)
                {
                    print("Resource pack name: '" + resPacksNames[i] + "'");

                    //set name of game-object (will be used later as chosen resource-pack identifier]
                    //string[] s = resourcePacks[i].Split('\\');
                    gameTiles[i].name = resPacksNames[i];

                    //use first image in pack as tile texture
                    gameTiles[i].transform.GetChild(0).renderer.material.mainTexture = Resources.Load(resPackPath + resPacksNames[i] + "/00") as Texture2D;
                }
            }

            //Add custom resoruce packs
#if UNITY_STANDALONE_WIN
            StartCoroutine(LoadCustomResourcePacks());
#endif
        }






		// Use this for initialization
		void OnMouseDown()
		{
			PlayerPrefs.SetString("resourcePackName", resourcePackName);
            MGC.Instance.sceneLoader.LoadScene("FindItGame", true);
		}
		
		public bool checkResourcePackForEnoughImages(int numberImages)
		{
			Sprite[] images = Resources.LoadAll<Sprite>(resourcePackName);
			//Debug.Log ("Resource pack " + resourcePackName + " loaded " + images.Length + " images.");
			return (images.Length >= numberImages);
		}
	}
}