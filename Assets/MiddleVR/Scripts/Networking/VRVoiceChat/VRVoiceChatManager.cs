using UnityEngine;
using UnityEngine.Networking;
using System.Diagnostics;
using System.Collections;

public class VRVoiceChatManager : NetworkBehaviour
{
    #region Attributes
    private NetworkManager m_NetworkManager = null;
    private Process m_WebRTCServerProcess = null;
    private vrWebView m_VRRTCWebView = null;
    private string m_RTCServerURL = "https://localhost:7778";

    private vrCommand m_RTCConnectionReadyCommand = null;

    public bool m_EnableVoiceChat = true;
    #endregion

    #region MonoBehaviour Integration
    protected void OnApplicationQuit()
    {
        if (isServer && m_WebRTCServerProcess != null)
        {
            Process.Start("cmd", "/C taskkill /f /t /pid " + m_WebRTCServerProcess.Id);
        }
    }

    private void OnDestroy()
    {
        MiddleVR.DisposeObject(ref m_RTCConnectionReadyCommand);
    }
    #endregion

    #region NetworkBehaviour Integration
    public override void OnStartServer()
    {
        if (vrClusterManager.GetInstance().IsClient() || !m_EnableVoiceChat)
            return;

        m_NetworkManager =  FindObjectOfType<NetworkManager>();

        var startInfo = new ProcessStartInfo();
        startInfo.UseShellExecute = false;
        startInfo.WindowStyle = ProcessWindowStyle.Normal;

        var pathToServer = MiddleVR.VRKernel.GetModuleFolder() + "\\MVRRTCServer";
        startInfo.FileName = pathToServer + "\\node.exe";
        startInfo.Arguments = pathToServer + "\\server.js " + (m_NetworkManager.networkPort + 1).ToString();

        m_WebRTCServerProcess = new Process();
        m_WebRTCServerProcess.StartInfo = startInfo;
        m_WebRTCServerProcess.Start();

        m_RTCServerURL = "https://" + m_NetworkManager.networkAddress + ":" + (m_NetworkManager.networkPort + 1).ToString();
    }

    public override void OnStartClient()
    {
        if (vrClusterManager.GetInstance().IsClient() || !m_EnableVoiceChat)
            return;

        if (!isServer)
        {
            m_NetworkManager =  FindObjectOfType<NetworkManager>();
            m_RTCServerURL = "https://" + m_NetworkManager.networkAddress + ":" + (m_NetworkManager.networkPort + 1).ToString();
        }

        m_RTCConnectionReadyCommand = new vrCommand("RTCConnectionReady", RTCConnectionReadyCommandHandler, null, (uint)VRCommandFlags.VRCommandFlag_DontSynchronizeCluster);

        StartCoroutine(CreateWebView());

    }
    #endregion

    private IEnumerator CreateWebView()
    {
        bool serverReady = false;
        float time = Time.time;

        do
        {
            WWW www = new WWW(m_RTCServerURL);
            yield return www;

            if (string.IsNullOrEmpty(www.error) ||
                (www.error.IndexOf("Could not resolve host", 0) < 0 &&
                 www.error.IndexOf("Connection refused", 0) < 0))
            {
                serverReady = true;
            }
            else
            {
                yield return new WaitForSeconds(10.0f);
            }
        } while (!serverReady && Time.time - time < 60.0f);

        if (serverReady)
        {
            m_VRRTCWebView = new vrWebView("", m_RTCServerURL);
        }
    }

    private vrValue RTCConnectionReadyCommandHandler(vrValue iValue)
    {
        if (isServer)
        {
            m_VRRTCWebView.ExecuteJavascript("OpenRoom('MVRAudioChat', '" + m_RTCServerURL + "');");
        }
        else
        {
            m_VRRTCWebView.ExecuteJavascript("JoinRoom('MVRAudioChat', '" + m_RTCServerURL + "');");
        }

        return null;
    }
}
