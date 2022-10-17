using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using Photon.Pun;
using Photon.Realtime;


// �̰� �÷��̾ �ٿ��߰ڴ�


public class InputTextt_YJ : MonoBehaviourPun
{
    //test Time
    private float time;

    // �ڽĿ�����Ʈ�� �ؽ�Ʈ 
    private Text childTextForNull;


    // �÷��̾��� �Է� �޾ƿ��� 
    public ChattingScroll chattingScroll;

    public string answer = null;

    // ����_���� �޾ƿ���
    public FileStream answerFile;
    public string memoPath;
    public string[] contents;

    // ����_�� �޾ƿ���
    public string memo;

    // ����
    public int round = 0;

    // �ð�
    private int firsttime = 18;
    private int roundtime = 10;

    // ����üũ
    public int correct;

    // ���üũ�� ���䰹��
    public int correctNumber = 0;

    // �̵�
    public float speed;
    // �̱��� �����
    public static InputTextt_YJ instance;
    private void Awake()
    {
        InputTextt_YJ.instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ReadMemo();

        // �ʱ� ä��â Ȱ��ȭ

    }

    public void StartGame()
    {
        photonView.RPC(nameof(StartGameRPC), RpcTarget.All);
    }

    [PunRPC]
    public void StartGameRPC()
    {
        StartCoroutine(IEStartGame());
    }

    IEnumerator IEStartGame()
    {
        // 14�� �ִٰ� ����
        yield return new WaitForSeconds(firsttime);

        // 10�ʸ��� round �÷��ֱ� -> 5�ʴ� UI, 3�ʴ� 8����, 2�ʴ� �����̸�
        for (int i = 0; i < 9; i++)
        {
            round++;
            yield return new WaitForSeconds(roundtime);


        }
    }


    // Update is called once per frame
    void Update()
    {
        //time += Time.deltaTime;
        //print(time);
        

        // �亯�� �Է��� ���°� && ����Ű�� �����ٸ�
        if ((chattingScroll.chatInputField.text.Length > 0) && (Input.GetKeyDown(KeyCode.Return)))
        {
            // �����ֱ�
            CompareAnswer();

            // ���� ����� ���� ��� �����ϱ�


            // ä�� ���
            Chat(chattingScroll.chatInputField.text);


            // ��� ����
            IfAnswerGoUp();
            IfWrongGoDown();



        }

    }


    public void Chat(string message)
    {
        photonView.RPC(nameof(Chat_RPC), RpcTarget.All,
            PhotonNetwork.LocalPlayer.NickName + " : " + message);
    }

    [PunRPC]
    private void Chat_RPC(string message)
    {
        chattingScroll.PlayerChat();
    }

    // ���� �޾Ƽ� �о����
    public void ReadMemo()
    {
        // ���� �޾ƿ���
        memoPath = Application.dataPath;
        memoPath += "/Text/QuestionStorage.txt";

        // contents���ٰ� ��ü ���� �����ͼ� contents��� ������ �����ϱ�
        contents = System.IO.File.ReadAllLines(memoPath);
    }

    // ������ �ȿ� �ִ� �޸� �����ͼ� �б�
    public string GetMeMo()
    {
        Debug.Assert(contents.Length > 0, "Error : contents is empty");
        return memo = contents[round];
    }


    public string GetAnswer()
    {
        // memo�� �Է¹����Ŷ� ���ϱ�
        // �Է��ѰŴ� answer 
        // ������ memo
        //answer = answerInput.GetComponent<InputField>().text;
        return answer = chattingScroll.chatInputField.text;


    }

    public void CompareAnswer()
    {

        // �䰡���ͼ�
        memo = GetMeMo();
        // �Է°� �޾Ƽ� 
        answer = GetAnswer();

        // �����̸� 1, �����̸� -1
        correct = answer == memo ? 1 : -1; 
    }

    public void IfAnswerGoUp()
    {
        // �����̸�
        if ( correct == 1 )
        {
            // �÷��ֱ� 
            // this.gameObject.transform.position += Vector3.up;
            this.gameObject.transform.position += Vector3.up * speed;


            correctNumber += 1;



        }

        correct = 0;

    }
    public void IfWrongGoDown()
    {
        // �����̸�
        if (correct == -1)
        {
            // �����ֱ�
            this.gameObject.transform.position += Vector3.down * speed;


        }

        correct = 0;


    }
}
