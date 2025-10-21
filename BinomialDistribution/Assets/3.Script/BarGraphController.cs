using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BarGraphController : MonoBehaviour
{
    [Header("Bar References (���� �̹� �����ϴ� 15��)")]
    public List<Image> bars = new List<Image>();   // Inspector�� 15�� Image ���� �Ҵ�
    public float maxHeight = 500f;                 // ���� �ִ� ����(px)

    private List<float> values = new List<float>(); // �� ������ ���� ��

    void Start()
    {
        // 15�� �� �ʱ�ȭ
        for (int i = 0; i < bars.Count; i++)
            values.Add(0f);

        ResetBars();  // �ʱ�ȭ ����
    }

    // ���� ��ü ����
    public void ResetBars()
    {
        for (int i = 0; i < bars.Count; i++)
        {
            values[i] = 0f;
            Debug.Log("ũ���ʱ�ȭ");
            // UI ũ�� �ʱ�ȭ
            RectTransform rt = bars[i].rectTransform;
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, 10f);
        }
    }

    // Ư�� �ε���(1~15)�� ���밪 +1
    public void AddValue(int index)
    {
        if (index < 0 || index >bars.Count)
        {
            Debug.LogWarning($"�߸��� �ε���: {index}. (1~{bars.Count})");
            return;
        }

        int idx = index - 1;  // 0-based index
        values[idx] += 1f;

        UpdateGraph();
    }

    // �׷��� ��ü ������Ʈ
    private void UpdateGraph()
    {
        // ���� �ִ밪 ã��
        float maxValue = Mathf.Max(values.ToArray());
        if (maxValue <= 0f) maxValue = 1f; // 0���� ������ ����

        // ���뺰 ũ�� ������
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

            // ���� ��ȭ(����)
            bars[i].color = Color.Lerp(Color.cyan, Color.red, normalized);
        }
    }

    // �׽�Ʈ�� �Է�
    void Update()
    {
        // ����Ű(Alpha1~Alpha9) �� Keypad1~15 �Է� �׽�Ʈ
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

        // RŰ �� ��ü ����
        if (Input.GetKeyDown(KeyCode.R))
            ResetBars();
    }
}
