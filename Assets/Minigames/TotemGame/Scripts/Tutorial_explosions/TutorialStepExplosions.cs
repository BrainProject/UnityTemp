using UnityEngine;
using System.Collections;

namespace TotemGame
{
    public class TutorialStepExplosions : MonoBehaviour
    {
        public TotemTutorialExplosionStates inStep;
        public ArrowAnimatorExplosions arrow;

        void OnMouseDown()
        {
            if (arrow.currrentState == inStep)
            {
                arrow.ChangeState(this.gameObject);
            }
        }
    }
}
