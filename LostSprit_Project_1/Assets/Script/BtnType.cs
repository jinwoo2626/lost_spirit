using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnType : MonoBehaviour
{
    public BTNType currentType;
    public CanvasGroup mainGroup;
    public CanvasGroup startGroupFire;
    public CanvasGroup startGroupWater;
    public void OnBtnClick()
    {
        switch (currentType)
        {
            case BTNType.Fire:
                CanvasGroupOn(startGroupFire);
                CanvasGroupOff(mainGroup);
                CanvasGroupOff(startGroupWater);
                break;
            case BTNType.Water:
                CanvasGroupOff(startGroupFire);
                CanvasGroupOff(mainGroup);
                CanvasGroupOn(startGroupWater);
                break;
            case BTNType.Quit:
                Debug.Log("Á¾·á");
                break;
        }
    }
    public void CanvasGroupOn(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }
    public void CanvasGroupOff(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }
}
