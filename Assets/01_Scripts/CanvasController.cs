using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{

    // �α׾ƿ� ��ư�� ������ �� ȣ��� �޼��� - ������ �ӽ÷� ���� ����� �����ص� 
    public void OnLogoutButtonPressed()
    {
        // ���� ���� ����
        Debug.Log("Logging out and quitting the game.");
        Application.Quit();

        // �����Ϳ��� ���� ���� �� ���� ���Ḧ �ùķ��̼�
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }


}
