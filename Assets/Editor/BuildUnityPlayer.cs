using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class BuildUnityPlayer
{

    [MenuItem("MyTools/Windows and Android Build")]
    public static void PerformBuild()
    {
        // the scenes we want to include in the build
        string[] scenes = { "Assets/Scenes/MainMenu.unity",
                 "Assets/Scenes/Procedural.unity",
                 "Assets/Scenes/Multiplayer.unity"
             };
        string path = "E:\\Tom\\Documents\\GitHub\\UnityStars\\Build\\Windows\\";
        string buildName = "UnityStars!";
        // build for windows stand alone
        string windowsStandAloneBuildName = buildName + "-Standalone.exe";
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.StandaloneWindows);
        BuildPipeline.BuildPlayer(scenes, path + windowsStandAloneBuildName, BuildTarget.StandaloneWindows, BuildOptions.None);

        
        path = "E:\\Dropbox\\Dropbox\\";
        // build for Android
        string androidPlayerBuildName = buildName + "-Android.apk";
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
        BuildPipeline.BuildPlayer(scenes, path + androidPlayerBuildName, BuildTarget.Android, BuildOptions.None);
    }
}