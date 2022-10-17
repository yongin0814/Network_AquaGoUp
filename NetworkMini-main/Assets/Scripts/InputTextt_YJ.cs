using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using Photon.Pun;
using Photon.Realtime;


// 이건 플레이어에 붙여야겠다


public class InputTextt_YJ : MonoBehaviourPun
{
    //test Time
    private float time;

    // 자식오브젝트의 텍스트 
    private Text childTextForNull;


    // 플레이어의 입력 받아오기 
    public ChattingScroll chattingScroll;

    public string answer = null;

    // 답지_파일 받아오기
    public FileStream answerFile;
    public string memoPath;
    public string[] contents;

    // 답지_줄 받아오기
    public string memo;

    // 라운드
    public int round = 0;

    // 시간
    private int firsttime = 18;
    private int roundtime = 10;

    // 정답체크
    public int correct;

    // 등수체크용 정답갯수
    public int correctNumber = 0;

    // 이동
    public float speed;
    // 싱글톤 만들기
    public static InputTextt_YJ instance;
    private void Awake()
    {
        InputTextt_YJ.instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ReadMemo();

        // 초기 채팅창 활성화

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
        // 14초 있다가 시작
        yield return new WaitForSeconds(firsttime);

        // 10초마다 round 올려주기 -> 5초는 UI, 3초는 8쉬기, 2초는 문제이름
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
        

        // 답변을 입력한 상태고 && 엔터키를 누른다면
        if ((chattingScroll.chatInputField.text.Length > 0) && (Input.GetKeyDown(KeyCode.Return)))
        {
            // 비교해주기
            CompareAnswer();

            // 비교한 결과에 따라 결과 적용하기


            // 채팅 출력
            Chat(chattingScroll.chatInputField.text);


            // 결과 뭔디
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

    // 파일 받아서 읽어오기
    public void ReadMemo()
    {
        // 파일 받아오기
        memoPath = Application.dataPath;
        memoPath += "/Text/QuestionStorage.txt";

        // contents에다가 전체 파일 가져와서 contents라는 변수에 저장하기
        contents = System.IO.File.ReadAllLines(memoPath);
    }

    // 컨텐츠 안에 있는 메모를 가져와서 읽기
    public string GetMeMo()
    {
        Debug.Assert(contents.Length > 0, "Error : contents is empty");
        return memo = contents[round];
    }


    public string GetAnswer()
    {
        // memo랑 입력받은거랑 비교하기
        // 입력한거는 answer 
        // 답지는 memo
        //answer = answerInput.GetComponent<InputField>().text;
        return answer = chattingScroll.chatInputField.text;


    }

    public void CompareAnswer()
    {

        // 답가져와서
        memo = GetMeMo();
        // 입력값 받아서 
        answer = GetAnswer();

        // 정답이면 1, 오답이면 -1
        correct = answer == memo ? 1 : -1; 
    }

    public void IfAnswerGoUp()
    {
        // 정답이면
        if ( correct == 1 )
        {
            // 올려주기 
            // this.gameObject.transform.position += Vector3.up;
            this.gameObject.transform.position += Vector3.up * speed;


            correctNumber += 1;



        }

        correct = 0;

    }
    public void IfWrongGoDown()
    {
        // 오답이면
        if (correct == -1)
        {
            // 내려주기
            this.gameObject.transform.position += Vector3.down * speed;


        }

        correct = 0;


    }
}
