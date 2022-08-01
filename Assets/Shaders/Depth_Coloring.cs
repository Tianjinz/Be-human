using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depth_Coloring : MonoBehaviour
{
    // Start is called before the first frame update
    public Material mat;

    void Start () 
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.DepthNormals;
    }

    void OnRenderImage (RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source,destination,mat);
        //mat is the material which contains the shader
        //we are passing the destination RenderTexture to
        
    }
}
