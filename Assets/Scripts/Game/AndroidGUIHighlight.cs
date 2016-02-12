using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Game
{
    public class AndroidGUIHighlight : MonoBehaviour
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        void Start()
        {
            Button modifiedButton = GetComponent<Button>();
            SpriteState highlightedSprite = modifiedButton.spriteState;
            highlightedSprite.highlightedSprite = null;
            modifiedButton.spriteState = highlightedSprite;
        }
#endif
    }
}