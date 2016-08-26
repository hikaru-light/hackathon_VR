using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class Build
{
    // ビルド実行でAndroidのapkを作成する例
    [UnityEditor.MenuItem("Builds/Build Project")]
    public static void BuildProject()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
        List<string> allScene = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                allScene.Add(scene.path);
            }
        }
        PlayerSettings.bundleIdentifier = "com.hoge.hoge";
        PlayerSettings.statusBarHidden = true;
        BuildPipeline.BuildPlayer(
            allScene.ToArray(),
            "test.apk",
            BuildTarget.Android,
            BuildOptions.None
        );
    }
}