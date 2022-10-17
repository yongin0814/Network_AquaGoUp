using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ChattingScroll �׽�Ʈ��
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

            // ä�� ó��
            chattingScroll.PlayerChat();

            // �ʱ⿣ ä���� ġ�� ���°� �ƴϴ�
            //if (!isChatting)
            //{
            //    // ä�� Ȱ��ȭ
            //    chatInputField.Select();
            //}
            //// InputField�� ä���� ġ�� ���¸� ���� ������ �Է��� ��ģ��
            //else
            //{
            //    PlayerChat("Player : " + chatInputField.text);
            //    chatInputField.Select();
            //}

            //// �ݴ�� ��ȯ
            //isChatting = !isChatting;
            chattingScroll.Activated = false;
        }
    }
}
