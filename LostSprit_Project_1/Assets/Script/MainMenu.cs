using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BTNType
{
    Fire,
    Water,
    Quit
}
public class MainMenu : MonoBehaviour
{
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickNewGame()
    {

    }

    public void OnClickLoad()
    {

    }

    public void OnClickOption()
    {

    }

    public void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }


}
