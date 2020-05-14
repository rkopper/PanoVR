using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FileBrowser_Demo : MonoBehaviour
{
    public string _sStartPath;
    public FileBrowser_UI.FetchMode _eMode;
    public FileBrowser_UI.ToDisplay _eDisplay;
    public string _sDefaultExtension;
    public Text _tResult;

    void Update ()
    {
        if (!FileBrowser_UI.Instance._bIsOpen && Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(WaitForResult());
	}

    IEnumerator WaitForResult()
    {
        FileBrowser_UI.Instance.ShowWindow(_sStartPath, _eDisplay, _eMode, _sDefaultExtension);

        while (FileBrowser_UI.Instance._bIsOpen)
            yield return null;

        _tResult.text = FileBrowser_UI.Instance._sResult;
    }
}
