using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEmotions : MonoBehaviour
{
    private Animator _animator;

    void Start()
    {
        // Animator 컴포넌트 가져오기
        _animator = GetComponentInChildren<Animator>();

        // voice Manager 에 animator 등록
        GameObject.FindGameObjectWithTag("VoiceManager").GetComponent<PushToTalkWithRecording>().SetAnimator(transform.gameObject);
    }

    void Update()
    {
        // 숫자 키 입력 감지 및 애니메이션 트리거 설정
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
