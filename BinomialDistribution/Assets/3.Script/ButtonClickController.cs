using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class infoButton 
{
    public Button button;
    public Sprite idleSprite;
    public Sprite disableSprite;
    public bool isActive = true;

    public void UpdateButtonStates()
    {
        if (isActive)
        {
            button.image.sprite = idleSprite;
        }
        else
        {
            button.image.sprite = disableSprite;
        }
    }
}


public class ButtonClickController : MonoBehaviour
{
    public infoButton[] infoButtons;



    public void ApplyButtonSelection(int btnIndex)
    {
        for (int i = 0; i < infoButtons.Length; i++)
        {
            infoButtons[i].isActive = false;
            if (i == btnIndex)
            {
                infoButtons[btnIndex].isActive = true;

            }
            infoButtons[i].UpdateButtonStates();
        }
    }
    public void ResetButtonActive()
    {
        for (int i = 0; i < infoButtons.Length; i++)
        {
            infoButtons[i].isActive = true;
            infoButtons[i].UpdateButtonStates();
        }
    }
}
