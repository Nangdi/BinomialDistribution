using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneSlider : MonoBehaviour
{
    [SerializeField]
    private SceneChanger homeReturn;
    [SerializeField]
    private GameObject[] slideScenes;
    [SerializeField]
    private GameObject Informationpanel;
    public int sceneIndex = 0;

    public bool useAutoSlide;
    private float slideTime =2;
    private int slideIndex = 0;

    private void Start()
    {
        if (useAutoSlide)
        {
            StartCoroutine(AutoSlide(slideTime , NextSlide));
        }
    }
    public void OnClickPreviousBtn()
    {
        sceneIndex--;
        sceneIndex = Mathf.Clamp(sceneIndex,0, 4);
        UpdateActiveScene(sceneIndex);
    }
    public void OnClickNextBtn()
    {
        sceneIndex++;
        sceneIndex = Mathf.Clamp(sceneIndex, 0, 4);
        UpdateActiveScene(sceneIndex);
    }
    public void UpdateActiveScene(int index)
    {
        homeReturn.lapseTime = 0;
        foreach (var item in slideScenes)
        {
            item.SetActive(false);
        }
        //slideScenes[index].GetComponents<SceneSlider>().
        slideScenes[index].SetActive(true);
    }
    public IEnumerator AutoSlide(float tartget , Action action)
    {
        float currentTime = 0;
        while (true)
        {
            currentTime += Time.deltaTime;
            if(currentTime>= tartget)
            {
                currentTime = 0;
                action.Invoke();
            }
            yield return null;
        }
    }
    public void SetSlideIndex(int index)
    {
        slideIndex = index;
    }
    private void NextSlide()
    {
        UpdateActiveScene(slideIndex);
        slideIndex++;
        slideIndex %= 4;
    }
}
