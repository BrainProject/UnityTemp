using UnityEngine;

/**
 * Newron minigame - TotemGame
 *
 * @author Petra Ambrozkova
 */
namespace TotemGame
{
    public class TutorialStep : MonoBehaviour
    {
        public TotemTutorialStates inStep;
        public ArrowAnimator arrow;

        void OnMouseDown()
        {
            if (arrow.currrentState == inStep)
            {
                arrow.ChangeState(this.gameObject);
            }
        }
    }
}
