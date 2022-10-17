using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

// MonoBehavior는 일반적인 이벤트 상태의 처리는 하지만 네트워크 처리는 하지 못함
// MonoBehavior와 같은 기능을 하면서 네트워크 기
public class Login : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject loginUI;
    [SerializeField] private Text loginText;

    void Start()
    {
        PhotonNetwork.GameVersion = "0.1";
        // 네트워크 객체들끼리 씬 자동 동기화
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void LoginFunc()
    {
        PhotonNetwork.ConnectUsingSettings();
        loginText.text = "접속 시도";
    }

    public override void OnConnectedToMaster()
    {
        loginText.text = "접속 성공";
        // Error : 방이 만들어지지 않은 상태에서 방에 진입 시도
        PhotonNetwork.JoinRandomRoom();
    }

    // 방 만들기 실패 시 방 만들기
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        loginText.text = "방 만들기 실패해서 내가 방 만듦";
        PhotonNetwork.CreateRoom(null, new RoomOptions{ MaxPlayers = 3 });
    }

    public override void OnJoinedRoom()
    {
        loginText.text = "방에 들어감";
        loginUI.SetActive(false);

        Vector2 pos = UnityEngine.Random.insideUnitCircle * 1.5f;
        PhotonNetwork.Instantiate("Player", new Vector3(pos.x, 1, pos.y), Quaternion.identity);
    }
}
