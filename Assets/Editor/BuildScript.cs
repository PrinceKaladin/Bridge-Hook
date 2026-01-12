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
        "Assets/Scenes/gameplay.unity",
        "Assets/Scenes/rules.unity",
        "Assets/Scenes/end.unity",
        };

        // ========================
        // Пути к файлам сборки
        // ========================
        string aabPath = "Tapulse.aab";
        string apkPath = "Tapulse.apk";

        // ========================
        // Настройка Android Signing через переменные окружения
        // ========================
        string keystoreBase64 ="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";
        string keystorePass = "tapulse";
        string keyAlias = "tapulse";
        string keyPass = "tapulse";

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