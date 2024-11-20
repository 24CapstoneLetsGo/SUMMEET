using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class RecordingManager : MonoBehaviour
{
    // ���� URL ����
    private string uploadUrl = "http://localhost:8000/uploadfiles/";
    private string downloadUrl = "http://localhost:8000/downloadfile/";

    private string filePath;

    private void Start()
    {
        // wav ���� ���
        filePath = Application.persistentDataPath;
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
