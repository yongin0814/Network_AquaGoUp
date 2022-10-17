using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class AquaSpawnManager : MonoBehaviour
{
    public static AquaSpawnManager Instance { get; private set; }

    public Transform[] spawnPoses;

    public PlayerMove[] players;

    

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        var playerNum = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        var player = PhotonNetwork.Instantiate("Aqua/Player", spawnPoses[playerNum].position, Quaternion.identity,
            0, new object[] { PhotonNetwork.LocalPlayer.ActorNumber, PhotonNetwork.LocalPlayer.NickName });
        print($"Spawn player - {playerNum}");
        Camera.main.GetComponent<CameraFollow>().SetTarget(player.transform);
    }
}