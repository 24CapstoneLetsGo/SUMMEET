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
            virtualCamera.enabled = true;  // ī�޶� Ȱ��ȭ
        }
        else
        {
            virtualCamera.enabled = false; // �ٸ� �÷��̾��� ī�޶� ��Ȱ��ȭ
        }
    }

}
