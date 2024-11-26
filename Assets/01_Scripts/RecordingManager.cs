using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class RecordingManager : MonoBehaviour
{
    private string serverIP = "15.164.255.131";
    // 서버 URL 설정
    private string uploadUrl;
    private string downloadUrl;
    private string testUrl;

    private string filePath;

    private void Start()
    {
        uploadUrl = $"http://{serverIP}:8000/uploadfiles/";
        downloadUrl = $"http://{serverIP}:8000/downloadfile/";
        testUrl = $"http://{serverIP}:8000/download-test/";

        // wav 파일 경로
        filePath = Application.persistentDataPath;
    }

    private void OnEnable()
    {
        // 활성화 됐다는건, 회의가 끝났다는 것! 
        DownloadTestFile();
        
    }

    // 테스트용 함수!!
    // 파일 다운로드를 처리하는 함수
    public void DownloadTestFile()
    {
        string savePath = Path.Combine(filePath, "downloaded_test.txt");
        StartCoroutine(DownloadTestCoroutine(savePath));
    }

    // 파일 다운로드를 위한 코루틴
    IEnumerator DownloadTestCoroutine(string savePath)
    {
        UnityWebRequest request = UnityWebRequest.Get(downloadUrl);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            byte[] fileData = request.downloadHandler.data;

            // 파일을 저장
            File.WriteAllBytes(savePath, fileData);
            Debug.Log("File download successful: " + savePath);
        }
        else
        {
            Debug.LogError("File download failed: " + request.error);
        }
    }



    // 파일 업로드를 처리하는 함수
    public void UploadAllFiles()
    {
        // 파일 경로에서 모든 파일 가져오기
        string[] filePaths = Directory.GetFiles(filePath);
        StartCoroutine(UploadFilesCoroutine(filePaths));
    }

    // 파일 다운로드를 처리하는 함수
    public void DownloadTxtFile()
    {
        string savePath = Path.Combine(filePath, "downloaded_test.txt");
        StartCoroutine(DownloadFileCoroutine(savePath));
    }

    // 파일 업로드를 위한 코루틴
    IEnumerator UploadFilesCoroutine(string[] filePaths)
    {
        UnityWebRequest request = new UnityWebRequest(uploadUrl, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.uploadHandler = new UploadHandlerRaw(new byte[0]);

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();

        foreach (string filePath in filePaths)
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            string fileName = Path.GetFileName(filePath);

            formData.Add(new MultipartFormFileSection("files", fileData, fileName, "application/octet-stream"));
        }

        byte[] boundary = UnityWebRequest.GenerateBoundary();
        request.uploadHandler = new UploadHandlerRaw(UnityWebRequest.SerializeFormSections(formData, boundary));
        request.SetRequestHeader("Content-Type", "multipart/form-data; boundary=" + System.Text.Encoding.UTF8.GetString(boundary));

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("File upload successful: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("File upload failed: " + request.error);
        }
    }

    // 파일 다운로드를 위한 코루틴
    IEnumerator DownloadFileCoroutine(string savePath)
    {
        UnityWebRequest request = UnityWebRequest.Get(downloadUrl);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            byte[] fileData = request.downloadHandler.data;

            // 파일을 저장
            File.WriteAllBytes(savePath, fileData);
            Debug.Log("File download successful: " + savePath);
        }
        else
        {
            Debug.LogError("File download failed: " + request.error);
        }
    }
}
