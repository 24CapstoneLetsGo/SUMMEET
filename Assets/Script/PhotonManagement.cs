using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManagement : MonoBehaviourPunCallbacks
{

    //버전 입력
    private readonly string version = "0.1f";
    // 사용자 아이디 입력
    private string userId = "Mary";

    private void Awake()
    {
        //같은 룸의 유저들에게 자동으로 씬을 로딩
        PhotonNetwork.AutomaticallySyncScene = true;
        //같은 버전의 유저끼리 접속 허용 
        PhotonNetwork.GameVersion = version;
        //유저 아이디 할당
        PhotonNetwork.NickName = userId;

        //포톤 서버와 통신 횟수 설정. 초당 30회
        Debug.Log(PhotonNetwork.SendRate);

        //서버 접속
        PhotonNetwork.ConnectUsingSettings();
    }

    //포톤 서버에 접속 후 호출되는 콜백 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master!");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby(); // 로비 입장 
    }

    //로비 접속 후 호출되는 콜백함수
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinRandomRoom(); // 랜덤 매치메이킹 기능 제공 
    }

    // 랜덤 룸 입장 실패시 호출되는 함수
    public override void OnJoinRandomFailed(short returncode, string message)
    {
        Debug.Log($"JoinRandom Failed {returncode}:{message}");


        // 룸의 속성 정의 
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20; // 최대 접속자 수 20명
        ro.IsOpen = true; // 룸의 오픈 여부
        ro.IsVisible = true; // 로비에서 룸 목록에 노출 시킬 것인지 여부 

        PhotonNetwork.CreateRoom("My room", ro);

    }

    // 룸 생성 후 콜백 함수 
    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
        Debug.Log($"Room name = {PhotonNetwork.CurrentRoom.Name}");
    }

    //룸 입장 후 콜백 함수
    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player count = {PhotonNetwork.CurrentRoom.PlayerCount}");


    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
