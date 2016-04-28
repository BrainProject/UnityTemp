using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Reddy
{
    public class OnClick : MonoBehaviour
    {



        public void ChangeToScene(string sceneToChangeTo)
        {

            MGC.Instance.sceneLoader.LoadScene(sceneToChangeTo);
            //Application.LoadLevel(sceneToChangeTo);
        }


    }

}
