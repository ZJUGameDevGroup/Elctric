using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    //��Ӧ��Ϸ��ʼ�°�ť��
    public void OnButtonGameStart()
    {
        SceneManager.LoadScene("level_01");  //��ȡ�ؿ�level1
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
