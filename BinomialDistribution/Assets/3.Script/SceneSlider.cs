using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSlider : MonoBehaviour
{
    [SerializeField]
    private HomeReturn homeReturn;
    [SerializeField]
    private GameObject[] slideScenes;
    [SerializeField]
    private GameObject Informationpanel;
    public int sceneIndex = 0;


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
}
