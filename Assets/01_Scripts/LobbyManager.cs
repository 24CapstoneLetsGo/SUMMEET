using UnityEngine;
using TMPro; // TextMeshPro ���� ���̺귯��
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField nicknameInputField; // �г��� �Է� �ʵ� (TextMeshPro)
    public TMP_InputField meetingRoomInputField; // ȸ�ǽ� �̸� �Է� �ʵ� (TextMeshPro)
    public Button joinButton; // Join ��ư

    private void Start()
    {
        // Join ��ư�� OnJoinRoom �Լ� ����
        joinButton.onClick.AddListener(OnJoinRoom);

    }

    private void OnJoinRoom()
    {
        // �г��� ����
        string nickname = nicknameInputField.text;
        if (string.IsNullOrEmpty(nickname))
        {
            Debug.LogWarning("�г����� �Է��� �ּ���.");
            return;
        }
        PhotonNetwork.NickName = nickname;

        // ȸ�ǽ� �̸� ����
        string roomName = meetingRoomInputField.text;
        if (string.IsNullOrEmpty(roomName))
        {
            Debug.LogWarning("ȸ�ǽ� �̸��� �Է��� �ּ���.");
            return;
        }

        // Photon ����
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            JoinOrCreateRoom(roomName);
        }
    }

    public override void OnConnectedToMaster()
    {
        // Photon ������ ����� �� ȸ�ǽ� ����
        string roomName = meetingRoomInputField.text;
        JoinOrCreateRoom(roomName);
    }

    private void JoinOrCreateRoom(string roomName)
    {
        // �Է��� ȸ�ǽ� �̸����� �뿡 �����ϰų� ������ ����
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 6 }; // �ִ� 6�� ����
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"ȸ�ǽ� '{PhotonNetwork.CurrentRoom.Name}'�� �����߽��ϴ�.");

        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player count = {PhotonNetwork.CurrentRoom.PlayerCount}");

        // Meeting Room Scene���� ��ȯ
        GameManager.Instance.LoadSceneCall();
    }
}
