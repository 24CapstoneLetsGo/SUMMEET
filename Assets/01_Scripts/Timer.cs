using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // TextMeshPro 컴포넌트 (UI용)
    private float timer = 0f; // 타이머 값 (초 단위)
    private bool isRunning = false; // 타이머 실행 여부

    private void OnEnable()
    {
        StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            timer += Time.deltaTime; // 시간 누적
            UpdateTimerText(); // 텍스트 갱신
        }
    }

    // 타이머 시작
    public void StartTimer()
    {
        timer = 0f; // 초기화
        isRunning = true;
    }

    // 타이머 정지
    public void StopTimer()
    {
        isRunning = false;
    }

    // 타이머 텍스트 업데이트
    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timer / 60); // 분 계산
        int seconds = Mathf.FloorToInt(timer % 60); // 초 계산
        timerText.text = $"{minutes:00}:{seconds:00}"; // MM:SS 포맷
    }

}
