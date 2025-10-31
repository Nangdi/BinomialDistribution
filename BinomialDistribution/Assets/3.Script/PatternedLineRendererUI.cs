using UnityEngine;
using UnityEngine.UI.Extensions;
using System.Collections.Generic;

[RequireComponent(typeof(UILineRenderer))]
public class PatternedLineRendererUI : MonoBehaviour
{
    [Header("��������Ʈ ���� �ؽ�ó (�ݺ��� ������)")]
    public Sprite patternSprite;

    [Header("�� ���� ����")]
    [Range(1f, 20f)] public float thickness = 5f;

    [Header("���� �ݺ� ������ (���� �������� �ݺ��� ������)")]
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
            Debug.LogWarning("Pattern Sprite�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // UI�� Unlit Material ����
        Shader shader = Shader.Find("UI/Unlit/Transparent");
        lineMaterial = new Material(shader);
        lineMaterial.mainTexture = patternSprite.texture;
        lineMaterial.mainTexture.wrapMode = TextureWrapMode.Repeat;

        lineRenderer.material = lineMaterial;
    }

    void Update()
    {
        if (lineRenderer == null || lineMaterial == null) return;

        // �β� �ݿ�
        lineRenderer.LineThickness = thickness;

        // ��ü �� ���̿� ����� UV �ݺ� ����
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
