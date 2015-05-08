/**
 * @file ImageSetsSetup.cs
 * @author Ján Bella
 */
using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FindIt
{
    /**
     * Creates tiles to choose image set for playing the game
     */
    public class ImageSetsSetup : MonoBehaviour
    {
        // constants deremine the number of images needed, also defines a difficulty
        const int BIG_SIZE = 44;
        const int SMALL_SIZE = 20;

        // one of BIG_SIZE or SMALL_SIZE. Number of images needed for current game
        private int numberPieces;

        // custom resource packs path 
        private string customResPackPath;
        
        // default resource packs array
        public string[] resPacksNames;

        // lists store indentifiers of those resource packs, that contain enough images. 
        private List<string> defResPacks = new List<string>();
        private List<string> customResPacks = new List<string>();

        public GameObject TilePrefab;

        /**
         * Returns proper dimensions of the grid to put the tiles
         * @ param elementsCount Number of elements to fit in the grid
         * @ array containing number of rows, number of columns
         */
        public int[] GetMenuDimensions(int elementsCount)
        {
            if (elementsCount <= 4)
            {
                return new int[] { 1, elementsCount };
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
            else if (elementsCount <= 12)
            {
                return new int[] { 3, 4 };
            }
            else
            {
                return new int[] { 3, 5 };
            }
        }

        /**
         * checks custom resource pack folder for any resource packs, containing the number of images needed for the game. 
         * Fills customResPacks list.
         * @param demandedCount number of images needed in the resource pack
         */
        private void CheckCustomResourcePacks(int demandedCount)
        {
            //sort of tests, if directories exists
            Directory.CreateDirectory(customResPackPath);

            string[] customResourcePacks = Directory.GetDirectories(customResPackPath);
            print("Founded custom image sets: " + customResourcePacks.Count());

            for (int i = 0; i < customResourcePacks.Count(); i++)
            {
                if (checkCustomForEnoughImages(demandedCount, customResourcePacks[i]))
                {
                    customResPacks.Add(customResourcePacks[i]);
                }
            }
        }

        /**
         * Checks given directory, whether it contains enough images
         * @param demandedCount number of images needed in the resource pack
         * @param directory path to a directory with images
         * @return true if number of images in the folder is sufficient
         */
        private bool checkCustomForEnoughImages(int demandedCount, string directory)
        {
            var allFilenames = Directory.GetFiles(directory).Where(
                    p => Path.GetExtension(p).ToLower() == ".png" || Path.GetExtension(p).ToLower() == ".jpg" ||
                         Path.GetExtension(p).ToLower() == ".jpeg" || Path.GetExtension(p).ToLower() == ".bmp" ||
                         Path.GetExtension(p).ToLower() == ".gif" || Path.GetExtension(p).ToLower() == ".tif");
            if(allFilenames.Count() < demandedCount)
            {
                print("Image set: '" + directory + "', contains only: " + allFilenames.Count() + " images. Required: " + demandedCount);
                return false;
            }

            return true;
        }

        /**
         * Checks whether default resource pack contains enough images
         * @param numberImages number of images needed in the resource pack
         * @param resourcePackName name of the resource pack, also name the folder to search in
         */
        public bool checkDefaultForEnoughImages(int numberImages, string resourcePackName)
        {
            return Resources.LoadAll<Texture2D>(resourcePackName).Length >= numberImages;
        }


        void Start()
        {
            numberPieces = 0;
            switch (MGC.Instance.selectedMiniGameDiff)
            {
                case 0:
                    numberPieces = 20;
                    break;
                case 1:
                    numberPieces = 44;
                    break;
            }

            //Check for default resource packs
            for (int i = 0; i < resPacksNames.Length; i++)
            {
                if (checkDefaultForEnoughImages(numberPieces, resPacksNames[i]))
                {
                    defResPacks.Add(resPacksNames[i]);
                }
            }

            customResPackPath = MGC.Instance.getPathtoCustomImageSets() + "/FindIt";

            //TODO solve this for Android platform

            //Check for custom resource packs
#if UNITY_STANDALONE_WIN
            CheckCustomResourcePacks(numberPieces);
#endif

            //Calculate number of menu items
            int[] menu = GetMenuDimensions(defResPacks.Count + customResPacks.Count);

            //Create menu
            CreateResourcePacksIcons(menu[0], menu[1]);
        }

    /**
     * Places icons for choosing resource packs into the scene
     * @param menuRows number of rows of the grid for images
     * @param menuColumns number of columns of the grid for images
     */
    private void CreateResourcePacksIcons(int menuRows, int menuColumns)
    {
        System.Random r = new System.Random();

        // defines borders in the scene
        /*const*/ float minx = -5.7f; // -6.5f;
        /*const*/ float maxx = 5.7f;  // 6.5f;
        const float miny = -2.75f;
        const float maxy = 2.75f;

        //int numberPacks = customResPacks.Count + defResPacks.Count;
        int i, j = 0;

        Debug.Log("Grid size chosen as " + menuRows + " x " + menuColumns);

        if (menuColumns == 2)
        {
            minx += 2;
            maxx -= 2;
        }

        for (i = menuRows - 1; i >= 0 && (menuRows - 2 - i) * menuColumns + j < defResPacks.Count; i--)
        {
            for (j = 0; j < menuColumns && (menuRows - 1 - i) * menuColumns + j < defResPacks.Count; j++)
            {
                GameObject g = Instantiate(TilePrefab) as GameObject;

                g.transform.localPosition = new Vector3(
                    menuColumns - 1 == 0 ? 0 : ((maxx - minx) / (menuColumns - 1)) * j + minx,
                    menuRows - 1 == 0 ? 0 : ((maxy - miny) / (menuRows - 1)) * i + miny,
                    0.0f);
                if (menuRows > 2)
                    g.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                else g.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);

                g.renderer.material.mainTexture = Resources.LoadAll<Texture2D>(defResPacks[(menuRows - 1 - i) * menuColumns + j])[0];

                ChooseImageSet chis = g.AddComponent<ChooseImageSet>();
                chis.custom = false;
                chis.resourcePackName = defResPacks[(menuRows - 1 - i) * menuColumns + j];
                chis.name = chis.resourcePackName;
            }
        }

        //Add custom resoruce packs
        #if UNITY_STANDALONE_WIN
        i++;
        while (i >= 0 && (menuRows - 1 - i) * menuColumns + j - defResPacks.Count < customResPacks.Count)
        {
            while (j < menuColumns && (menuRows - 1 - i) * menuColumns + j - defResPacks.Count < customResPacks.Count)
            {
                string file = Directory.GetFiles(customResPacks[(menuRows - 1 - i) * menuColumns + j - defResPacks.Count]).Where(
                   p => Path.GetExtension(p).ToLower() == ".png" || Path.GetExtension(p).ToLower() == ".jpg" ||
                        Path.GetExtension(p).ToLower() == ".jpeg" || Path.GetExtension(p).ToLower() == ".bmp" ||
                        Path.GetExtension(p).ToLower() == ".gif" || Path.GetExtension(p).ToLower() == ".tif").ElementAt<string>(/*r.Next(numberPieces)*/0);

                GameObject g = Instantiate(TilePrefab) as GameObject;

                g.transform.localPosition = new Vector3(
                    menuColumns - 1 == 0 ? 0 : ((maxx - minx) / (menuColumns - 1)) * j + minx,
                    menuRows - 1 == 0 ? 0 : ((maxy - miny) / (menuRows - 1)) * i + miny,
                    0.0f);
                if (menuRows > 2)
                    g.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                else g.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);

                WWW www = new WWW("file:///" + file);
                g.renderer.material.mainTexture = www.texture;

                ChooseImageSet chis = g.AddComponent<ChooseImageSet>();
                chis.custom = true;
                chis.resourcePackName = customResPacks[(menuRows - 1 - i) * menuColumns + j - defResPacks.Count];
                //chis.name = customResPacks[(menuRows - 1 - i) * menuColumns + j - defResPacks.Count];
                
                j++;
            }
            j = 0;
            i--;
        }
        #endif
    }
    }
}
