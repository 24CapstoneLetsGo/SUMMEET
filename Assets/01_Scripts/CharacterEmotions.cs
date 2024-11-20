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
        // ���� Ű �Է� ���� �� �ִϸ��̼� ���
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(PlayEmotion("Clap", 3.0f));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(PlayEmotion("ThumbsUp", 3.0f));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartCoroutine(PlayEmotion("Laugh", 3.0f));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StartCoroutine(PlayEmotion("Disbelief", 3.0f));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            StartCoroutine(PlayEmotion("Angry", 3.0f));
        }
    }

    // Ư�� �ִϸ��̼��� ���� �ð� ���� ����ϰ� �����ϴ� Coroutine
    private IEnumerator PlayEmotion(string animName, float duration)
    {
        // �ִϸ��̼� ��� (Bool�� ����)
        _animator.SetBool(animName, true);

        // ���� �ð� ���
        yield return new WaitForSeconds(duration);

        // �ִϸ��̼� ����
        _animator.SetBool(animName, false);
    }

    public void PlayAnim(string animName)
    {
        StartCoroutine(PlayEmotion(animName, 3.0f)); // 3�� ���� ��� �� ����
    }
}
