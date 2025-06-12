using UnityEngine;
using System.IO;

public class TransparentScreenshot : MonoBehaviour
{
    public Camera renderCam; // 투명 배경으로 캡처할 전용 카메라
    public int width = 512;
    public int height = 512;

    public MeshFilter meshFilter;

    [ContextMenu("Capture Transparent Screenshot")]
    public void Capture()
    {
        // RenderTexture 생성 (ARGB32: 알파 포함)
        RenderTexture rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        renderCam.targetTexture = rt;

        // 배경 투명하게 설정
        renderCam.clearFlags = CameraClearFlags.SolidColor;
        renderCam.backgroundColor = new Color(0, 0, 0, 0); // 완전 투명

        // 렌더링
        renderCam.Render();

        // 픽셀 읽기
        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        // PNG 저장
        byte[] bytes = tex.EncodeToPNG();
        string path = Path.Combine(Application.dataPath, "ScreenShoot/"+ meshFilter.mesh.name+ ".png");
        File.WriteAllBytes(path, bytes);
        Debug.Log("Saved to: " + path);

        // 정리
        renderCam.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(rt);
        DestroyImmediate(tex);
    }
}
