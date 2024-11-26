using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // TextMeshPro ������Ʈ (UI��)
    private float timer = 0f; // Ÿ�̸� �� (�� ����)
    private bool isRunning = false; // Ÿ�̸� ���� ����

    private void OnEnable()
    {
        StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            timer += Time.deltaTime; // �ð� ����
            UpdateTimerText(); // �ؽ�Ʈ ����
        }
    }

    // Ÿ�̸� ����
    public void StartTimer()
    {
        timer = 0f; // �ʱ�ȭ
        isRunning = true;
    }

    // Ÿ�̸� ����
    public void StopTimer()
    {
        isRunning = false;
    }

    // Ÿ�̸� �ؽ�Ʈ ������Ʈ
    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timer / 60); // �� ���
        int seconds = Mathf.FloorToInt(timer % 60); // �� ���
        timerText.text = $"{minutes:00}:{seconds:00}"; // MM:SS ����
    }

}
