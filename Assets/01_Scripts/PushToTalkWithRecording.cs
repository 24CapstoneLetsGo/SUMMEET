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
        // 스페이스 키를 누르면 음성 전송 및 녹음 시작
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartRecording();
            recorder.TransmitEnabled = true; // Photon Voice 전송 시작
        }

        // 스페이스 키를 떼면 녹음 종료 및 파일로 저장
        if (Input.GetKeyUp(KeyCode.Space))
        {
            recorder.TransmitEnabled = false; // Photon Voice 전송 종료
            StopRecordingAndSave();
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

            // 새로운 AudioClip을 생성하여 실제 녹음된 부분만 추출
            AudioClip trimmedClip = AudioClip.Create(recordedClip.name, actualSampleLength, recordedClip.channels, recordedClip.frequency, false);
            float[] data = new float[actualSampleLength];
            recordedClip.GetData(data, 0);
            trimmedClip.SetData(data, 0);

            // AudioClip을 파일로 저장
            //string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");

            // 파일명 생성 (발언번호_닉네임.wav)
            string fileName = $"{speechCount}_{playerName}.wav";
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
