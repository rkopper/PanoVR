#if UNITY_5_3_OR_NEWER
using System.Diagnostics;
using UnityEngine;
using UnityEditor;
using System.IO;
using Debug = UnityEngine.Debug;

[InitializeOnLoad]
public class MiddleVR_UnityEngine_Networking_Check
{
    private const string CompanyName = "MiddleVR";
    private const string FileVersion = "0.1.0.0";

#if UNITY_5_3
    private const string InstallerName = "MiddleVR_UnityEngine.Networking_Installer5.3.exe";
#elif UNITY_5_4_OR_NEWER
    private const string InstallerName = "MiddleVR_UnityEngine.Networking_Installer5.4.exe";
#endif
    private const string UninstallerName = "MiddleVR_UnityEngine.Networking_Uninstaller.exe";

    static MiddleVR_UnityEngine_Networking_Check()
    {
        if(UnityEditorInternal.InternalEditorUtility.isHumanControllingUs)
            CheckUnityNetworking();
    }

    private static bool RunExe(string exePath, string arguments)
    {
        var uninstallerProcess = new Process();
        uninstallerProcess.StartInfo = new ProcessStartInfo(exePath.Replace("/", "\\"), arguments);

        uninstallerProcess.Start();
        uninstallerProcess.WaitForExit();

        return uninstallerProcess.ExitCode == 0;
    }

    private static void CheckUnityNetworking()
    {
        var installFolder = Path.Combine(EditorApplication.applicationContentsPath, "UnityExtensions/Unity/Networking");
        var localFolder = Path.Combine(Application.dataPath, "MiddleVR/Editor");

        System.Diagnostics.FileVersionInfo versionInfo = null;

        var targetNetworkingAssembly = Path.Combine(installFolder, "UnityEngine.Networking.dll");

        try
        {
            versionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(targetNetworkingAssembly);
        }
        catch (System.Exception)
        {
            Debug.Log("[X] MiddleVR failed to check version of '" + targetNetworkingAssembly + "'.");
            return;
        }

        
        var installerPath = Path.Combine(Path.Combine(Application.dataPath, "MiddleVR/Editor"), InstallerName);

        if (versionInfo.CompanyName != CompanyName)
        {
            var dialogMessage =
                "MiddleVR Networking is not present in your Unity installation, would you like to install it? Note: This will replace the default UNet module. See documentation for differences and limitations.";
            if (EditorUtility.DisplayDialog("MiddleVR", dialogMessage, "Install MiddleVR Networking", "Cancel"))
            {
                if (RunExe(installerPath, "/S /D="+installFolder.Replace("/", "\\")))
                {
                    var dialogSuccess = "MiddleVR Networking has successfully been installed!";
                    EditorUtility.DisplayDialog("MiddleVR", dialogSuccess, "OK");
                }
                else
                {
                    var dialogError = "An error has occured while installing MiddleVR Networking! Please contact MiddleVR Support.";
                    EditorUtility.DisplayDialog("MiddleVR", dialogError, "OK");
                }
            }
        }
        else if (versionInfo.FileVersion != FileVersion)
        {
            var dialogMessage =
                "Your installation uses a different version of MiddleVR Networking, would you like to replace it with the current version?";
            if (EditorUtility.DisplayDialog("MiddleVR", dialogMessage, "Replace MiddleVR Networking", "Cancel"))
            {
                var uninstallerPath = Path.Combine(installFolder, UninstallerName);
                if (RunExe(uninstallerPath, "/S"))
                {
                    RunExe(installerPath, "/S /D=" + installFolder.Replace("/", "\\"));
                }
            }
        }
    }
}
#endif
