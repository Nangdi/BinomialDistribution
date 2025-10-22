using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using System.Linq;
using static UnityEngine.Rendering.DebugUI;
[System.Serializable]
public class BarGraph
{
    public Image bar;   // ���� �̹���
    public TMP_Text valueText; // ���� �� ǥ�ÿ� �ؽ�Ʈ
    public int barValue;   // ���� ī��Ʈ
}

public class BarGraphController : MonoBehaviour
{
    [Header("Today")]
    public List<BarGraph> barGraphs =new List<BarGraph>();   // Inspector�� 15�� Image ���� �Ҵ�
    public float maxHeight = 500f;                 // ���� �ִ� ����(px)
    public int todayTotalCount;
    //private List<int> values = new List<int>(); // �� ������ ���� ��
    [Header("Untilnow")]
    public List<BarGraph> totalBarGraphs = new List<BarGraph>();   // Inspector�� 15�� Image ���� �Ҵ�

    void Start()
    {


        // 15�� �� �ʱ�ȭ
        InitTotal();
        ResetBars();  // �ʱ�ȭ ����
    }

    // ���� ��ü ����
    public void ResetBars()
    {
        Debug.Log("����");
        for (int i = 0; i < barGraphs.Count; i++)
        {
            barGraphs[i].barValue = 0;
            UpdateValueText(barGraphs[i]);
            // UI ũ�� �ʱ�ȭ
            RectTransform rt = barGraphs[i].bar.rectTransform;
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, 10f);
            // ���� ��ȭ(����)
            barGraphs[i].bar.color = Color.Lerp(Color.cyan, Color.red, 0);
        }
    }
    //total UI Init
    private void InitTotal()
    {
        for (int i = 0; i < totalBarGraphs.Count; i++)
        {
            totalBarGraphs[i].barValue = JsonManager.instance.gameSettingData.barValueArray[i];
        }
        UpdateGraph();
    }

    // Ư�� �ε���(1~15)�� ���밪 +1
    public void AddValue(int index)
    {
        if (index < 0 || index > barGraphs.Count)
        {
            Debug.LogWarning($"�߸��� �ε���: {index}. (1~{barGraphs.Count})");
            return;
        }

        int idx = index - 1;  // 0-based index
        barGraphs[idx].barValue += 1;
        todayTotalCount++;
        //��ü total ���� ++;
        JsonManager.instance.gameSettingData.totalSum++;
        //idx�� �ش��ϴ� totalbarValue[idx] ++;
       totalBarGraphs[idx].barValue++;
        


        UpdateGraph();
    }

    // �׷��� ��ü ������Ʈ
    private void UpdateGraph()
    {
        // ���� �ִ밪 ã��
        // �ִ밪 ã��
        float maxValue = -1;
        float totalMaxValue = -1;
        foreach (var bar in barGraphs)
        {
            if (bar.barValue > maxValue) maxValue = bar.barValue;
        }
        foreach (var bar in totalBarGraphs)
        {
            if (bar.barValue > totalMaxValue) totalMaxValue = bar.barValue;
        }
        //Debug.Log($"maxValue : {maxValue}");
        // ���뺰 ũ�� ������
        float percentageSum =0;
        for (int i = 0; i < barGraphs.Count; i++)
        {
            //�׷�������, �� ������Ʈ
            SetGraph(barGraphs[i].barValue, barGraphs[i].bar, maxValue);
            SetGraph(totalBarGraphs[i].barValue, totalBarGraphs[i].bar , totalMaxValue);
            //�ؽ�Ʈ ������Ʈ(�׷����� ī��Ʈ)
            UpdateValueText(barGraphs[i]);
            float normalized = (float)totalBarGraphs[i].barValue / JsonManager.instance.gameSettingData.totalSum *100;
            float rounded = Mathf.Round(normalized * 10) / 10;
            Debug.Log($"�ݿø� : {Mathf.Round(normalized * 10)}");
            Debug.Log($"�ݿø��� �� : {rounded}");

            if (i != totalBarGraphs.Count - 1)
            {
                percentageSum += rounded;
                UpdatePercentageText(totalBarGraphs[i].valueText, rounded);

            }
            else
            {
                float fixPer = 100 - percentageSum;
                float fixRounded = Mathf.Round(fixPer * 10) / 10;
                UpdatePercentageText(totalBarGraphs[i].valueText, fixRounded);
            }

        }
    }
    public void SetGraph(int value , Image graph , float max)
    {
        float normalized = value / max;
        float height = normalized * maxHeight;
        //Debug.Log($"value : {value} , max : {max} , nomalized : {normalized} ");
        if (height == 0)
        {
            height = 10f;
        }
        RectTransform rt = graph.rectTransform;
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);

        // ���� ��ȭ(����)
        graph.color = Color.Lerp(Color.cyan, Color.red, normalized);


    }

    // �׽�Ʈ�� �Է�
    void Update()
    {
        // ����Ű(Alpha1~Alpha9) �� Keypad1~15 �Է� �׽�Ʈ
        for (int i = 1; i < 10; i++)
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
    public void UpdateValueText(BarGraph graph) 
    {
        graph.valueText.text = $"{graph.barValue}";
    }
    public void UpdatePercentageText(TMP_Text percentageText , float percent)
    {
        //��з��� ������Ʈ
        percentageText.text = $"{percent}%";
    }




    private void SaveDataToJson()
    {
        int[] tempArray = JsonManager.instance.gameSettingData.barValueArray;

        for (int i = 0; i < tempArray.Length; i++)
        {
            tempArray[i] += barGraphs[i].barValue;
        }
        JsonManager.instance.gameSettingData.barValueArray = tempArray;
        JsonManager.instance.gameSettingData.totalSum = tempArray.Sum();
        JsonManager.instance.SaveData();

    }


    void OnApplicationQuit()
    {
        SaveDataToJson();
    }
}
