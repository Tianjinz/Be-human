using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


public class ShaderBakeToTexture : MonoBehaviour
{
    public Material ImageMaterial;
    public string FilePath;
    public Vector2Int Resolution;

    void Start()
    {
        BakeTexture();
    }

    void BakeTexture()
    {   
        //render the material to a temporary rendertexture
        RenderTexture renderTexture = RenderTexture.GetTemporary(Resolution.x, Resolution.y);
        Graphics.Blit(null, renderTexture, ImageMaterial);

        //transfer image from rendertexture to texture
        Texture2D texture = new Texture2D(Resolution.x, Resolution.y);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(Vector2.zero, Resolution), 0, 0);

        byte[] png = texture.EncodeToPNG();
        File.WriteAllBytes(FilePath, png);
        //AssetDatabase.Refresh();

        //RenderTexture.active = null;
        //RenderTexture.ReleaseTemporary(renderTexture);
        //DestroyImmediate(texture);
    }
 
}
