using UnityEngine;
using UnityEngine.UI.Extensions;
using System.Collections.Generic;

public class BarGraphCurveConnector : MonoBehaviour
{
    [Header("막대들이 들어있는 부모 (Canvas 내부)")]
    public RectTransform barParent;

    [Header("곡선을 그릴 UILineRenderer")]
    public UILineRenderer lineRenderer;

    [Header("곡선 부드러움 정도 (0.05~0.2 정도 권장)")]
    [Range(0.01f, 0.3f)] public float smoothStep = 0.1f;

    void Start()
    {
        ConnectBarTops();
    }

    public void ConnectBarTops()
    {
        if (barParent == null || lineRenderer == null)
        {
            Debug.LogError("barParent 또는 lineRenderer가 지정되지 않았습니다!");
            return;
        }

        // --- 1️⃣ 막대들의 상단 좌표 수집 ---
        List<Vector2> points = new List<Vector2>();
        for (int i = 0; i < barParent.childCount; i++)
        {
            RectTransform bar = barParent.GetChild(i).GetChild(0).GetComponent<RectTransform>();
            if (bar == barParent) continue; // 자기 자신 제외
            if (!bar.gameObject.activeSelf) continue; // 비활성화된 막대 제외

            // anchoredPosition + (height/2)
            float topY = bar.sizeDelta.y - bar.anchoredPosition.y;
            float x = barParent.GetChild(i).GetComponent<RectTransform>().anchoredPosition.x;
            points.Add(new Vector2(x, topY));
        }

        // --- 2️⃣ x좌표순으로 정렬 (혹시 섞여 있을 경우 대비) ---
        points.Sort((a, b) => a.x.CompareTo(b.x));

        // --- 3️⃣ Catmull-Rom 보간으로 부드럽게 연결 ---
        Vector2[] smoothPoints = Smooth(points.ToArray(), smoothStep);

        // --- 4️⃣ UILineRenderer에 적용 ---
        lineRenderer.Points = smoothPoints;
        lineRenderer.LineThickness = 3f;
        lineRenderer.color = Color.yellow;
    }

    Vector2[] Smooth(Vector2[] pts, float step)
    {
        List<Vector2> smoothPts = new List<Vector2>();
        for (int i = 0; i < pts.Length - 1; i++)
        {
            Vector2 p0 = i > 0 ? pts[i - 1] : pts[i];
            Vector2 p1 = pts[i];
            Vector2 p2 = pts[i + 1];
            Vector2 p3 = (i + 2 < pts.Length) ? pts[i + 2] : pts[i + 1];

            for (float t = 0; t <= 1f; t += step)
            {
                Vector2 pt = 0.5f * (
                    (2f * p1) +
                    (-p0 + p2) * t +
                    (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
                    (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t
                );
                smoothPts.Add(pt);
            }
        }
        smoothPts.Add(pts[pts.Length - 1]);
        return smoothPts.ToArray();
    }
}
