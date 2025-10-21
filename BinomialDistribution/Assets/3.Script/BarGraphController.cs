using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BarGraphController : MonoBehaviour
{
    [Header("Bar References (씬에 이미 존재하는 15개)")]
    public List<Image> bars = new List<Image>();   // Inspector에 15개 Image 직접 할당
    public float maxHeight = 500f;                 // 막대 최대 높이(px)

    private List<float> values = new List<float>(); // 각 막대의 실제 값

    void Start()
    {
        // 15개 값 초기화
        for (int i = 0; i < bars.Count; i++)
            values.Add(0f);

        ResetBars();  // 초기화 시작
    }

    // 막대 전체 리셋
    public void ResetBars()
    {
        for (int i = 0; i < bars.Count; i++)
        {
            values[i] = 0f;
            Debug.Log("크기초기화");
            // UI 크기 초기화
            RectTransform rt = bars[i].rectTransform;
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, 10f);
        }
    }

    // 특정 인덱스(1~15)의 막대값 +1
    public void AddValue(int index)
    {
        if (index < 0 || index >bars.Count)
        {
            Debug.LogWarning($"잘못된 인덱스: {index}. (1~{bars.Count})");
            return;
        }

        int idx = index - 1;  // 0-based index
        values[idx] += 1f;

        UpdateGraph();
    }

    // 그래프 전체 업데이트
    private void UpdateGraph()
    {
        // 현재 최대값 찾기
        float maxValue = Mathf.Max(values.ToArray());
        if (maxValue <= 0f) maxValue = 1f; // 0으로 나누기 방지

        // 막대별 크기 재조정
        for (int i = 0; i < bars.Count; i++)
        {
            float normalized = values[i] / maxValue;
            float height = normalized * maxHeight;
            if(height == 0)
            {
                height = 10f;
            }
            RectTransform rt = bars[i].rectTransform;
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);

            // 색상 변화(선택)
            bars[i].color = Color.Lerp(Color.cyan, Color.red, normalized);
        }
    }

    // 테스트용 입력
    void Update()
    {
        // 숫자키(Alpha1~Alpha9) 및 Keypad1~15 입력 테스트
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                AddValue(i);
        }
        for (int i = 0; i < 6; i++)
        {
            if (Input.GetKeyDown(KeyCode.Keypad0 + i))
                AddValue(10 + i);  // 10~15
        }

        // R키 → 전체 리셋
        if (Input.GetKeyDown(KeyCode.R))
            ResetBars();
    }
}
