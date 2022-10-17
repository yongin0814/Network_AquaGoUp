using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class DebugConnect_GoUp : MonoBehaviourPunCallbacks
{
    private readonly string version = "0.1";

    public UnityEvent onJoinedRoom = new UnityEvent();
    public UnityEvent onPlayerEnteredRoom = new UnityEvent();

    void Start()
    {
        PhotonNetwork.GameVersion = version;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        print("Debug - connected");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        var option = new RoomOptions
        {
            MaxPlayers = 4,
            IsVisible = true,
            IsOpen = true
        };
        PhotonNetwork.JoinOrCreateRoom("Debug Go Up", option, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        // 플레이어 생성
        PhotonNetwork.LocalPlayer.NickName = "Player" + PhotonNetwork.LocalPlayer.ActorNumber;
        onJoinedRoom?.Invoke();
        GoUpSpawnManager.Instance.Spawn();
        //print($"Player Count - {PhotonNetwork.CurrentRoom.PlayerCount}");
        //GoUpGameSystem.Instance.StartGame();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // 디버그용
        // 방장이면 시작 명령
        if (PhotonNetwork.CurrentRoom.PlayerCount > 2 && PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            print($"Player Count - {PhotonNetwork.CurrentRoom.PlayerCount}");
            GoUpGameSystem.Instance.StartGame();
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("Join Room Failed?");
        var option = new RoomOptions
        {
            MaxPlayers = 3,
            IsVisible = true,
            IsOpen = true
        };
        PhotonNetwork.JoinOrCreateRoom("Test02", option, TypedLobby.Default);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        print($"Player {otherPlayer.ActorNumber} left the room");
    }
}
