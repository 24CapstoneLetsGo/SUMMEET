using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEmotions : MonoBehaviour
{
    private Animator _animator;

    void Start()
    {
        // Animator ������Ʈ ��������
        _animator = GetComponentInChildren<Animator>();

        // voice Manager �� animator ���
        GameObject.FindGameObjectWithTag("VoiceManager").GetComponent<PushToTalkWithRecording>().SetAnimator(transform.gameObject);
    }

    void Update()
    {
        // ���� Ű �Է� ���� �� �ִϸ��̼� Ʈ���� ����
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _animator.SetTrigger("Clap");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _animator.SetTrigger("ThumbsUp");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _animator.SetTrigger("Laugh");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _animator.SetTrigger("Disbelief");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _animator.SetTrigger("Angry");
        }
    }
}
