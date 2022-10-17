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
        // 자동 활성화
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
    /// 플레이어의 채팅 내용을 전체에게 보낸다
    /// </summary>
    public void PlayerChat()
    {
        // 채팅 내용 전체한테 전파
        photonView.RPC(nameof(ChatRPC), RpcTarget.All, PhotonNetwork.LocalPlayer.NickName + " : " + CurrentInput);

        // 자기 채팅은 클리어
        chatInputField.text = "";

        // 스크롤 버튼 활성화 비활성화 결정

        // 자동 활성화 비활성화 결정

    }

    [PunRPC]
    private void ChatRPC(string message)
    {
        chatText.text += message + "\n";
    }
}
