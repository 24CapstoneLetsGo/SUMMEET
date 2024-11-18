using UnityEngine;
using TMPro; // TextMeshPro 관련 라이브러리
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField nicknameInputField; // 닉네임 입력 필드 (TextMeshPro)
    public TMP_InputField meetingRoomInputField; // 회의실 이름 입력 필드 (TextMeshPro)
    public Button joinButton; // Join 버튼

    private void Start()
    {
        // Join 버튼에 OnJoinRoom 함수 연결
        joinButton.onClick.AddListener(OnJoinRoom);

    }

    private void OnJoinRoom()
    {
        // 닉네임 설정
        string nickname = nicknameInputField.text;
        if (string.IsNullOrEmpty(nickname))
        {
            Debug.LogWarning("닉네임을 입력해 주세요.");
            return;
        }
        PhotonNetwork.NickName = nickname;

        // 회의실 이름 설정
        string roomName = meetingRoomInputField.text;
        if (string.IsNullOrEmpty(roomName))
        {
            Debug.LogWarning("회의실 이름을 입력해 주세요.");
            return;
        }

        // Photon 연결
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
        // Photon 서버에 연결된 후 회의실 입장
        string roomName = meetingRoomInputField.text;
        JoinOrCreateRoom(roomName);
    }

    private void JoinOrCreateRoom(string roomName)
    {
        // 입력한 회의실 이름으로 룸에 참가하거나 없으면 생성
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 6 }; // 최대 6명 접속
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"회의실 '{PhotonNetwork.CurrentRoom.Name}'에 접속했습니다.");

        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player count = {PhotonNetwork.CurrentRoom.PlayerCount}");

        // Meeting Room Scene으로 전환
        GameManager.Instance.LoadSceneCall();
    }
}
