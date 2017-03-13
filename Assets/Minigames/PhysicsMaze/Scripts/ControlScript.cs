using UnityEngine;
using System.Collections;

namespace MinigameMaze2
{
    public class ControlScript : MonoBehaviour
    {
        private float maxAngle = 30.0f;
        private float rotationSpeed = 15f;

        /*#if UNITY_ANDROID
        private Gyroscope gyro;

        void Start()
        {
            gyro = Input.gyro;
            if (!gyro.enabled)
            {
                gyro.enabled = true;
            }
        }
        #endif*/

        void LateUpdate()
        {
            float horizontal;
            float vertical;
            Quaternion direction;

            /*#if UNITY_ANDROID
                Quaternion attitude = Input.gyro.attitude;
                horizontal = Mathf.Clamp(attitude.eulerAngles.x, -maxAngle, maxAngle);
                vertical = Mathf.Clamp(attitude.eulerAngles.z, -maxAngle, maxAngle);
                direction = Quaternion.Euler(new Vector3(vertical, 0, horizontal));
                transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotationSpeed * Time.deltaTime);
            #endif*/

            if (Input.GetMouseButton(0))
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.x -= Screen.width / 2;
                mousePos.y -= Screen.height / 2;

                horizontal = mousePos.x * 0.2f * (-1);
                vertical = mousePos.y * 0.2f;

                horizontal = Mathf.Clamp(transform.rotation.x + horizontal, -maxAngle, maxAngle);
                vertical = Mathf.Clamp(transform.rotation.z + vertical, -maxAngle, maxAngle);

                direction = Quaternion.Euler(new Vector3(vertical, 0, horizontal));
                transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
