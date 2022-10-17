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

        // ó���� �˾� ���� ����
        createRoomPopup.SetActive(false);

        UpdateUserInfo();
    }

    private void UpdateUserInfo()
    {
        userNameText.text = PhotonNetwork.LocalPlayer.NickName;
    }

    void Update()
    {
        // ���� ������ ä��â Ȱ��ȭ
    }

    public void QuickJoin()
    {
        networkManager.JoinRandomRoom();
    }

    public void CreateRoom()
    {
        // �� �����
        networkManager.CreateRoom();
    }
}
