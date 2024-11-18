using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPun
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

    // 씬 전환
    public void LoadSceneCall()
    {
        StartCoroutine(LoadNextScene("MeetingRoom"));
    }

    // load scene - async
    private IEnumerator LoadNextScene(string sceneName)
    {
        // async load  scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // wait for done
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Debug.Log(sceneName+"Load Done");

        // 플레이어 복제 
        GameObject _player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }



}
