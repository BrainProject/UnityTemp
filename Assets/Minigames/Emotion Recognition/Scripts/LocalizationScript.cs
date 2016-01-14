using UnityEngine;
using System.Collections;
using SmartLocalization;
using UnityEngine.UI;

namespace EmotionRecognition
{

    public class LocalizationScript : MonoBehaviour
    {
        private string learningKey = "Learning";
        private string gameWithHintKey = "Game with hint";
        private string gameWithoutHintKey = "Game without hint";

        private string language;

        public Text learningText;
        public Text gameWithHintText;
        public Text gameWithoutHintText;

        void Start()
        {
            ChangeLanguage("cs");
        }

        public void ChangeLanguage(string lang)
        {
            LanguageManager.Instance.ChangeLanguage(lang);
            language = lang;
            learningText.text = LanguageManager.Instance.GetTextValue(learningKey);
            gameWithHintText.text = LanguageManager.Instance.GetTextValue(gameWithHintKey);
            gameWithoutHintText.text = LanguageManager.Instance.GetTextValue(gameWithoutHintKey);
        }

        public string getLanguage()
        {
            return language;
        }
    }

}
