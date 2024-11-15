using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    public float pushDistance = 0.5f; // ���ڰ� �̵��� �Ÿ� (���� z ������)
    public float pushDuration = 0.4f; // ���ڰ� �ڷ� ���� �ð�
    public float returnDuration = 0.8f; // ���ڰ� ���ڸ��� ���ƿ��� �ð�
    public float waitDuration = 0.5f; // �ڷ� �� ���¿��� ����ϴ� �ð�

    private Transform chairMesh; // ������ �ð��� �κ�
    private Vector3 originalLocalPosition;

    void Start()
    {
        // �ڽ� ������Ʈ�� �ִ� mesh�� �����ɴϴ�.
        chairMesh = transform.GetChild(1);
        if (chairMesh != null)
        {
            originalLocalPosition = chairMesh.localPosition;
            Debug.Log("���� mesh ã��");
        }
        else
        {
            Debug.LogError("ChairMesh ������Ʈ�� ã�� �� �����ϴ�. ���ڿ� ChairMesh��� �̸��� �ڽ� ������Ʈ�� �߰��ϼ���.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (chairMesh != null)
            {
                // �÷��̾ ���ڿ� ����� �� ���ڸ� �ڷ� ���� �ڷ�ƾ ����
                StartCoroutine(PushAndReturnChair());
            }
        }
    }

    private IEnumerator PushAndReturnChair()
    {
        // ���ڸ� �ڷ� �̵���Ű��
        Vector3 targetPosition = originalLocalPosition + new Vector3(pushDistance, 0, 0);
        float elapsedTime = 0;

        while (elapsedTime < pushDuration)
        {
            chairMesh.localPosition = Vector3.Lerp(originalLocalPosition, targetPosition, elapsedTime / pushDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        chairMesh.localPosition = targetPosition;

        // ���� �ð� ���
        yield return new WaitForSeconds(waitDuration);

        // ���� �ð� ��� �� ���ڸ� ���ڸ��� �̵���Ű��
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
