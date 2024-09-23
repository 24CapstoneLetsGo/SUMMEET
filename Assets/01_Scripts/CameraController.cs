using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public PhotonView photonView;

    void Start()
    {
        if (photonView.IsMine)
        {
            virtualCamera.enabled = true;  // 카메라 활성화
        }
        else
        {
            virtualCamera.enabled = false; // 다른 플레이어의 카메라 비활성화
        }
    }
}
