using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ChattingScroll 테스트용
/// </summary>
public class DebugChat : MonoBehaviour
{
    public ChattingScroll chattingScroll;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (!chattingScroll.Activated)
            {
                print("chatting not Activated");
                chattingScroll.Activated = true;
                return;
            }

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
            chattingScroll.Activated = false;
        }
    }
}
