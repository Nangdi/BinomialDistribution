using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;
[System.Serializable]
public class BarGraph
{
    public Image bar;   // 막대 이미지
    public TMP_Text valueText; // 현재 값 표시용 텍스트
    public int barValue;   // 내부 카운트
   

    
}

public class BarGraphController : MonoBehaviour
{
    [Header("Today")]
    public List<BarGraph> barGraphs =new List<BarGraph>();   // Inspector에 15개 Image 직접 할당
    [Header("Untilnow")]
    public List<BarGraph> totalBarGraphs = new List<BarGraph>();   // Inspector에 15개 Image 직접 할당
    public float maxHeight = 600f;                 // 막대 최대 높이(px)
    public int todayTotalCount;
    //private List<int> values = new List<int>(); // 각 막대의 실제 값
    float percentageSum = 0;
    [SerializeField]
    private BarGraphCurveConnector barconnector;
    void Start()
    {


        // 15개 값 초기화
        InitTotal();
        ResetTodayBars();  // 초기화 시작
    }

    // 막대 전체 리셋
    public void ResetTodayBars()
    {
        Debug.Log("리셋");
        for (int i = 0; i < barGraphs.Count; i++)
        {
            barGraphs[i].barValue = 0;
            UpdateValueText(barGraphs);
            // UI 크기 초기화
            //RectTransform rt = barGraphs[i].bar.rectTransform;
            //rt.sizeDelta = new Vector2(rt.sizeDelta.x, 0);
            // 색상 변화(선택)
            //barGraphs[i].bar.color = Color.Lerp(Color.cyan, Color.red, 0);
        }
    }
    public void MotionReset(List<BarGraph> _graphs)
    {
        foreach (var item in _graphs)
        {
            item.bar.fillAmount = 0;
            RectTransform textRt = item.valueText.rectTransform;
            textRt.anchoredPosition = new Vector2(textRt.anchoredPosition.x, 0);
        }
    }
    //total UI Init
    private void InitTotal()
    {
        for (int i = 0; i < totalBarGraphs.Count; i++)
        {
            totalBarGraphs[i].barValue = JsonManager.instance.gameSettingData.barValueArray[i];
        }
        UpdateGraphs();
    }

    // 특정 인덱스(1~15)의 막대값 +1
    public void AddValue(int index)
    {
        if (index < 0 || index > barGraphs.Count)
        {
            Debug.LogWarning($"잘못된 인덱스: {index}. (1~{barGraphs.Count})");
            return;
        }

        int idx = index - 1;  // 0-based index
        barGraphs[idx].barValue += 1;
        todayTotalCount++;
        //전체 total 갯수 ++;
        JsonManager.instance.gameSettingData.totalSum++;
        //idx에 해당하는 totalbarValue[idx] ++;
        totalBarGraphs[idx].barValue++;

        UpdateGraphs();


    }
    // 그래프 전체 업데이트
   
    private float GetMaxValue(List<BarGraph> _barGraphs)
    {
        float maxValue = -1;
        foreach (var bar in _barGraphs)
        {
            if (bar.barValue > maxValue) maxValue = bar.barValue;
        }
        return maxValue;
    }
    public void UpdateGraphs()
    {
        UpdateGraph(barGraphs);
        UpdateValueText(barGraphs);
        UpdateGraph(totalBarGraphs);
        UpdatePercentageText(totalBarGraphs);

    }

    private void UpdateGraph(List<BarGraph> _barGraphs)
    {//그래프 높이 업데이트하는 메소드

        // 최대값 찾기
        float maxValue = GetMaxValue(_barGraphs);

        // 막대별 크기 재조정
      
        for (int i = 0; i < _barGraphs.Count; i++)
        {
            //그래프높이
            SetGraph(_barGraphs[i], maxValue);
        }
    }
    public void SetGraph(BarGraph graph , float max)
    {
        //비율구하기
        float normalized = graph.barValue / max;
        if(max == 0)
        {
            normalized = 0;
        }
        //graph.bar.fillAmount = normalized;

        float yPos = maxHeight * normalized;
        RectTransform textRt = graph.valueText.rectTransform;
        //textRt.anchoredPosition = new Vector2(textRt.anchoredPosition.x, yPos);
        GraphRunner runner = graph.bar.GetComponent<GraphRunner>();
        runner.AnimateFill(normalized, maxHeight , graph);
        //// 색상 변화(선택)
        //graph.color = Color.Lerp(Color.cyan, Color.red, normalized);


    }

    // 테스트용 입력
    void Update()
    {
        // 숫자키(Alpha1~Alpha9) 및 Keypad1~15 입력 테스트
        //for (int i = 1; i < 10; i++)
        //{
        //    if (Input.GetKeyDown(KeyCode.Alpha0 + i))
        //        AddValue(i);
        //}
        //for (int i = 0; i < 6; i++)
        //{
        //    if (Input.GetKeyDown(KeyCode.Keypad0 + i))
        //        AddValue(10 + i);  // 10~15
        //}

        // R키 → 전체 리셋
        if (Input.GetKeyDown(KeyCode.R))
            ResetTodayBars();
        barconnector.ConnectBarTops();

    }
    //오늘하루 쌓인 갯수업데이트
    public void UpdateValueText(List<BarGraph> _graph)
    {
        foreach (var item in _graph)
        {
            item.valueText.text = $"{item.barValue}";

        }
    }
    //총계 백분률적용하기
    public void UpdatePercentageText(List<BarGraph> _graph)
    {
            percentageSum = 0;
        for (int i = 0; i < _graph.Count; i++)
        {
            float normalized = (float)_graph[i].barValue / JsonManager.instance.gameSettingData.totalSum * 100;
            float rounded = Mathf.Round(normalized * 10) / 10;
            if (i != _graph.Count - 1)
            {
                percentageSum += rounded;
                _graph[i].valueText.text = $"{rounded}";

            }
            else
            {
                float fixPer = 100 - percentageSum;
                //Debug.Log($"fixPer : {fixPer}");
                float fixRounded = Mathf.Round(fixPer * 10) / 10;
                //Debug.Log($"fixRounded : {fixRounded}");
                _graph[i].valueText.text = $"{fixRounded}";
            }

            //백분률로 업데이트
        }

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
