using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterInfoPanel : MonoBehaviour {

    public Text infoText;
    public InputField pwField;
    public Button cancelButton;
    public Button submitButton;


    // Use this for initialization
    public void GetPassword (LobbyServerEntry e) {
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() => { gameObject.SetActive(false); });

        submitButton.onClick.RemoveAllListeners();
        submitButton.onClick.AddListener(() =>
        {
            e.submittedPW = pwField.text;
            gameObject.SetActive(false);
        });

        gameObject.SetActive(true);
    }
}
