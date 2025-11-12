using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions.Tweens;

public class HomeReturn : MonoBehaviour
{
    public float lapseTime =0;
    public float endTime =120f;
    public GameObject InfoPanel;
    public SceneSlider[] sliders;
    private void Start()
    {
        endTime = JsonManager.instance.gameSettingData.homeReturnTime;
    }
    private void Update()
    {
        lapseTime += Time.deltaTime;
        if(lapseTime >= endTime)
        {
            lapseTime = 0;
            InfoPanel.SetActive(false);
            ResetSliders();
        }
    }
    public void ResetSliders()
    {
        foreach (var item in sliders)
        {
            item.sceneIndex = 0;
            item.UpdateActiveScene(0);
        }
    }
    public void ChangeScene(int index)
    {
        lapseTime = 0;
        ResetSliders();
        InfoPanel.SetActive(true);
        sliders[index].UpdateActiveScene(index);
    }
}
