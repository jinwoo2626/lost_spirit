using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public GameObject MenuCam;
  //  public GameObject FireCam;
    public PlayerController WaterPlayer;
    public PlayerController FirePlayer;
    public GameObject StartMenu;
    public GameObject UIPanel;
    public Image Img1;
    public Image Img2;
    public Transform[] PuzzlePos;
    public GameObject[] Puzzleitem;

    bool isInstantiate = false;


    public void GameStartFire()
    {

        StartMenu.SetActive(false);
        UIPanel.SetActive(true);
        MenuCam.SetActive(false);        
        FirePlayer.gameObject.SetActive(true);
        WaterPlayer.gameObject.SetActive(false);
        Img1.color = new Color(1, 1, 1, 0);
        Img2.color = new Color(1, 1, 1, 0);
        isInstantiate = true;
        for (int i = 0; i < 4; i++)
        {
            Instantiate(Puzzleitem[0], PuzzlePos[i].position, Quaternion.identity);
        }

    }
    public void GameStartWater()
    {

        StartMenu.SetActive(false);
        UIPanel.SetActive(true);
        MenuCam.SetActive(false);
        FirePlayer.gameObject.SetActive(false);
        WaterPlayer.gameObject.SetActive(true);
        Img1.color = new Color(1, 1, 1, 0);
        Img2.color = new Color(1, 1, 1, 0);
        if (!isInstantiate)
        {
            for (int i = 0; i < 4; i++)
            {
                Instantiate(Puzzleitem[0], PuzzlePos[i].position, Quaternion.identity);
            }
        }
    }
    void LateUpdate()   //Update() 가 끝난 후 호출됨
    {

        Img1.color = new Color(1, 1, 1, WaterPlayer.cntitem[0] != 0 ? 1 : 0); //RGB는 건들이지 않고 alpha값만 변경
        Img2.color = new Color(1, 1, 1, WaterPlayer.cntitem[1] != 0 ? 1 : 0); //RGB는 건들이지 않고 alpha값만 변경
       
    }

}
