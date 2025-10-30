using UnityEngine;
using UnityEngine.UI.Extensions;
using System.Collections.Generic;

[RequireComponent(typeof(UILineRenderer))]
public class UIBinomialCurve : MonoBehaviour
{
    public RectTransform[] bars;
    public int smoothness = 10;
    public float lineThickness = 6f;

    private UILineRenderer line;
    private RectTransform lineRect;  // 라인 자기 자신 RectTransform

    void Start()
    {
        line = GetComponent<UILineRenderer>();
        lineRect = GetComponent<RectTransform>();
        line.LineThickness = lineThickness;
        UpdateCurve();
    }

    public void UpdateCurve()
    {
        if (bars == null || bars.Length < 2) return;

        List<Vector2> points = new List<Vector2>();

        for (int i = 0; i < bars.Length - 1; i++)
        {
            Vector2 p0 = GetTopLocal(i - 1 < 0 ? bars[i] : bars[i - 1]);
            Vector2 p1 = GetTopLocal(bars[i]);
            Vector2 p2 = GetTopLocal(bars[i + 1]);
            Vector2 p3 = GetTopLocal(i + 2 >= bars.Length ? bars[i + 1] : bars[i + 2]);

            for (int j = 0; j <= smoothness; j++)
            {
                float t = j / (float)smoothness;
                points.Add(CatmullRom(p0, p1, p2, p3, t));
            }
        }

        line.Points = points.ToArray();
    }

    Vector2 GetTopLocal(RectTransform bar)
    {
        // 막대 상단의 월드 좌표
        Vector3 worldTop = bar.TransformPoint(new Vector3(0, bar.rect.height * (1f - bar.pivot.y), 0));
        // 그 월드좌표를 UILineRenderer의 로컬 좌표로 변환
        Vector2 localTop = lineRect.InverseTransformPoint(worldTop);
        return localTop;
    }

    Vector2 CatmullRom(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        return 0.5f * (
            (2f * p1) +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * (t * t) +
            (-p0 + 3f * p1 - 3f * p2 + p3) * (t * t * t)
        );
    }
}
