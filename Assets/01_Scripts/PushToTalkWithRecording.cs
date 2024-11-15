using UnityEngine;
using Photon.Voice.Unity;
using System.IO;
using Photon.Pun; // Photon ���� ���̺귯��

public class PushToTalkWithRecording : MonoBehaviour
{
    private Recorder recorder;
    private AudioClip recordedClip;
    private bool isRecording = false;
    private int recordingStartPosition = 0;

    [Header("fileName")]
    private int speechCount;
    private string playerName;

    // �ִϸ��̼� 
    public Animator _animator; // Animator�� �ܺο��� �Ҵ��� �� �ֵ��� public���� ����

    void Start()
    {
        // Photon Voice Recorder ������Ʈ ��������
        recorder = GetComponent<Recorder>();

        // �⺻������ ���� ���� ��Ȱ��ȭ
        recorder.TransmitEnabled = false;

        // Photon���� �÷��̾� �г��� ��������
        playerName = PhotonNetwork.NickName;

    }

    void Update()
    {
        if (_animator == null) return; // Animator�� ������ Update ó������ ����

        // �����̽� Ű�� ������ ���� ���� �� ���� ����
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartRecording();
            recorder.TransmitEnabled = true; // Photon Voice ���� ����

            // ���ϱ� �ִϸ��̼� ���� (bool ������ ����)
            _animator.SetBool("IsTalking", true);

        }

        // �����̽� Ű�� ���� ���� ���� �� ���Ϸ� ����
        if (Input.GetKeyUp(KeyCode.R))
        {
            recorder.TransmitEnabled = false; // Photon Voice ���� ����
            StopRecordingAndSave();

            // ���ϱ� �ִϸ��̼� ����
            _animator.SetBool("IsTalking", false);

        }
    }

    // Player ������Ʈ�� �ε�� �� Animator�� �Ҵ��ϴ� �Լ�
    public void SetAnimator(GameObject playerObject)
    {
        if (playerObject != null && playerObject.GetComponent<PhotonView>().IsMine)
        {
            _animator = playerObject.GetComponent<Animator>();
            if (_animator == null)
            {
                Debug.LogError("Animator ������Ʈ�� ã�� �� �����ϴ�. Player ������Ʈ ������ Ȯ���ϼ���.");
            }
            else
            {
                Debug.Log("Animator ������Ʈ�� ���������� �Ҵ��߽��ϴ�.");
            }
        }
        else
        {
            Debug.LogError("Player ������Ʈ�� null�Դϴ�. Animator �Ҵ� ����.");
        }
    }


    // ���� ���� ����
    void StartRecording()
    {
        if (!isRecording)
        {
            // GameManager���� �߾� ��ȣ ��������
            speechCount = GameManager.Instance.GetNextSpeechCount();

            // �ִ� ���� �ð��� ��� ���� (��: 5��)
            recordedClip = Microphone.Start(null, false, 300, 44100);
            isRecording = true;

            // ������ ���۵� ���� ��ġ ����
            recordingStartPosition = Microphone.GetPosition(null);
        }
    }

    // ���� ���� �� ���� ������ ���̸�ŭ AudioClip �ڸ���
    void StopRecordingAndSave()
    {
        if (isRecording)
        {
            // ���� ����
            int recordingEndPosition = Microphone.GetPosition(null);
            Microphone.End(null);

            // ������ ������ ���̸� ���
            int actualSampleLength = recordingEndPosition - recordingStartPosition;


            // ������ ���� 
            Debug.Log("������ ����: "+actualSampleLength);

            // ���ο� AudioClip�� �����Ͽ� ���� ������ �κи� ����
            AudioClip trimmedClip = AudioClip.Create(recordedClip.name, actualSampleLength, recordedClip.channels, recordedClip.frequency, false);
            float[] data = new float[actualSampleLength];
            recordedClip.GetData(data, 0);
            trimmedClip.SetData(data, 0);

            // speechCount�� 4�ڸ� ���ڷ� ��ȯ
            string fileName = $"{speechCount.ToString("D4")}_{playerName}.wav";
            SaveAudioClip(trimmedClip, fileName);

            isRecording = false;
        }
    }

    // AudioClip�� WAV ���Ϸ� ����
    void SaveAudioClip(AudioClip clip, string filename)
    {
        var filePath = Path.Combine(Application.persistentDataPath, filename);
        SavWav.Save(filePath, clip);
        Debug.Log("Audio saved to " + filePath);
    }
}
