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
        // 빌드 타겟을 Windows로 설정
        EditorUserBuildSettings.SwitchActiveBuildTarget(
            BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);

        // 공통 스크립트 추가 (해상도 설정 스크립트 생성)
        string scriptPath = "Assets/ResolutionSetter.cs";
        CreateResolutionSetterScript(scriptPath);

        for (int i = 1; i <= playerCount; i++)
        {
            // 각 플레이어의 빌드 경로와 실행 파일 이름 설정
            BuildPipeline.BuildPlayer(GetScenePaths(),
                $"Builds/Win64/{GetProjectName()}{i}/" +
                $"{GetProjectName()}{i}.exe",
                BuildTarget.StandaloneWindows64, BuildOptions.AutoRunPlayer);
        }

        // 빌드가 끝난 후 임시 스크립트 제거
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
        // 해상도 고정 스크립트 작성
        string scriptContent =
            "using UnityEngine;\n\n" +
            "public class ResolutionSetter : MonoBehaviour\n" +
            "{\n" +
            "    void Start()\n" +
            "    {\n" +
            "        Screen.SetResolution(3840, 2160, false);\n" + // 창 모드
            "        Debug.Log(\"해상도 설정: 3840x2160 (4K UHD)\");\n" +
            "    }\n" +
            "}";

        File.WriteAllText(path, scriptContent);
        AssetDatabase.Refresh(); // Unity 에디터에 스크립트 반영
    }
}
