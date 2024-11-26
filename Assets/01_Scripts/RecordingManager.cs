using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using System.Text;
using System.Text.RegularExpressions;

public class RecordingManager : MonoBehaviour
{
    private string serverIP = "15.164.255.131";
    // ���� URL ����
    private string uploadUrl;
    private string downloadUrl;
    private string testUrl;

    private string filePath;

    public TextMeshProUGUI summeet_text;

    private void Awake()
    {
        uploadUrl = $"http://{serverIP}:8000/uploadfiles/";
        downloadUrl = $"http://{serverIP}:8000/downloadfile/";
        testUrl = $"http://{serverIP}:8000/download-test/";

        // wav ���� ���
        filePath = Application.persistentDataPath;
    }

    private void OnEnable()
    {
        // Ȱ��ȭ �ƴٴ°�, ȸ�ǰ� �����ٴ� ��! 
        DownloadTestFile();
        
    }

    // �׽�Ʈ�� �Լ�!!
    // ���� �ٿ�ε带 ó���ϴ� �Լ�
    public void DownloadTestFile()
    {
        string savePath = Path.Combine(filePath, "downloaded_test.txt");
        StartCoroutine(DownloadTestCoroutine(savePath));
    }

    // ���� �ٿ�ε带 ���� �ڷ�ƾ
    IEnumerator DownloadTestCoroutine(string savePath)
    {
        UnityWebRequest request = UnityWebRequest.Get(testUrl);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            byte[] fileData = request.downloadHandler.data;

            // ���� �����͸� ���ڵ� 
            string fileDataString = Encoding.UTF8.GetString(fileData);

            // Ư�� ����(**�� ������ �ؽ�Ʈ) ó��
            string styledData = ApplyStyling(fileDataString);

            // ��Ÿ�� ����� �ؽ�Ʈ ui�� ǥ�� 
            summeet_text.text = styledData;

            // ������ ����
            File.WriteAllBytes(savePath, fileData);
            Debug.Log("File download successful: " + savePath);
        }
        else
        {
            Debug.LogError("File download failed: " + request.error);
        }
    }

    // ** �� ó���ϴ� �Լ� 
    string ApplyStyling(string rawText)
    {
        // ���Խ����� **�� ������ �κ��� ã�Ƽ� TextMeshPro �±�(<b><size=150%>)�� ��ȯ
        string styledText = Regex.Replace(
            rawText,
            @"\*\*(.*?)\*\*",
            @"<b><size=150%>$1</size></b>"
        );

        return styledText;
    }

    // ���� ���ε带 ó���ϴ� �Լ�
    public void UploadAllFiles()
    {
        // ���� ��ο��� ��� ���� ��������
        string[] filePaths = Directory.GetFiles(filePath);
        StartCoroutine(UploadFilesCoroutine(filePaths));
    }

    // ���� �ٿ�ε带 ó���ϴ� �Լ�
    public void DownloadTxtFile()
    {
        string savePath = Path.Combine(filePath, "downloaded_test.txt");
        StartCoroutine(DownloadFileCoroutine(savePath));
    }

    // ���� ���ε带 ���� �ڷ�ƾ
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

    // ���� �ٿ�ε带 ���� �ڷ�ƾ
    IEnumerator DownloadFileCoroutine(string savePath)
    {
        UnityWebRequest request = UnityWebRequest.Get(downloadUrl);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            byte[] fileData = request.downloadHandler.data;

            // ������ ����
            File.WriteAllBytes(savePath, fileData);
            Debug.Log("File download successful: " + savePath);
        }
        else
        {
            Debug.LogError("File download failed: " + request.error);
        }
    }
}
