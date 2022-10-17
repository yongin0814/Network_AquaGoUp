using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class ChattingScroll : MonoBehaviourPun
{
    [Header("UI Components")]
    public TMP_InputField chatInputField;
    public ScrollRect chatScrollView;
    public TextMeshProUGUI chatText;
    public Button scrollUpButton;
    public Button scrollDownButton;

    public bool startActivated = true;

    public bool Activated
    {
        get => _activated;
        set
        {
            if (_activated != value)
            {
                print($"Swap Activated - {value}");
                _activated = value;
                chatInputField.Select();
            }
        }
    }

    private bool _activated = false;

    public string CurrentInput => chatInputField.text;

    private void Start()
    {
        // �ڵ� Ȱ��ȭ
        Activated = startActivated;
    }

    public void ScrollUp()
    {
        //chatText.
    }

    public void ScrollDown()
    {

    }

    /// <summary>
    /// �÷��̾��� ä�� ������ ��ü���� ������
    /// </summary>
    public void PlayerChat()
    {
        // ä�� ���� ��ü���� ����
        photonView.RPC(nameof(ChatRPC), RpcTarget.All, PhotonNetwork.LocalPlayer.NickName + " : " + CurrentInput);

        // �ڱ� ä���� Ŭ����
        chatInputField.text = "";

        // ��ũ�� ��ư Ȱ��ȭ ��Ȱ��ȭ ����

        // �ڵ� Ȱ��ȭ ��Ȱ��ȭ ����

    }

    [PunRPC]
    private void ChatRPC(string message)
    {
        chatText.text += message + "\n";
    }
}
