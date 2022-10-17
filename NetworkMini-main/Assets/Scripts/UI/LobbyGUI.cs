using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class LobbyGUI : MonoBehaviour
{
    [Header("Create Room")]
    public GameObject createRoomPopup;

    [Header("User Info")] 
    public TextMeshProUGUI userNameText;

    private NetworkManager networkManager;

    void Start()
    {
        networkManager = FindObjectOfType<NetworkManager>();

        // 처음에 팝업 닫혀 있음
        createRoomPopup.SetActive(false);

        UpdateUserInfo();
    }

    private void UpdateUserInfo()
    {
        userNameText.text = PhotonNetwork.LocalPlayer.NickName;
    }

    void Update()
    {
        // 엔터 누르면 채팅창 활성화
    }

    public void QuickJoin()
    {
        networkManager.JoinRandomRoom();
    }

    public void CreateRoom()
    {
        // 방 만들기
        networkManager.CreateRoom();
    }
}
