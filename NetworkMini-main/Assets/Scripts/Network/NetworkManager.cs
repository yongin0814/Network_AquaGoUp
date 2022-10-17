using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Random = UnityEngine.Random;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance { get; private set; }

    public event Action<Player> onPlayerEnteredRoom;
    public event Action<Player> onPlayerLeftRoom;

    public string UserName { get; set; }

    [Header("Login")] 
    [SerializeField] private LoginGUI loginUI;
    [Header("Lobby")]
    [SerializeField] private LobbyGUI lobbyUI;
    [Header("Room")]
    [SerializeField] private RoomGUI roomUI;

    [Header("Debug")]
    [SerializeField] private TextMeshProUGUI connectStateText;

    List<RoomInfo> roomInfos = new List<RoomInfo>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        PhotonNetwork.GameVersion = "0.1";
        // 네트워크 객체들끼리 씬 자동 동기화
        PhotonNetwork.AutomaticallySyncScene = true;

        if (PhotonNetwork.InRoom)
        {
            print("Room State");
            ActiveNetworkStateUI();
        }
        else if (PhotonNetwork.InLobby)
        {
            print("Lobby State");
            ActiveNetworkStateUI();
        }
        else
        {
            print("Login State");
            ActiveNetworkStateUI();
        }

        // 상태 체크
        StartCoroutine(UpdateNetworkState());
    }

    private void ActiveNetworkStateUI()
    {
        roomUI.gameObject.SetActive(PhotonNetwork.InRoom);
        lobbyUI.gameObject.SetActive(PhotonNetwork.InLobby);
        loginUI.gameObject.SetActive(!PhotonNetwork.InLobby && !PhotonNetwork.InRoom);
    }

    IEnumerator UpdateNetworkState()
    {
        while (true)
        {
            connectStateText.text = PhotonNetwork.NetworkClientState.ToString();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Connect()
    {
        PhotonNetwork.LocalPlayer.NickName = UserName;
        PhotonNetwork.ConnectUsingSettings();
        print($"Connect - {UserName}");
    }

    public override void OnConnectedToMaster()
    {
        print("Connected");
        PhotonNetwork.JoinLobby();
        connectStateText.text = "Connected";
    }

    public override void OnJoinedLobby()
    {
        print("Joined Lobby");
        ActiveNetworkStateUI();
    }

    public void CreateRoom()
    {
        //roomInput.text = string.IsNullOrEmpty(roomInput.text)
        //    ? "Room" + Random.Range(0, 100)
        //    : roomInput.text;
        PhotonNetwork.CreateRoom("Room Name", new RoomOptions { MaxPlayers = 4 });
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnCreatedRoom()
    {
        print("Create Room");

    }

    /// <summary>
    /// 방에 들어온 경우 - 방을 생성해서 들어온 경우는 제외
    /// </summary>
    public override void OnJoinedRoom()
    {
        print("Join Room");
        ActiveNetworkStateUI();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        print($"{newPlayer.NickName} entered room");
        onPlayerEnteredRoom?.Invoke(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        print($"{otherPlayer.NickName} left room");
        onPlayerLeftRoom?.Invoke(otherPlayer);
    }

    //랜덤으로 들어갈 방이 없을 때
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectStateText.text = "Fail Join Random Room";
        CreateRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {

    }

    public void SendChatMessage(string message)
    {
        photonView.RPC(nameof(ChatRPC), RpcTarget.All, message);
    }

    [PunRPC]
    void ChatRPC(string message)
    {
        //chatManager.PlayerChat(message);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
