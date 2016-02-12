using System;
using UnityEngine;

namespace UnityStandardAssets.Water
{
    [ExecuteInEditMode]
    public class WaterBasic : MonoBehaviour
    {
        Material mat;

        void Start()
        {
            if(GetComponent<Renderer>())
            mat = GetComponent<Renderer>().sharedMaterial;
        }

        void Update()
        {
            if (!mat)
            {
                return;
            }

            //Vector4 waveSpeed = mat.GetVector("WaveSpeed");
           // float waveScale = mat.GetFloat("_WaveScale");
            float t = Time.time / 40.0f;

            //Vector4 offset4 = waveSpeed * (t * waveScale);
            //Vector4 offsetClamped = new Vector4(Mathf.Repeat(offset4.x, 1.0f), Mathf.Repeat(offset4.y, 1.0f),
            //Mathf.Repeat(offset4.z, 1.0f), Mathf.Repeat(offset4.w, 1.0f));
            mat.SetTextureOffset("_MainTex", new Vector2(t, 0));// ("_WaveOffset", offsetClamped);
        }
    }
}