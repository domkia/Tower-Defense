using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SelectionPostEffect : MonoBehaviour
{
    public int downscale = 2;
    public Shader whiteShader;
    public Material outlineEffectMaterial;

    private Camera cam;
    private Camera effectCamera;
    private RenderTexture rt;
    private int width, height;

    void Awake()
    {
        //Debug.Log("Device supports image effects: " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8));
        cam = GetComponent<Camera>();

        //Setup effect camera
        effectCamera = new GameObject("Effect Camera").AddComponent<Camera>();
        effectCamera.CopyFrom(cam);
        effectCamera.transform.parent = transform;
        effectCamera.clearFlags = CameraClearFlags.Color;
        effectCamera.backgroundColor = Color.black;
        effectCamera.cullingMask = 1 << LayerMask.NameToLayer("Outline");
        effectCamera.enabled = false;

        width = Screen.width >> downscale;
        height = Screen.height >> downscale;
    }

    private void OnPreRender()
    {
        rt = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.R8);
        effectCamera.targetTexture = rt;
        effectCamera.RenderWithShader(whiteShader, "");
    }

    private void OnPostRender()
    {
        effectCamera.targetTexture = null;
        outlineEffectMaterial.SetTexture("_MainTex", rt);

        Graphics.Blit(rt, null as RenderTexture, outlineEffectMaterial);
        RenderTexture.ReleaseTemporary(rt);
    }

    public void SetColor(Color color)
    {
        outlineEffectMaterial.SetColor("_OutlineColor", color);
    }
}
