/**
 * @file ChoosePictureSetup.cs
 * @author Ján Bella
 */
using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Puzzle
{
    /**
     * Creates tiles to choose image set for playing the game
     */
    public class ChoosePictureSetup : MonoBehaviour
    {
        // constants deremine the number of images needed, also defines a difficulty
        const int BIG_SIZE = 44;
        const int SMALL_SIZE = 20;

        // one of BIG_SIZE or SMALL_SIZE. Number of images needed for current game
        private int numberPieces;

        // custom resource packs path 
        private string customResPackPath;
        
        // default resource packs array
        public string defaultPicturesPath = "Pictures";

        public GameObject TilePrefab;

        /**
         * Directory.GetFiles() returns files, where folder is 
         * 
         */
        private string repairPath(string s)
        {
            string c = "";
            for(int i=0;i<s.Length;i++)
            {
                char v = s.ElementAt(i);
                if (v == '/')
                    c = c + '\\';
                else c += v;
            }
            return c;
        }

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
        private IEnumerable<string> LoadCustomResourcePacks()
        {
            Directory.CreateDirectory(customResPackPath);

            return Directory.GetFiles(customResPackPath).Where(
                    p => Path.GetExtension(p).ToLower() == ".png" || Path.GetExtension(p).ToLower() == ".jpg" ||
                         Path.GetExtension(p).ToLower() == ".jpeg" || Path.GetExtension(p).ToLower() == ".bmp" ||
                         Path.GetExtension(p).ToLower() == ".gif" || Path.GetExtension(p).ToLower() == ".tif");    
        }

        void Start()
        {
            //MGC.Instance.selectedMiniGameDiff = 0;

            Texture2D[] defaultPics = Resources.LoadAll<Texture2D>(defaultPicturesPath);

            IEnumerable<string> customPics;

            customResPackPath = MGC.Instance.getPathtoCustomImageSets() + "Puzzle/";

            //TODO this should works on all platforms

            //Check for custom resource packs
        #if !UNITY_WEBPLAYER
            customPics = LoadCustomResourcePacks();

            //Calculate number of menu items
            int[] menu = GetMenuDimensions(customPics.Count() + defaultPics.Length);

            //Create menu
            CreateResourcePacksIcons(menu[0], menu[1],defaultPics,customPics);    
        #endif


        }

    /**
     * Places icons for choosing resource packs into the scene
     * @param menuRows number of rows of the grid for images
     * @param menuColumns number of columns of the grid for images
     */
    private void CreateResourcePacksIcons(int menuRows, int menuColumns, Texture2D[] defaultPics, IEnumerable<string> customPics)
    {
        // defines borders in the scene
        /*const*/ float minx = -5.7f; // -6.5f;
        /*const*/ float maxx = 5.7f; // 6.5f;
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

        for (i = menuRows - 1; i >= 0 && (menuRows - 2 - i) * menuColumns + (menuColumns - 1 - j) < defaultPics.Length; i--)
        {
            for (j = menuColumns - 1; j >= 0 && (menuRows - 1 - i) * menuColumns + (menuColumns - 1 - j) < defaultPics.Length; j--)
            {

                GameObject g = Instantiate(TilePrefab) as GameObject;

                g.transform.localPosition = new Vector3(
                     menuColumns - 1 == 0 ? 0 : ((maxx - minx) / (menuColumns - 1)) * j + minx,
                     menuRows - 1 == 0 ? 0 : ((maxy - miny) / (menuRows - 1)) * i + miny,
                     0.0f);
                if (menuRows > 2)
                    g.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                else g.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);

                g.GetComponent<MeshRenderer>().material.mainTexture = defaultPics[(menuRows - 1 - i) * menuColumns + (menuColumns - 1 - j)];
                
                ChoosePictureScript chps = g.AddComponent<ChoosePictureScript>();
                chps.custom = false;
                chps.name = defaultPics[(menuRows - 1 - i) * menuColumns + (menuColumns - 1 - j)].name;
                chps.imageName = defaultPics[(menuRows - 1 - i) * menuColumns + (menuColumns - 1 - j)].name;
                chps.defaultPicsPath = defaultPicturesPath;
            }
        }

        //Add custom resoruce packs
        #if UNITY_STANDALONE_WIN
        i++;
        while (i >= 0 && (menuRows - 1 - i) * menuColumns + (menuColumns - 1 - j) - defaultPics.Length < customPics.Count())
        {
            while (j >= 0 && (menuRows - 1 - i) * menuColumns + (menuColumns - 1 - j) - defaultPics.Length < customPics.Count())
            {
                Debug.Log("file:///" + /*repairPath*/(customPics.ElementAt<string>((menuRows - 1 - i) * menuColumns + (menuColumns - 1 - j) - defaultPics.Length)));
                WWW www = new WWW("file:///" + /*repairPath*/(customPics.ElementAt<string>((menuRows - 1 - i) * menuColumns + (menuColumns - 1 - j) - defaultPics.Length)));

                GameObject g = Instantiate(TilePrefab) as GameObject;

                g.transform.localPosition = new Vector3(
                    ((maxx - minx)/(menuColumns-1))*j + minx,
                    ((maxy - miny)/(menuRows-1))*i + miny,
                     0.0f);
                if (menuRows > 2)
                    g.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                else g.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);

                g.GetComponent<MeshRenderer>().material.mainTexture = www.texture;
                
                ChoosePictureScript chps = g.AddComponent<ChoosePictureScript>();
                chps.custom = true;
                chps.name = www.texture.name;
                chps.imageName = customPics.ElementAt<string>((menuRows - 1 - i) * menuColumns + (menuColumns - 1 - j) - defaultPics.Length);
                chps.defaultPicsPath = defaultPicturesPath;
                j--;
            }
            j = menuColumns -1;
            i--;
        }
        #endif
    }
    }
}
