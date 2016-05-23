using UnityEngine;

/**
 * Newron minigame - TotemGame
 *
 * @author Petra Ambrozkova
 */
namespace TotemGame
{
    public enum TotemTutorialStates
    {
        FIRST,
        SECOND,
        THIRD,
        FOURTH,
        FIFTH,
        SIXTH,
        SEVENTH
    }

    public class ArrowAnimator : MonoBehaviour
    {
        public TotemTutorialStates currrentState;
        private Animator anim;
        private GameObject arrow;
        private RigidbodyConstraints playerConstraints;
        public GameObject Player;
        public GameObject Cube0;
        public GameObject Beam1;
        public GameObject Cube1;
        public GameObject Beam2;
        public GameObject Pyramid1;
        public GameObject Pyramid2;

        void Start()
        {
            arrow = this.gameObject;
            anim = GetComponent<Animator>();
            currrentState = TotemTutorialStates.FIRST;
            playerConstraints = Player.GetComponent<Rigidbody>().constraints;
            Player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        }

        public void ChangeState(GameObject currentObject)
        {
            switch (currrentState)
            {
                case TotemTutorialStates.FIRST:
                    if (currentObject.gameObject == Player)
                    {
                        Player.GetComponent<Rigidbody>().constraints = playerConstraints;
                        arrow.transform.Rotate(0, 0, 90);
                        arrow.transform.position = new Vector3(Cube0.transform.localPosition.x - 1,
                            Cube0.transform.localPosition.y, arrow.transform.localPosition.z);
                        Cube0.AddComponent<DestroyObject>();
                        currrentState = TotemTutorialStates.SECOND;
                    }
                    break;
                case TotemTutorialStates.SECOND:
                    if (currentObject.gameObject == Cube0)
                    {
                        arrow.transform.Rotate(0, 0, -45);
                        arrow.transform.position = new Vector3(Beam1.transform.localPosition.x - 1,
                            Beam1.transform.localPosition.y + 1, arrow.transform.localPosition.z);
                        Beam1.AddComponent<DestroyObject>();
                        currrentState = TotemTutorialStates.THIRD;
                    }
                    break;
                case TotemTutorialStates.THIRD:
                    if (currentObject.gameObject == Beam1)
                    {
                        arrow.transform.Rotate(0, 0, 0);
                        arrow.transform.position = new Vector3(Cube1.transform.localPosition.x - 1,
                            Cube1.transform.localPosition.y + 1, arrow.transform.localPosition.z);
                        Cube1.AddComponent<DestroyObject>();
                        currrentState = TotemTutorialStates.FOURTH;
                    }
                    break;
                case TotemTutorialStates.FOURTH:
                    if (currentObject.gameObject == Cube1)
                    {
                        arrow.transform.Rotate(0, 0, 45);
                        arrow.transform.position = new Vector3(Pyramid1.transform.localPosition.x - 1,
                            Pyramid2.transform.localPosition.y + 1, arrow.transform.localPosition.z);
                        Pyramid1.AddComponent<DestroyObject>();
                        currrentState = TotemTutorialStates.FIFTH;
                    }
                    break;
                case TotemTutorialStates.FIFTH:
                    if (currentObject.gameObject == Pyramid1)
                    {
                        arrow.transform.Rotate(0, 0, 180);
                        arrow.transform.position = new Vector3(Pyramid2.transform.localPosition.x + 1,
                            Pyramid2.transform.localPosition.y + 1, arrow.transform.localPosition.z);
                        Pyramid2.AddComponent<DestroyObject>();
                        currrentState = TotemTutorialStates.SIXTH;
                    }
                    break;
                case TotemTutorialStates.SIXTH:
                    if (currentObject.gameObject == Pyramid2)
                    {
                        arrow.transform.Rotate(0, 0, 45);
                        arrow.transform.position = new Vector3(Beam2.transform.localPosition.x + 1,
                            Beam2.transform.localPosition.y + 1, arrow.transform.localPosition.z);
                        Beam2.AddComponent<DestroyObject>();
                        currrentState = TotemTutorialStates.SEVENTH;
                    }
                    break;
                case TotemTutorialStates.SEVENTH:
                    if (currentObject.gameObject == Beam2)
                    {
                        anim.Stop();
                        arrow.gameObject.SetActive(false);
                    }
                    break;
            }
        }
    }
}

