using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class DebugConnect : MonoBehaviourPunCallbacks
{
    public UnityEvent onJoinedRoom = new UnityEvent();
    public UnityEvent onPlayerEnteredRoom = new UnityEvent();

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        print("Debug - connected");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        print("Debug - On Joined Lobby");
        var option = new RoomOptions
        {
            MaxPlayers = 4,
            IsVisible = true,
            IsOpen = true
        };
        PhotonNetwork.JoinOrCreateRoom("Test01", option, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        print($"Debug - On Joined Room : {PhotonNetwork.CurrentRoom.Name}");
        PhotonNetwork.LocalPlayer.NickName = $"Player {PhotonNetwork.LocalPlayer.ActorNumber}";
        onJoinedRoom?.Invoke();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // ����׿�
        // �����̸� ���� ���
        onPlayerEnteredRoom?.Invoke();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        var option = new RoomOptions
        {
            MaxPlayers = 3,
            IsVisible = true,
            IsOpen = true
        };
        PhotonNetwork.JoinOrCreateRoom("Test02", option, TypedLobby.Default);
    }
}
