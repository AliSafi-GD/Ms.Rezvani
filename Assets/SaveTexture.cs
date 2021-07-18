using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveTexture : MonoBehaviour
{
    public RenderTexture renderTexture;
    public string nameTex;
    public void SaveTexturePNG()
    {
        // Encode texture into PNG
        var tex = toTexture2D(renderTexture);
        byte[] bytes = tex.EncodeToPNG();
        //Object.Destroy(tex);

        // For testing purposes, also write to a file in the project folder
        File.WriteAllBytes(Application.dataPath + "/"+ nameTex+".png", bytes);
        print((Application.dataPath + "SavedScreen.png"));
    }
    // Start is called before the first frame update
    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(1000, 1000, TextureFormat.RGBA32, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }
}
