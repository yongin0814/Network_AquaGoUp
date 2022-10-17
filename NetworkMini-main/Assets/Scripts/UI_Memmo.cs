using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �̰� Memo�� ���°���

// text �� ���� �׸��� ������ش�
// inputmanager ���� memo ��������. memo �� string �̴�


public class UI_Memmo : MonoBehaviour
{
    // UI
    public Text memoText;
    private string memoGot;
    public Image mask;

    // ���۹��� ģ����
    public string[] startTexts;

    // ������ȣ ģ����
    public string[] numberTexts;

    // ó�������ƴ���
    private int roundGot;

    // Start is called before the first frame update
    IEnumerator Start()
    {

        // �����ϰ� 5�� �ڿ�
        yield return new WaitForSeconds(5);
        
        // string�� �����ش� 
        for (int i = 0; i < 3; i++)
        {            
            memoText.text = startTexts[i];

            // �ѹ� ����� ������ 3�� ��ٸ���
            yield return new WaitForSeconds(2.0f);
        }

        // ���۹��� ģ���� �� �ϰ� ���� ��μ� ����
        StartCoroutine(NextRound());
    }

    // Update is called once per frame
    void Update()
    {
        // ���� ��������
        roundGot = InputTextt_YJ.instance.round;

        // ���忡 �ش��ϴ� �޸� ��������
        memoGot = InputTextt_YJ.instance.memo;


        //test
        //print("roundGot : " + roundGot);
        //print("memoText : " + memoText.text);

    }

    IEnumerator NextRound()
    {
        // �ȳ���������
        for (int i = 0; i < 9; i++)
        {
            // ��ȣ ���ֱ�
            TurnOnlyNumber();
            // 2�� �ڿ� �ٲٰ�
            yield return new WaitForSeconds(2);
            ChangeToMemo();

            // 5�� �ڿ� ���ְ�
            yield return new WaitForSeconds(5);
            TurnMemoOff();

            // 3�ʵڿ� 
            yield return new WaitForSeconds(3);

            // �ٽ� ���ְ���??
        }
    }

    private void Result()
    {
        // ������ �� ���â �����ֱ�
        if (roundGot == 9)
        {
            // ���â �����ֱ�
        }
    }
    private void TurnOnlyNumber()
    {
        // ����ũ ���ֱ� 
        mask.GetComponent<Image>().enabled = true;

        // �޸� ���ֱ� 
        this.gameObject.GetComponent<Text>().enabled = true;
        memoText.text = numberTexts[roundGot];
    }
    private void ChangeToMemo()
    {

        // �޾ƿ� �޸� text�� ���ش�
        memoText.text = memoGot;

    }

    private void TurnMemoOff()
    {
        // ����ũ�� memo ���ֱ�
        mask.GetComponent<Image>().enabled = false;
        this.gameObject.GetComponent<Text>().enabled = false;

    }
}

