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
        private string customResPackPath = "\\CustomImages\\";
        
        // default resource packs array
        public string defaultPicturesPath = "Pictures";

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
        private IEnumerable<string> LoadCustomResourcePacks()
        {
            //sort of tests, if directories exists
            Directory.CreateDirectory(Environment.CurrentDirectory + customResPackPath);
            Directory.CreateDirectory(Environment.CurrentDirectory + customResPackPath + "\\Puzzle");

            return Directory.GetFiles(Environment.CurrentDirectory + customResPackPath + "Puzzle\\").Where(
                    p => Path.GetExtension(p).ToLower() == ".png" || Path.GetExtension(p).ToLower() == ".jpg" ||
                         Path.GetExtension(p).ToLower() == ".jpeg" || Path.GetExtension(p).ToLower() == ".bmp" ||
                         Path.GetExtension(p).ToLower() == ".gif" || Path.GetExtension(p).ToLower() == ".tif");    
        }

        void Start()
        {
            Texture2D[] defaultPics = Resources.LoadAll<Texture2D>(defaultPicturesPath);

            IEnumerable<string> customPics;
            //Check for custom resource packs
            #if UNITY_STANDALONE_WIN
                customPics = LoadCustomResourcePacks();
                
            #endif

            //Calculate number of menu items
                int[] menu = GetMenuDimensions(customPics.Count() + defaultPics.Length);

            //Create menu
            CreateResourcePacksIcons(menu[0], menu[1],defaultPics,customPics);
        }

    /**
     * Places icons for choosing resource packs into the scene
     * @param menuRows number of rows of the grid for images
     * @param menuColumns number of columns of the grid for images
     */
    private void CreateResourcePacksIcons(int menuRows, int menuColumns, Texture2D[] defaultPics, IEnumerable<string> customPics)
    {
        // defines borders in the scene
        const float minx = -6.5f;
        const float maxx = 6.5f;
        const float miny = -2.75f;
        const float maxy = 2.75f;

        //int numberPacks = customResPacks.Count + defResPacks.Count;
        int i, j = 0;

        Debug.Log("Grid size chosen as " + menuRows + " x " + menuColumns);

        for (i = menuRows - 1; i >= 0 && (menuRows - 2 - i) * menuColumns + j < defaultPics.Length; i--)
        {
            for (j = 0; j < menuColumns && (menuRows - 1 - i) * menuColumns + j < defaultPics.Length; j++)
            {

                GameObject g = Instantiate(TilePrefab) as GameObject;

                g.transform.localPosition = new Vector3(
                     menuColumns - 1 == 0 ? 0 : ((maxx - minx) / (menuColumns - 1)) * j + minx,
                     menuRows - 1 == 0 ? 0 : ((maxy - miny) / (menuRows - 1)) * i + miny,
                     0.0f);
                if (menuRows > 2)
                    g.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                else g.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);

                g.GetComponent<MeshRenderer>().material.mainTexture = defaultPics[(menuRows - 1 - i) * menuColumns + j];
                
                ChoosePictureScript chps = g.AddComponent<ChoosePictureScript>();
                chps.custom = false;
                chps.name = defaultPics[(menuRows - 1 - i) * menuColumns + j].name;
                chps.imageName = defaultPics[(menuRows - 1 - i) * menuColumns + j].name;
                chps.defaultPicsPath = defaultPicturesPath;
            }
        }

        //Add custom resoruce packs
        #if UNITY_STANDALONE_WIN
        i++;
        while (i >= 0 && (menuRows - 1 - i) * menuColumns + j - defaultPics.Length < customPics.Count())
        {
            while (j < menuColumns && (menuRows - 1 - i) * menuColumns + j - defaultPics.Length < customPics.Count())
            {
                WWW www = new WWW("file://" + customPics.ElementAt<string>((menuRows - 1 - i) * menuColumns + j - defaultPics.Length));

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
                chps.imageName = customPics.ElementAt<string>((menuRows - 1 - i) * menuColumns + j - defaultPics.Length);
                chps.defaultPicsPath = defaultPicturesPath;
                j++;
            }
            j = 0;
            i--;
        }
        #endif
    }
    }
}
