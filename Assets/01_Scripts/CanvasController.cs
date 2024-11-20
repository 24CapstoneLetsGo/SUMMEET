using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{

    // 로그아웃 버튼이 눌렸을 때 호출될 메서드 - 지금은 임시로 게임 종료로 연결해둠 
    public void OnLogoutButtonPressed()
    {
        // 게임 종료 로직
        Debug.Log("Logging out and quitting the game.");
        Application.Quit();

        // 에디터에서 실행 중일 때 게임 종료를 시뮬레이션
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }


}
