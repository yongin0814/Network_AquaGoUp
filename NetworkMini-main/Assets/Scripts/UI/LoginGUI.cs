using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoginGUI : MonoBehaviour
{
    private const string UserNamePlayerPref = "NetworkProjectPref";

    private NetworkManager chatNewComponent;

    public TMP_InputField idInput;
    public GameObject loginPanel;

    public void Start()
    {
        chatNewComponent = FindObjectOfType<NetworkManager>();
        print("In Login");

        string prefsName = PlayerPrefs.GetString(UserNamePlayerPref);
        if (!string.IsNullOrEmpty(prefsName))
        {
            idInput.text = prefsName;
            print("Get prefsName");
        }

        // 입력창 자동 활성화
        idInput.ActivateInputField();
        idInput.Select();
    }

    // new UI will fire "EndEdit" event also when loosing focus. So check "enter" key and only then StartConnect.
    public void EndEditOnEnter()
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            StartConnect();
        }
    }

    public void StartConnect()
    {
        NetworkManager chatNewComponent = FindObjectOfType<NetworkManager>();
        chatNewComponent.UserName = idInput.text.Trim();
        chatNewComponent.Connect();

        PlayerPrefs.SetString(UserNamePlayerPref, chatNewComponent.UserName);
        print("Set prefsName");

        loginPanel.SetActive(false);
    }
}
