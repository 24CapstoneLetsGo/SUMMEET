using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    public float pushDistance = 0.5f; // 의자가 이동할 거리 (로컬 z 축으로)
    public float pushDuration = 0.4f; // 의자가 뒤로 가는 시간
    public float returnDuration = 0.8f; // 의자가 제자리로 돌아오는 시간
    public float waitDuration = 0.5f; // 뒤로 간 상태에서 대기하는 시간

    private Transform chairMesh; // 의자의 시각적 부분
    private Vector3 originalLocalPosition;

    void Start()
    {
        // 자식 오브젝트로 있는 mesh를 가져옵니다.
        chairMesh = transform.GetChild(1);
        if (chairMesh != null)
        {
            originalLocalPosition = chairMesh.localPosition;
            Debug.Log("의자 mesh 찾음");
        }
        else
        {
            Debug.LogError("ChairMesh 오브젝트를 찾을 수 없습니다. 의자에 ChairMesh라는 이름의 자식 오브젝트를 추가하세요.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (chairMesh != null)
            {
                // 플레이어가 의자와 닿았을 때 의자를 뒤로 빼는 코루틴 실행
                StartCoroutine(PushAndReturnChair());
            }
        }
    }

    private IEnumerator PushAndReturnChair()
    {
        // 의자를 뒤로 이동시키기
        Vector3 targetPosition = originalLocalPosition + new Vector3(pushDistance, 0, 0);
        float elapsedTime = 0;

        while (elapsedTime < pushDuration)
        {
            chairMesh.localPosition = Vector3.Lerp(originalLocalPosition, targetPosition, elapsedTime / pushDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        chairMesh.localPosition = targetPosition;

        // 일정 시간 대기
        yield return new WaitForSeconds(waitDuration);

        // 일정 시간 대기 후 의자를 제자리로 이동시키기
        elapsedTime = 0;
        while (elapsedTime < returnDuration)
        {
            chairMesh.localPosition = Vector3.Lerp(targetPosition, originalLocalPosition, elapsedTime / returnDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        chairMesh.localPosition = originalLocalPosition;
    }
}
