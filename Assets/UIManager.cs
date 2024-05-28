using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region SingleTon Pattern
    public static UIManager instance;  // Singleton instance

    void Awake() // SingleTon
    {
        // 이미 인스턴스가 존재하면서 이게 아니면 파괴 반환
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // Set the instance to this object and make sure it persists between scene loads
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

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
