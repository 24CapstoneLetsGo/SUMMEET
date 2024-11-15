using UnityEngine;
using Photon.Voice.Unity;
using System.IO;
using Photon.Pun; // Photon 관련 라이브러리

public class PushToTalkWithRecording : MonoBehaviour
{
    private Recorder recorder;
    private AudioClip recordedClip;
    private bool isRecording = false;
    private int recordingStartPosition = 0;

    [Header("fileName")]
    private int speechCount;
    private string playerName;

    // 애니메이션 
    public Animator _animator; // Animator를 외부에서 할당할 수 있도록 public으로 변경

    void Start()
    {
        // Photon Voice Recorder 컴포넌트 가져오기
        recorder = GetComponent<Recorder>();

        // 기본적으로 음성 전송 비활성화
        recorder.TransmitEnabled = false;

        // Photon에서 플레이어 닉네임 가져오기
        playerName = PhotonNetwork.NickName;

    }

    void Update()
    {
        if (_animator == null) return; // Animator가 없으면 Update 처리하지 않음

        // 스페이스 키를 누르면 음성 전송 및 녹음 시작
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartRecording();
            recorder.TransmitEnabled = true; // Photon Voice 전송 시작

            // 말하기 애니메이션 실행 (bool 값으로 설정)
            _animator.SetBool("IsTalking", true);

        }

        // 스페이스 키를 떼면 녹음 종료 및 파일로 저장
        if (Input.GetKeyUp(KeyCode.R))
        {
            recorder.TransmitEnabled = false; // Photon Voice 전송 종료
            StopRecordingAndSave();

            // 말하기 애니메이션 종료
            _animator.SetBool("IsTalking", false);

        }
    }

    // Player 오브젝트가 로드된 후 Animator를 할당하는 함수
    public void SetAnimator(GameObject playerObject)
    {
        if (playerObject != null && playerObject.GetComponent<PhotonView>().IsMine)
        {
            _animator = playerObject.GetComponent<Animator>();
            if (_animator == null)
            {
                Debug.LogError("Animator 컴포넌트를 찾을 수 없습니다. Player 오브젝트 구조를 확인하세요.");
            }
            else
            {
                Debug.Log("Animator 컴포넌트를 성공적으로 할당했습니다.");
            }
        }
        else
        {
            Debug.LogError("Player 오브젝트가 null입니다. Animator 할당 실패.");
        }
    }


    // 음성 녹음 시작
    void StartRecording()
    {
        if (!isRecording)
        {
            // GameManager에서 발언 번호 가져오기
            speechCount = GameManager.Instance.GetNextSpeechCount();

            // 최대 녹음 시간을 길게 설정 (예: 5분)
            recordedClip = Microphone.Start(null, false, 300, 44100);
            isRecording = true;

            // 녹음이 시작된 샘플 위치 저장
            recordingStartPosition = Microphone.GetPosition(null);
        }
    }

    // 녹음 종료 및 실제 녹음된 길이만큼 AudioClip 자르기
    void StopRecordingAndSave()
    {
        if (isRecording)
        {
            // 녹음 종료
            int recordingEndPosition = Microphone.GetPosition(null);
            Microphone.End(null);

            // 실제로 녹음된 길이를 계산
            int actualSampleLength = recordingEndPosition - recordingStartPosition;


            // 녹음된 길이 
            Debug.Log("녹음된 길이: "+actualSampleLength);

            // 새로운 AudioClip을 생성하여 실제 녹음된 부분만 추출
            AudioClip trimmedClip = AudioClip.Create(recordedClip.name, actualSampleLength, recordedClip.channels, recordedClip.frequency, false);
            float[] data = new float[actualSampleLength];
            recordedClip.GetData(data, 0);
            trimmedClip.SetData(data, 0);

            // speechCount를 4자리 숫자로 변환
            string fileName = $"{speechCount.ToString("D4")}_{playerName}.wav";
            SaveAudioClip(trimmedClip, fileName);

            isRecording = false;
        }
    }

    // AudioClip을 WAV 파일로 저장
    void SaveAudioClip(AudioClip clip, string filename)
    {
        var filePath = Path.Combine(Application.persistentDataPath, filename);
        SavWav.Save(filePath, clip);
        Debug.Log("Audio saved to " + filePath);
    }
}
