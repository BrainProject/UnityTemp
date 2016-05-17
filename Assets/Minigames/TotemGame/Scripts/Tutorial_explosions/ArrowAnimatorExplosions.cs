using UnityEngine;
using System.Collections;

namespace TotemGame
{
    public enum TotemTutorialExplosionStates
    {
        FIRST,
        SECOND,
        THIRD,
        FOURTH,
        FIFTH,
        SIXTH,
        SEVENTH
    }

    public class ArrowAnimatorExplosions : MonoBehaviour
    {
        public TotemTutorialExplosionStates currrentState;
        private Animator anim;
        private GameObject arrow;
        private RigidbodyConstraints playerConstraints;
        public GameObject Player;
        public GameObject Cube0;
        public GameObject Beam1;
        public GameObject Cube1;
        public GameObject Beam2;
        public GameObject Sphere2;
        public GameObject bomb;

        void Start()
        {
            arrow = this.gameObject;
            anim = GetComponent<Animator>();
            currrentState = TotemTutorialExplosionStates.FIRST;
            playerConstraints = Player.GetComponent<Rigidbody>().constraints;
            Player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        }

        public void ChangeState(GameObject currentObject)
        {
            switch (currrentState)
            {
                case TotemTutorialExplosionStates.FIRST:
                    if (currentObject.gameObject == Player)
                    {
                        Player.GetComponent<Rigidbody>().constraints = playerConstraints;
                        arrow.transform.Rotate(0, 0, 90);
                        arrow.transform.position = new Vector3(Cube0.transform.localPosition.x - 1,
                            Cube0.transform.localPosition.y, arrow.transform.localPosition.z);
                        Cube0.AddComponent<DestroyObject>();
                        currrentState = TotemTutorialExplosionStates.SECOND;
                    }
                    break;
                case TotemTutorialExplosionStates.SECOND:
                    if (currentObject.gameObject == Cube0)
                    {
                        arrow.transform.Rotate(0, 0, -45);
                        arrow.transform.position = new Vector3(Cube1.transform.localPosition.x - 1,
                            Cube1.transform.localPosition.y + 1, arrow.transform.localPosition.z);
                        Cube1.AddComponent<ExplosionForce>();
                        Cube1.GetComponent<ExplosionForce>().radius = 100.0F;
                        Cube1.GetComponent<ExplosionForce>().power = 110.0F;
                        Cube1.GetComponent<ExplosionForce>().bomb = bomb;
                        currrentState = TotemTutorialExplosionStates.THIRD;
                    }
                    break;
                case TotemTutorialExplosionStates.THIRD:
                    if (currentObject.gameObject == Cube1)
                    {
                        arrow.transform.Rotate(0, 0, 0);
                        arrow.transform.position = new Vector3(Beam1.transform.localPosition.x - 1,
                            Beam1.transform.localPosition.y, arrow.transform.localPosition.z);
                        Beam1.AddComponent<DestroyObject>();
                        currrentState = TotemTutorialExplosionStates.FOURTH;
                    }
                    break;
                case TotemTutorialExplosionStates.FOURTH:
                    if (currentObject.gameObject == Beam1)
                    {
                        arrow.transform.Rotate(0, 0, 0);
                        arrow.transform.position = new Vector3(Sphere2.transform.localPosition.x -1,
                            Sphere2.transform.localPosition.y +1, arrow.transform.localPosition.z);
                        Sphere2.AddComponent<ExplosionForce>();
                        Sphere2.GetComponent<ExplosionForce>().radius = 70.0F;
                        Sphere2.GetComponent<ExplosionForce>().power = 70.0F;
                        Sphere2.GetComponent<ExplosionForce>().bomb = bomb;
                        currrentState = TotemTutorialExplosionStates.FIFTH;
                    }
                    break;
                case TotemTutorialExplosionStates.FIFTH:
                    if (currentObject.gameObject == Sphere2)
                    {
                        arrow.transform.Rotate(0, 0, 270);
                        arrow.transform.position = new Vector3(Beam2.transform.localPosition.x + 1,
                            Beam2.transform.localPosition.y + 1, arrow.transform.localPosition.z);
                        Beam2.AddComponent<DestroyObject>();
                        currrentState = TotemTutorialExplosionStates.SIXTH;
                    }
                    break;
                case TotemTutorialExplosionStates.SIXTH:
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
