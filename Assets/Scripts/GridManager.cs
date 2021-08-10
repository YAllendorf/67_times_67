using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//hierfür nehme ich keinen Credit
//ich habe das Script aus dem im README erwähnten Tutorial und lediglich die Werte angepasst, um die Umrandung des Feldes zu rendern

public class GridManager : MonoBehaviour
{
    private Material lineMaterial;

    public bool showMain = true;

    public int gridSizeX;
    public int gridSizeY;

    public float startX;
    public float startY;
    public float startZ;

    public Color mainColor = new Color(0f, 1f, 0f, 1f);

    void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            var shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);

            //hides it from the garbageCollector
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;

            // Turn on alpha blending
            lineMaterial.SetInt("_ScrBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

            //turn off depth writing
            lineMaterial.SetInt("_ZWrite", 0);

            //turn off backface culling
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        }
    }

    private void OnDisable()
    {
        DestroyImmediate(lineMaterial);
    }

    private void OnPostRender()
    {
        CreateLineMaterial();

        lineMaterial.SetPass(0);

        GL.Begin(GL.LINES);

        if (showMain)
        {
            GL.Color(mainColor);

            GL.Vertex3(startX, startY, 0);
            GL.Vertex3(gridSizeX - 0.5f, startY, 0);
            GL.Vertex3(startX, gridSizeY - 0.5f, 0);
            GL.Vertex3(gridSizeX - 0.5f, gridSizeY - 0.5f, 0);
            GL.Vertex3(startX, startY, 0);
            GL.Vertex3(startX, gridSizeY - 0.5f, 0);
            GL.Vertex3(gridSizeX - 0.5f, startY, 0);
            GL.Vertex3(gridSizeX - 0.5f, gridSizeY - 0.5f, 0);
        }

        GL.End();
    }
}
