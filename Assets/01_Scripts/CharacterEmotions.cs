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
        // 숫자 키 입력 감지 및 애니메이션 재생
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

    // 특정 애니메이션을 일정 시간 동안 재생하고 종료하는 Coroutine
    private IEnumerator PlayEmotion(string animName, float duration)
    {
        // 애니메이션 재생 (Bool로 설정)
        _animator.SetBool(animName, true);

        // 일정 시간 대기
        yield return new WaitForSeconds(duration);

        // 애니메이션 중지
        _animator.SetBool(animName, false);
    }

    public void PlayAnim(string animName)
    {
        StartCoroutine(PlayEmotion(animName, 3.0f)); // 3초 동안 재생 후 종료
    }
}
