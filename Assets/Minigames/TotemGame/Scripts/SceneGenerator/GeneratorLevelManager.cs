using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Newron minigame - TotemGame
 *
 * @author Petra Ambrozkova
 */
namespace TotemGame
{
    public class GeneratorLevelManager : MonoBehaviour
    {
        public Slider mainSlider;
        private float difficulty;
        private List<GameObject> Objects = new List<GameObject>();

        public void ValueChangeCheck(Slider mainSlider)
        {
            Debug.Log(mainSlider.value);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                difficulty = mainSlider.value;
                List<GameObject> tmp = GenerateObject();
                print(tmp);
            }
        }

        public List<GameObject> GenerateObject()
        {

            if (difficulty == 1)
            {
                int i = Random.Range(0, 3);
                GameObject prefabRef = Resources.Load<GameObject>("First" + i);
                GameObject clone = Instantiate(prefabRef, Vector3.zero, Quaternion.identity) as GameObject;
                Objects.Add(clone);
                return Objects;

            }

            if (difficulty == 2)
            {
                int i = Random.Range(0, 3);
                GameObject prefabRef = Resources.Load<GameObject>("First" + i);
                GameObject prefabRef2 = Resources.Load<GameObject>("Second" + i);

                GameObject clone = Instantiate(prefabRef, Vector3.zero, Quaternion.identity) as GameObject;
                GameObject clone2 = Instantiate(prefabRef2, Vector3.zero, Quaternion.identity) as GameObject;
                Objects.Add(clone);
                Objects.Add(clone2);
                return Objects;
            }

            if (difficulty == 3)
            {
                int i = Random.Range(0, 3);
                GameObject prefabRef = Resources.Load<GameObject>("First" + i);
                GameObject prefabRef2 = Resources.Load<GameObject>("Second" + i);
                GameObject prefabRef3 = Resources.Load<GameObject>("Third");

                GameObject clone = Instantiate(prefabRef, Vector3.zero, Quaternion.identity) as GameObject;
                GameObject clone2 = Instantiate(prefabRef2, Vector3.zero, Quaternion.identity) as GameObject;
                GameObject clone3 = Instantiate(prefabRef3, Vector3.zero, Quaternion.identity) as GameObject;
                Objects.Add(clone);
                Objects.Add(clone2);
                Objects.Add(clone3);
                return Objects;
            }
            return null;
        }
    }
}
