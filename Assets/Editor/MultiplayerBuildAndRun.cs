using UnityEditor;
using UnityEngine;
using System.IO;

public class MultiplayerBuildAndRun
{
    [MenuItem("Tools/Run Multiplayer/Win64/3 Players")]
    static void PerformWin64Build3()
    {
        PerformWin64Build(2);
    }

    static void PerformWin64Build(int playerCount)
    {
        // ���� Ÿ���� Windows�� ����
        EditorUserBuildSettings.SwitchActiveBuildTarget(
            BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);

        // ���� ��ũ��Ʈ �߰� (�ػ� ���� ��ũ��Ʈ ����)
        string scriptPath = "Assets/ResolutionSetter.cs";
        CreateResolutionSetterScript(scriptPath);

        for (int i = 1; i <= playerCount; i++)
        {
            // �� �÷��̾��� ���� ��ο� ���� ���� �̸� ����
            BuildPipeline.BuildPlayer(GetScenePaths(),
                $"Builds/Win64/{GetProjectName()}{i}/" +
                $"{GetProjectName()}{i}.exe",
                BuildTarget.StandaloneWindows64, BuildOptions.AutoRunPlayer);
        }

        // ���尡 ���� �� �ӽ� ��ũ��Ʈ ����
        File.Delete("Assets/ResolutionSetter.cs");
        AssetDatabase.Refresh();
    }

    static string GetProjectName()
    {
        string[] s = Application.dataPath.Split('/');
        return s[s.Length - 2];
    }

    static string[] GetScenePaths()
    {
        string[] scenes = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }
        return scenes;
    }

    static void CreateResolutionSetterScript(string path)
    {
        // �ػ� ���� ��ũ��Ʈ �ۼ�
        string scriptContent =
            "using UnityEngine;\n\n" +
            "public class ResolutionSetter : MonoBehaviour\n" +
            "{\n" +
            "    void Start()\n" +
            "    {\n" +
            "        Screen.SetResolution(3840, 2160, false);\n" + // â ���
            "        Debug.Log(\"�ػ� ����: 3840x2160 (4K UHD)\");\n" +
            "    }\n" +
            "}";

        File.WriteAllText(path, scriptContent);
        AssetDatabase.Refresh(); // Unity �����Ϳ� ��ũ��Ʈ �ݿ�
    }
}
