using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun; // Photon ���� ���̺귯��

public class Popup : MonoBehaviour
{
    public TMP_InputField topicInputField;    // Topic �Է¶�
    public TMP_InputField agendaInputField;   // Agenda �Է¶�
    public Button startButton;                // Start ��ư
    //public Button closeButton;                // Close ��ư

    void Start()
    {
        // ��ư Ŭ�� �̺�Ʈ ���
        startButton.onClick.AddListener(OnStartButtonClicked);
        //closeButton.onClick.AddListener(OnCloseButtonClicked);
    }

    // Start ��ư�� ������ �� ����Ǵ� �Լ�
    void OnStartButtonClicked()
    {
        string topic = topicInputField.text;
        string agenda = agendaInputField.text;

        // �Է��� ��� ������ �������� ����
        if (string.IsNullOrEmpty(topic) || string.IsNullOrEmpty(agenda))
        {
            Debug.LogWarning("Both fields must be filled.");
            return;
        }

        // txt ���Ϸ� ����
        SaveMeetingInfoToFile(topic, agenda);

        gameObject.SetActive(false); // �˾� â �ݱ�
    }

    // Close ��ư�� ������ �� ����Ǵ� �Լ� (�˾� �ݱ� ���)
    void OnCloseButtonClicked()
    {
        gameObject.SetActive(false); // �˾� â �ݱ�
    }

    // �ؽ�Ʈ ���Ϸ� ȸ�� ������ �����ϴ� �Լ�
    void SaveMeetingInfoToFile(string topic, string agenda)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "meeting_info.txt");

        // Photon Room ���� ��� �÷��̾��� �̸��� ��������
        string participants = "";
        foreach (var player in PhotonNetwork.PlayerList)
        {
            participants += player.NickName + ", "; // �г����� ��ǥ�� �����Ͽ� �߰�
        }

        // ������ ��ǥ�� ���� ���� (�����ڰ� �ִ� ��쿡��)
        if (participants.Length > 2)
        {
            participants = participants.Substring(0, participants.Length - 2); // ������ ", " ����
        }

        // ��¥ ���� ����
        string meetingDate = System.DateTime.Now.ToString("yyyy�� M�� d��");

        // �ؽ�Ʈ ���Ͽ� ������ ���� ������
        string content = $"ȸ�� ��¥: {meetingDate}\n" +
                         $"������: {participants}\n" +
                         $"ȸ�� ����: {topic}\n" +
                         $"�Ȱ�:\n{agenda.Replace("\n", "\n")}";

        // ���Ͽ� ����
        File.WriteAllText(filePath, content);

        Debug.Log("Meeting info saved to: " + filePath);
    }
}