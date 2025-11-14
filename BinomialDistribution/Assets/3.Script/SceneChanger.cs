using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions.Tweens;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    private ButtonClickController clickController;
    public float lapseTime =0;
    public float endTime =40f;
    private bool startCount;
    public GameObject InfoPanel;
    public SceneSlider[] sliders;
    private void Start()
    {
        endTime = JsonManager.instance.gameSettingData.homeTime;
    }
    private void Update()
    {
        lapseTime += Time.deltaTime;
        if(lapseTime >= endTime)
        {
            ReturnHome();
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
        foreach (var item in sliders)
        {
            item.gameObject.SetActive(false);
        }
        //slideScenes[index].GetComponents<SceneSlider>().
        sliders[index].gameObject.SetActive(true);
        startCount = true;
        if(index == 1)
        {
            sliders[index].SetSlideIndex(0);
        }
    }
    public void ReturnHome()
    {
        startCount = false;
        lapseTime = 0;
        InfoPanel.SetActive(false);
        ResetSliders();
        clickController.ResetButtonActive();
    }
}
