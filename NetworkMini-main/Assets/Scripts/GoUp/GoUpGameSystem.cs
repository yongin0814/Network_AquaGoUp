using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

/// <summary>
/// ���� ������ �а� UI�� ������ �� ��� �� �����ϴ� Ŭ����
/// </summary>
public class GoUpGameSystem : MonoBehaviourPun
{
    public static GoUpGameSystem Instance { get; private set; }

    [Header("Title")]
    public Image titleImage;
    [SerializeField] private float titleAnimTime = 5.0f;

    [Header("Memo UI")]
    public TextMeshProUGUI memoText;
    public Image mask;

    [Header("Guide")]
    public string[] guides;
    public float guideWaitTime = 3.0f;

    [Header("Memo")]
    // ����_���� �޾ƿ���
    private readonly string MEMO_PATH = "/Text/QuestionStorage.txt";
    public string[] memoLines;
    public float roundNumShowTime = 2.0f;
    public float memoShowTime = 5.0f;
    public float roundWaitTime = 3.0f;

    [Header("Round")]
    public int MaxRound = 9;
    public int Round { get; private set; } = 0;

    public GoUpResultUI resultUI;

    // ������ȣ ģ����
    private readonly string NUMBER_TEXT = "����";

    // �亯 ���� ����
    // �亯 ������ �������� Ȯ�� - ó�� ���ѰŸ� �亯 ó��
    public bool IsAnswerable { get; private set; } = false;
    // ���� ����
    private int currentRank = 0;

    // �� ĳ���� �� ��� ĳ���͵� ����
    public GoUpPlayer[] players;
    //public HashSet<GameObject> players;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // ���� ���� �� ����� �� ���ư��� �ۿ��� �̸� �����ϴ� ������ ����
        //ReadAnswerFile();

        StartGame();
    }

    /// <summary>
    /// ���Ϸκ��� ���� ���� �޾ƿ���
    /// </summary>
    private void ReadAnswerFile()
    {
        // ���� ��� ����
        string answerPath = Application.dataPath + MEMO_PATH;
        // ���� ������ �о ���� �ٿ� �ֱ�
        memoLines = System.IO.File.ReadAllLines(answerPath);
    }
    
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(StartGameRPC), RpcTarget.All);
        }
    }

    [PunRPC]
    private void StartGameRPC()
    {
        print("Start Game");
        StartCoroutine(IEStartGuide());
    }

    IEnumerator IEStartGuide()
    {
        // Title Animation �����ְ�
        yield return IETitleAnimation();

        // ���� ������ �����ش�
        for (int i = 0; i < guides.Length; i++)
        {
            memoText.text = guides[i];
            // �ѹ� ����� ������ 3�� ��ٸ���
            yield return new WaitForSeconds(guideWaitTime);
        }
        
        // ���۹��� ģ���� �� �ϰ� ���� ��μ� ����
        StartCoroutine(IEStartRounds());
    }

    IEnumerator IETitleAnimation()
    {
        titleImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(titleAnimTime);
        titleImage.gameObject.SetActive(false);
    }

    // �ٿ� ����� �Ѿ��
    IEnumerator IEStartRounds()
    {
        // �ȳ���������
        for (Round = 0; Round < MaxRound; Round++)
        {
            // ��ȣ ���ֱ�
            ShowNumber();
            // 2�� �ڿ� �ٲٰ�
            yield return new WaitForSeconds(roundNumShowTime);

            ShowMemo();
            // 5�� �ڿ� ���ְ�
            yield return new WaitForSeconds(memoShowTime);

            TurnOffMemo();
            // 3�ʵڿ� 
            yield return new WaitForSeconds(roundWaitTime);

            // ���� ���� ����
            foreach (var goUpPlayer in players)
            {
                if (goUpPlayer.gameObject.activeSelf)
                {
                    goUpPlayer.HideRank();
                }
            }
        }

        // ��� â �����ֱ�
        ShowResult();
    }

    private void ShowNumber()
    {
        // ����ũ ���ֱ� 
        mask.enabled = true;

        // �޸� ���ֱ� 
        memoText.enabled = true;
        memoText.text = NUMBER_TEXT + (Round + 1);
    }

    /// <summary>
    /// �޸� �����ֱ�
    /// �� ������ ������ ���� �� �ְ�, ģ ä���� �������� �����ȴ�
    /// </summary>
    private void ShowMemo()
    {
        currentRank = 0;
        IsAnswerable = true;

        // �޾ƿ� �޸� text�� ���ش�
        memoText.text = memoLines[Round];
    }

    private void TurnOffMemo()
    {
        // �޸� �������� �� �̻� ������ ���� �� ����
        // ���� ������ ���� ���� ���¶�� Ʋ�ȴٰ� �����ؾ� �Ѵ�
        if (IsAnswerable)
        {
            JudgeAnswer(false);
        }

        // ����ũ�� memo ���ֱ�
        mask.enabled = false;
        memoText.enabled = false;
    }

    private void ShowResult()
    {
        print("End");

        // ������ �� ���â �����ֱ�
        resultUI.gameObject.SetActive(true);


        Invoke(nameof(ReturnToRoom), 5.0f);
    }

    private void ReturnToRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Lobby");
        }
    }

    public string GetMemo()
    {
        Debug.Assert(memoLines.Length > 0, "Error : answerLines is empty");
        return memoLines[Round];
    }

    // ���� ����
    public void JudgeAnswer(bool correct)
    {
        // �����̸� �� ĳ���� �ø���
        photonView.RPC(nameof(JudgeAnswerRPC), RpcTarget.AllBuffered, 
            correct, PhotonNetwork.LocalPlayer.ActorNumber);

        // ������ �޾����� �´��� Ʋ������ ������� �� �̻� ������ ���� ���ϰ� ����
        IsAnswerable = false;
    }

    // RPC - ���� ����
    [PunRPC]
    public void JudgeAnswerRPC(bool correct, int actorNumber)
    {
        if (correct)
        {
            print($"Player{actorNumber} has correct answer - rank {currentRank + 1}");
            players[actorNumber - 1].GoUp(currentRank++);
        }
        else
        {
            print($"Player{actorNumber} has incorrect answer");
            players[actorNumber - 1].GoDown();
        }
    }

    #region Debug

    public void DebugStartGame()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount > 2 && PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            print($"Player Count - {PhotonNetwork.CurrentRoom.PlayerCount}");
            StartGame();
        }
    }
    #endregion
}
