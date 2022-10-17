using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GoUpChatSystem : MonoBehaviourPun
{
    // 플레이어의 입력 받아오기 
    public ChattingScroll chattingScroll;
    public GoUpPlayer[] players;

    void Start()
    {
        // 초기 채팅창 활성화
    }

    /// <summary>
    /// 서바이벌 올라타자의 입력은 여기서만 
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // 채팅 켜져있는지 확인

            // 채팅 비어있으면 그대로 끄기

            // 답을 적을 수 있는 상태이고 이번 라운드에 처음 답하는거면 답 비교하기
            if (GoUpGameSystem.Instance.IsAnswerable)
            {
                // 비교한 결과에 따라 결과 적용하기
                GoUpGameSystem.Instance.JudgeAnswer(IsCorrectAnswer());
            }

            PlayerChat(chattingScroll.CurrentInput);
            // 채팅 출력 : 채팅은 정답과 상관없이 출력해야한다
            chattingScroll.PlayerChat();
        }
    }

    /// <summary>
    /// 플레이어 채팅 띄우기
    /// </summary>
    /// <param name="message">적용할 내용</param>
    public void PlayerChat(string message)
    {
        photonView.RPC(nameof(PlayerChatRPC), RpcTarget.All,
            PhotonNetwork.LocalPlayer.ActorNumber - 1,
            message);
    }

    [PunRPC]
    private void PlayerChatRPC(int playerNum, string message)
    {
        players[playerNum].ShowChat(message);
    }

    private bool IsCorrectAnswer()
    {
        // 정답 가져오기
        string memo = GoUpGameSystem.Instance.GetMemo();
        // 입력값 받고
        string answer = chattingScroll.chatInputField.text;
        
        // 비교
        return answer == memo;
    }
}
