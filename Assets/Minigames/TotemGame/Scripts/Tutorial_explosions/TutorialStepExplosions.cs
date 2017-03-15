using UnityEngine;

/**
 * Newron minigame - TotemGame
 *
 * @author Petra Ambrozkova
 */
namespace TotemGame
{
    public class TutorialStepExplosions : MonoBehaviour
    {
        public Rigidbody thisRigidbody;
        public TotemTutorialExplosionStates inStep;
        public ArrowAnimatorExplosions arrow;

        private void Start()
        {
            if (!thisRigidbody)
            {
                Rigidbody tmp = GetComponent<Rigidbody>();
                thisRigidbody = tmp;
                if (!thisRigidbody)
                {
                    Debug.LogWarning(gameObject.name + " doesn't have any rigidbody!");
                    enabled = false;
                }
            }

            if (thisRigidbody)
            {
                thisRigidbody.isKinematic = true;
            }
        }

        void OnEnable()
        {
            TotemLevelManager.OnClicked += ActivatePhysics;
        }

        void OnDisable()
        {
            TotemLevelManager.OnClicked -= ActivatePhysics;
        }

        void ActivatePhysics()
        {
            if (thisRigidbody)
            {
                thisRigidbody.isKinematic = false;
            }
        }

        void OnMouseDown()
        {
            if (arrow.currrentState == inStep)
            {
                arrow.ChangeState(this.gameObject);
            }
        }
    }
}
