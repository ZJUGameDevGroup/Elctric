using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    //响应游戏开始事按钮件
    public void OnButtonGameStart()
    {
        SceneManager.LoadScene("level_01");  //读取关卡level1
    }
    public void OnButtonExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
