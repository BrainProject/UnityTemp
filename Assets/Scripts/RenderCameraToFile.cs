using UnityEngine;
using System.Collections;
using System.IO;

#if UNITY_STANDALONE
public class RenderCameraToFile : MonoBehaviour 
{
    public Camera cam;
    public int width = 1600;
    public int height = 900;
    public string defaultFileName = "savedCameraFrame.png";

    public void RenderToFile(string fileName)
    {
        // create new render texture and set it        
        RenderTexture tempRT = new RenderTexture(width, height, 24);
        cam.targetTexture = tempRT;

        //render a frame
        cam.Render();

        //read out pixels from renderTexture to Texture2D
        RenderTexture.active = cam.targetTexture;
        Texture2D virtualPhoto = new Texture2D(cam.targetTexture.width, cam.targetTexture.height, TextureFormat.RGB24, false); // false, meaning no need for mipmaps
        virtualPhoto.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);

        // can help avoid errors
        RenderTexture.active = null; 
        cam.targetTexture = null;
        
        //save Texture2D to file
        byte[] bytes;
        bytes = virtualPhoto.EncodeToPNG();
        print("Saving picture to file: " + Application.dataPath + "/" + fileName);
        System.IO.File.WriteAllBytes(Application.dataPath + "/" + fileName, bytes);

        Destroy(tempRT);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            
            RenderToFile(defaultFileName);
        }        
    }
}
#endif