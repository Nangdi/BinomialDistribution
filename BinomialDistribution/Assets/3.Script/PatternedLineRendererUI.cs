using UnityEngine;
using UnityEngine.UI.Extensions;
using System.Collections.Generic;

[RequireComponent(typeof(UILineRenderer))]
public class PatternedLineRendererUI : MonoBehaviour
{
    [Header("스프라이트 패턴 텍스처 (반복될 선무늬)")]
    public Sprite patternSprite;

    [Header("선 굵기 조정")]
    [Range(1f, 20f)] public float thickness = 5f;

    [Header("패턴 반복 스케일 (값이 작을수록 반복이 촘촘함)")]
    [Range(0.1f, 100f)] public float patternScale = 1f;

    private UILineRenderer lineRenderer;
    private Material lineMaterial;

    void Awake()
    {
        lineRenderer = GetComponent<UILineRenderer>();
        InitMaterial();
    }

    void InitMaterial()
    {
        if (patternSprite == null)
        {
            Debug.LogWarning("Pattern Sprite가 지정되지 않았습니다.");
            return;
        }

        // UI용 Unlit Material 생성
        Shader shader = Shader.Find("UI/Unlit/Transparent");
        lineMaterial = new Material(shader);
        lineMaterial.mainTexture = patternSprite.texture;
        lineMaterial.mainTexture.wrapMode = TextureWrapMode.Repeat;

        lineRenderer.material = lineMaterial;
    }

    void Update()
    {
        if (lineRenderer == null || lineMaterial == null) return;

        // 두께 반영
        lineRenderer.LineThickness = thickness;

        // 전체 선 길이에 비례한 UV 반복 조정
        float totalLength = GetTotalLineLength(lineRenderer.Points);
        lineMaterial.mainTextureScale = new Vector2(totalLength / (100f * patternScale), 1f);
    }

    float GetTotalLineLength(Vector2[] pts)
    {
        if (pts == null || pts.Length < 2) return 1f;
        float len = 0f;
        for (int i = 0; i < pts.Length - 1; i++)
            len += Vector2.Distance(pts[i], pts[i + 1]);
        return len;
    }
}
