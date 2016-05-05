using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TotemGame
{
    public class DropdownScript : MonoBehaviour
    {
        private string filesPath;
        private string fieldText;
        public Dropdown myDropdown;

        void Start()
        {
            FillDropdown();
        }

        public void FillDropdown()
        {
            filesPath = Application.dataPath + "/XmlDocs";
            DirectoryInfo dir = new DirectoryInfo(filesPath);
            FileInfo[] info = dir.GetFiles("*.xml");
            int x = info.Length;

            for (int i = 0; i < x - 1; i++)
            {
                fieldText = info.GetValue(i).ToString();
                myDropdown.options.Add(new Dropdown.OptionData(fieldText));
            }
        }
    }
}