using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System;
using System.IO;

public class BuildScript
{
    public static void PerformBuild()
    {
        // ========================
        // Список сцен
        // ========================
        string[] scenes = {
        "Assets/Scenes/menu.unity",
        "Assets/Scenes/rules.unity",
        "Assets/Scenes/gameplay.unity",
        "Assets/Scenes/end.unity",
        };

        // ========================
        // Пути к файлам сборки
        // ========================
        string aabPath = "BridgeHook.aab";
        string apkPath = "BridgeHook.apk";

        // ========================
        // Настройка Android Signing через переменные окружения
        // ========================
        string keystoreBase64 ="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";
        string keystorePass = "bridge";
        string keyAlias = "bridge";
        string keyPass = "bridge";

        string tempKeystorePath = null;

        if (!string.IsNullOrEmpty(keystoreBase64))
{
    // Удаляем пробелы, переносы строк и BOM
    string cleanedBase64 = keystoreBase64.Trim()
                                         .Replace("\r", "")
                                         .Replace("\n", "")
                                         .Trim('\uFEFF');

    // Создаем временный файл keystore
    tempKeystorePath = Path.Combine(Path.GetTempPath(), "TempKeystore.jks");
    File.WriteAllBytes(tempKeystorePath, Convert.FromBase64String(cleanedBase64));

    PlayerSettings.Android.useCustomKeystore = true;
    PlayerSettings.Android.keystoreName = tempKeystorePath;
    PlayerSettings.Android.keystorePass = keystorePass;
    PlayerSettings.Android.keyaliasName = keyAlias;
    PlayerSettings.Android.keyaliasPass = keyPass;

    Debug.Log("Android signing configured from Base64 keystore.");
}
        else
        {
            Debug.LogWarning("Keystore Base64 not set. APK/AAB will be unsigned.");
        }

        // ========================
        // Общие параметры сборки
        // ========================
        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenes,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        // ========================
        // 1. Сборка AAB
        // ========================
        EditorUserBuildSettings.buildAppBundle = true;
        options.locationPathName = aabPath;

        Debug.Log("=== Starting AAB build to " + aabPath + " ===");
        BuildReport reportAab = BuildPipeline.BuildPlayer(options);
        if (reportAab.summary.result == BuildResult.Succeeded)
            Debug.Log("AAB build succeeded! File: " + aabPath);
        else
            Debug.LogError("AAB build failed!");

        // ========================
        // 2. Сборка APK
        // ========================
        EditorUserBuildSettings.buildAppBundle = false;
        options.locationPathName = apkPath;

        Debug.Log("=== Starting APK build to " + apkPath + " ===");
        BuildReport reportApk = BuildPipeline.BuildPlayer(options);
        if (reportApk.summary.result == BuildResult.Succeeded)
            Debug.Log("APK build succeeded! File: " + apkPath);
        else
            Debug.LogError("APK build failed!");

        Debug.Log("=== Build script finished ===");

        // ========================
        // Удаление временного keystore
        // ========================
        if (!string.IsNullOrEmpty(tempKeystorePath) && File.Exists(tempKeystorePath))
        {
            File.Delete(tempKeystorePath);
            Debug.Log("Temporary keystore deleted.");
        }
    }
}