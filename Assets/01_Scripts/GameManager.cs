using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPun, IPunObservable
{
    #region SingleTon Pattern
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        // If an instance already exists and it's not this one, destroy this one
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        // Set this as the instance and ensure it persists across scenes
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    // 발언 번호 카운터
    private int speechCount = 0;

    // 발언 번호 반환 및 카운터 증가
    public int GetNextSpeechCount()
    {
        if (photonView.IsMine) // 로컬 플레이어만 speechCount를 증가시킬 수 있음
        {
            speechCount++;
            Debug.Log($"New Speech Count: {speechCount}");
        }
        return speechCount;
    }

    // Photon PUN 동기화 메서드
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 로컬 플레이어가 데이터를 보내는 중일 때, speechCount 전송
            stream.SendNext(speechCount);
        }
        else
        {
            // 다른 클라이언트로부터 데이터를 받을 때, speechCount 동기화
            speechCount = (int)stream.ReceiveNext();
        }
    }
}
