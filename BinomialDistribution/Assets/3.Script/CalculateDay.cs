using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CalculateDay
{

    public  static int GetPassedDays(int[] installDay)
    {
        DateTime installDate = new DateTime(installDay[0], installDay[1], installDay[2]);
        DateTime nowDate = DateTime.Now;

        int passedDays = (nowDate - installDate).Days + 1;  // 설치일 포함
        Debug.Log($"설치 후 {passedDays}일 지남");
        return passedDays;
    }
}
