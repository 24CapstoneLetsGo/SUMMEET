using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun; // Photon 관련 라이브러리

public class Popup : MonoBehaviour
{
    public TMP_InputField topicInputField;    // Topic 입력란
    public TMP_InputField agendaInputField;   // Agenda 입력란
    public Button startButton;                // Start 버튼
    public Button closeButton;                // Close 버튼

    void Start()
    {
        // 버튼 클릭 이벤트 등록
        startButton.onClick.AddListener(OnStartButtonClicked);
        closeButton.onClick.AddListener(OnCloseButtonClicked);
    }

    // Start 버튼을 눌렀을 때 실행되는 함수
    void OnStartButtonClicked()
    {
        string topic = topicInputField.text;
        string agenda = agendaInputField.text;

        // 입력이 비어 있으면 저장하지 않음
        if (string.IsNullOrEmpty(topic) || string.IsNullOrEmpty(agenda))
        {
            Debug.LogWarning("Both fields must be filled.");
            return;
        }

        // txt 파일로 저장
        SaveMeetingInfoToFile(topic, agenda);

        gameObject.SetActive(false); // 팝업 창 닫기
    }

    // Close 버튼을 눌렀을 때 실행되는 함수 (팝업 닫기 기능)
    void OnCloseButtonClicked()
    {
        gameObject.SetActive(false); // 팝업 창 닫기
    }

    // 텍스트 파일로 회의 정보를 저장하는 함수
    void SaveMeetingInfoToFile(string topic, string agenda)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "meeting_info.txt");

        // Photon Room 내의 모든 플레이어의 이름을 가져오기
        string participants = "";
        foreach (var player in PhotonNetwork.PlayerList)
        {
            participants += player.NickName + ", "; // 닉네임을 쉼표로 구분하여 추가
        }

        // 마지막 쉼표와 공백 제거 (참여자가 있는 경우에만)
        if (participants.Length > 2)
        {
            participants = participants.Substring(0, participants.Length - 2); // 마지막 ", " 제거
        }

        // 날짜 형식 설정
        string meetingDate = System.DateTime.Now.ToString("yyyy년 M월 d일");

        // 텍스트 파일에 저장할 내용 포맷팅
        string content = $"회의 날짜: {meetingDate}\n" +
                         $"참석자: {participants}\n" +
                         $"회의 주제: {topic}\n" +
                         $"안건:\n{agenda.Replace("\n", "\n")}";

        // 파일에 쓰기
        File.WriteAllText(filePath, content);

        Debug.Log("Meeting info saved to: " + filePath);
    }
}