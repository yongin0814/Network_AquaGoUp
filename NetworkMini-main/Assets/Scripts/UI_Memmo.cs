using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 이거 Memo에 들어가는거임

// text 를 담을 그릇을 만들어준다
// inputmanager 에서 memo 가져오기. memo 는 string 이다


public class UI_Memmo : MonoBehaviour
{
    // UI
    public Text memoText;
    private string memoGot;
    public Image mask;

    // 시작문구 친구들
    public string[] startTexts;

    // 문제번호 친구들
    public string[] numberTexts;

    // 처음인지아닌지
    private int roundGot;

    // Start is called before the first frame update
    IEnumerator Start()
    {

        // 시작하고 5초 뒤에
        yield return new WaitForSeconds(5);
        
        // string을 보여준다 
        for (int i = 0; i < 3; i++)
        {            
            memoText.text = startTexts[i];

            // 한번 띄워준 다음에 3초 기다린다
            yield return new WaitForSeconds(2.0f);
        }

        // 시작문구 친구들 다 하고 나면 비로소 시작
        StartCoroutine(NextRound());
    }

    // Update is called once per frame
    void Update()
    {
        // 라운드 가져오기
        roundGot = InputTextt_YJ.instance.round;

        // 라운드에 해당하는 메모 가져오기
        memoGot = InputTextt_YJ.instance.memo;


        //test
        //print("roundGot : " + roundGot);
        //print("memoText : " + memoText.text);

    }

    IEnumerator NextRound()
    {
        // 안끝났을때는
        for (int i = 0; i < 9; i++)
        {
            // 번호 켜주기
            TurnOnlyNumber();
            // 2초 뒤에 바꾸고
            yield return new WaitForSeconds(2);
            ChangeToMemo();

            // 5초 뒤에 꺼주고
            yield return new WaitForSeconds(5);
            TurnMemoOff();

            // 3초뒤에 
            yield return new WaitForSeconds(3);

            // 다시 켜주겠지??
        }
    }

    private void Result()
    {
        // 끝났을 때 결과창 보여주기
        if (roundGot == 9)
        {
            // 결과창 보여주기
        }
    }
    private void TurnOnlyNumber()
    {
        // 마스크 켜주기 
        mask.GetComponent<Image>().enabled = true;

        // 메모 켜주기 
        this.gameObject.GetComponent<Text>().enabled = true;
        memoText.text = numberTexts[roundGot];
    }
    private void ChangeToMemo()
    {

        // 받아온 메모를 text에 해준다
        memoText.text = memoGot;

    }

    private void TurnMemoOff()
    {
        // 마스크랑 memo 꺼주기
        mask.GetComponent<Image>().enabled = false;
        this.gameObject.GetComponent<Text>().enabled = false;

    }
}

