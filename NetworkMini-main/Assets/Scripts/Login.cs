using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

// MonoBehavior�� �Ϲ����� �̺�Ʈ ������ ó���� ������ ��Ʈ��ũ ó���� ���� ����
// MonoBehavior�� ���� ����� �ϸ鼭 ��Ʈ��ũ ��
public class Login : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject loginUI;
    [SerializeField] private Text loginText;

    void Start()
    {
        PhotonNetwork.GameVersion = "0.1";
        // ��Ʈ��ũ ��ü�鳢�� �� �ڵ� ����ȭ
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void LoginFunc()
    {
        PhotonNetwork.ConnectUsingSettings();
        loginText.text = "���� �õ�";
    }

    public override void OnConnectedToMaster()
    {
        loginText.text = "���� ����";
        // Error : ���� ��������� ���� ���¿��� �濡 ���� �õ�
        PhotonNetwork.JoinRandomRoom();
    }

    // �� ����� ���� �� �� �����
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        loginText.text = "�� ����� �����ؼ� ���� �� ����";
        PhotonNetwork.CreateRoom(null, new RoomOptions{ MaxPlayers = 3 });
    }

    public override void OnJoinedRoom()
    {
        loginText.text = "�濡 ��";
        loginUI.SetActive(false);

        Vector2 pos = UnityEngine.Random.insideUnitCircle * 1.5f;
        PhotonNetwork.Instantiate("Player", new Vector3(pos.x, 1, pos.y), Quaternion.identity);
    }
}
