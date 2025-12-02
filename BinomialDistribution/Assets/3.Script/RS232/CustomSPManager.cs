using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Windows;

public class CustomSPManager : SerialPortManager
{
    [SerializeField] private BarGraphController graphController;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void ReceivedData(string data)
    {
        /*                       컨트롤러               PC
         1번 공 떨어짐   D01 + 엔터(0xd) ->
         15번 공 떨어짐  D15 + 엔터(0xd) ->
        */
        // "D01" → 1, "D15" → 15 로 변환

        //Debug.Log($"받은데이터 : {data}");
        dataText1.text = $"소프트웨어에서 전처리한 신호 : {data}";

        if (data.StartsWith("D"))
        {
            // "D" 이후의 숫자 부분만 추출
            string numPart = data.Substring(1);

            if (int.TryParse(numPart, out int index))
            {
                graphController.AddValue(index);
            }
            else
            {
                Debug.LogWarning($"Invalid data format: {data}");
            }
        }
    }
    private void Update()
    {
        for (int i = 1; i < 16; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                ReceivedData($"D{i}");
        }
        for (int i = 0; i < 6; i++)
        {
            if (Input.GetKeyDown(KeyCode.Keypad0 + i))
                ReceivedData($"D1{i}");
        }
        
           
    }
}
