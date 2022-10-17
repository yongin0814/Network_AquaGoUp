using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Photon.Pun;
using Photon.Realtime;

public class GoUpSpawnManager : MonoBehaviourPun
{
    public static GoUpSpawnManager Instance { get; private set; }

    public GoUpPlayer[] players;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        var playerNum = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        print($"Spawn player - {playerNum}");
        photonView.RPC(nameof(SpawnRPC), RpcTarget.AllBuffered, 
            playerNum, PhotonNetwork.LocalPlayer.NickName);
    }

    [PunRPC]
    private void SpawnRPC(int playerNum, string playerName)
    {
        players[playerNum].gameObject.SetActive(true);
        players[playerNum].GetComponent<GoUpPlayer>().nameText.text = playerName;
    }
}
