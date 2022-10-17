using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomGUI : MonoBehaviourPun
{
    public TextMeshProUGUI roomName;


    [Header("Player")]
    public RoomPlayerInfoUI[] roomPlayers;

    [Header("Chatting")] 
    public ChattingScroll chattingScroll;

    [Header("Game Explain")] 
    public Image gameScreenShot;
    public TextMeshProUGUI gameName;
    public TextMeshProUGUI gameExplain;
    public GameData[] gameDatas;
    private int gameDataIndex = 0;

    [Header("Buttons")]
    public Button prevGameButton;
    public Button nextGameButton;
    public Button gameStartButton;

    private NetworkManager networkManager;

    void OnEnable()
    {
        // 네트워크 
        networkManager = FindObjectOfType<NetworkManager>();
        networkManager.onPlayerEnteredRoom += UpdateEnteredPlayerInfo;
        networkManager.onPlayerLeftRoom += UpdateLeftPlayerInfo;

        // 초기 세팅
        roomName.text = PhotonNetwork.CurrentRoom.Name;

        InitRoomPlayers();
        SetRoomMasterSetting();

        UpdateGameExplain();
    }

    private void OnDisable()
    {
        networkManager.onPlayerEnteredRoom -= UpdateEnteredPlayerInfo;
        networkManager.onPlayerLeftRoom -= UpdateLeftPlayerInfo;
    }

    private void Update()
    {
        // 처음 엔터를 누르면 Input Field 활성화
        // Input Field 활성화된 상태에서 엔터를 누르면 
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // 채팅 처리
            chattingScroll.PlayerChat();

            // 초기엔 채팅을 치는 상태가 아니다
            //if (!isChatting)
            //{
            //    // 채팅 활성화
            //    chatInputField.Select();
            //}
            //// InputField에 채팅을 치는 상태면 엔터 누르면 입력을 마친다
            //else
            //{
            //    PlayerChat("Player : " + chatInputField.text);
            //    chatInputField.Select();
            //}

            //// 반대로 전환
            //isChatting = !isChatting;
        }
    }

    // 룸 안에 있는 플레이어 정보 설정
    private void InitRoomPlayers()
    {
        int playerCount = PhotonNetwork.PlayerList.Length;
        print(playerCount);
        
        // 기존 방에 있는 멤버들 정보를 받음
        foreach (var player in PhotonNetwork.PlayerList)
        {
            print($"{player.NickName}'s actor number - {player.ActorNumber}");
            roomPlayers[player.ActorNumber - 1].SetPlayerInfo(player);
        }
        // 그 외는 사용 안 함 처리
        for (int i = 0; i < roomPlayers.Length; i++)
        {
            if (!roomPlayers[i].isUsed)
            {
                roomPlayers[i].DisableUI();
            }
        }
    }

    // 방장 정보 설정
    private void SetRoomMasterSetting()
    {
        bool isMasterClient = PhotonNetwork.LocalPlayer.IsMasterClient;

        gameStartButton.interactable = isMasterClient;
        prevGameButton.interactable = isMasterClient;
        nextGameButton.interactable = isMasterClient;
    }

    private void UpdateEnteredPlayerInfo(Photon.Realtime.Player newPlayer)
    {
        roomPlayers[newPlayer.ActorNumber-1].SetPlayerInfo(newPlayer); 
    }

    private void UpdateLeftPlayerInfo(Photon.Realtime.Player otherPlayer)
    {
        roomPlayers[otherPlayer.ActorNumber - 1].DisableUI();
    }

    #region Game Select
    public void SetPrevGame()
    {
        photonView.RPC(nameof(SetPrevGameRPC), RpcTarget.All);
    }

    [PunRPC]
    private void SetPrevGameRPC()
    {
        if (--gameDataIndex < 0)
        {
            gameDataIndex += gameDatas.Length;
        }
        UpdateGameExplain();
    }

    public void SetNextGame()
    {
        photonView.RPC(nameof(SetNextGameRPC), RpcTarget.All);
    }

    [PunRPC]
    private void SetNextGameRPC()
    {
        gameDataIndex = (gameDataIndex + 1) % gameDatas.Length;
        UpdateGameExplain();
    }

    private void UpdateGameExplain()
    {
        var gameData = gameDatas[gameDataIndex];

        // 이후 애니메이션 추가
        gameScreenShot.sprite = gameData.gameScreenShot;
        gameName.text = gameData.gameName;
        gameExplain.text = gameData.gameExplain;
    }
    #endregion

    #region For Game Start, Leave Button
    public void StartGame()
    {
        var gameNumber = gameDataIndex != 0 ? gameDataIndex : UnityEngine.Random.Range(1, gameDatas.Length);
        var gameData = gameDatas[gameNumber];
        PhotonNetwork.LoadLevel(gameData.gameScene.name);
    }

    public void LeaveRoom()
    {
        networkManager.LeaveRoom();
    }
    #endregion
}
