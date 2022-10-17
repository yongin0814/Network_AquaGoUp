using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GoUpChatSystem : MonoBehaviourPun
{
    // �÷��̾��� �Է� �޾ƿ��� 
    public ChattingScroll chattingScroll;
    public GoUpPlayer[] players;

    void Start()
    {
        // �ʱ� ä��â Ȱ��ȭ
    }

    /// <summary>
    /// �����̹� �ö�Ÿ���� �Է��� ���⼭�� 
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // ä�� �����ִ��� Ȯ��

            // ä�� ��������� �״�� ����

            // ���� ���� �� �ִ� �����̰� �̹� ���忡 ó�� ���ϴ°Ÿ� �� ���ϱ�
            if (GoUpGameSystem.Instance.IsAnswerable)
            {
                // ���� ����� ���� ��� �����ϱ�
                GoUpGameSystem.Instance.JudgeAnswer(IsCorrectAnswer());
            }

            PlayerChat(chattingScroll.CurrentInput);
            // ä�� ��� : ä���� ����� ������� ����ؾ��Ѵ�
            chattingScroll.PlayerChat();
        }
    }

    /// <summary>
    /// �÷��̾� ä�� ����
    /// </summary>
    /// <param name="message">������ ����</param>
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
        // ���� ��������
        string memo = GoUpGameSystem.Instance.GetMemo();
        // �Է°� �ް�
        string answer = chattingScroll.chatInputField.text;
        
        // ��
        return answer == memo;
    }
}
