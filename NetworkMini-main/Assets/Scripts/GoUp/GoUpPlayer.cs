using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// �����̹� �ö�Ÿ�� �÷��̾� ������ ���� Ŭ����
/// �÷��̾� ���� ���� ���� �� ������ ����
/// </summary>
public class GoUpPlayer : MonoBehaviour
{
    [Tooltip("���� ����")]
    public int correctNumber = 0;

    // ���� �� �ö󰡴� ����
    public float[] rankBonuses = new float[4];

    public TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI rankText;

    [Header("PlayerChat")]
    [SerializeField] private GameObject chatTextArea;
    [SerializeField] private TextMeshProUGUI chatText;
    [SerializeField] private float chatShowTime = 5.0f;
    private Coroutine chatCoroutine = null;

    /// <summary>
    /// �����ŭ �ö󰡱�
    /// </summary>
    public void GoUp(int rank)
    {
        StartCoroutine(IEUpTo(rankBonuses[rank], 2.0f));
        ShowRank((rank + 1).ToString());
        correctNumber += 1;
    }

    /// <summary>
    /// Ʋ����(Ȥ�� ���� �� ����) ��������
    /// </summary>
    public void GoDown()
    {
        StartCoroutine(IEUpTo(-1, 2.0f));
        ShowRank("X");
    }

    IEnumerator IEUpTo(float upAmount, float time)
    {
        var to = transform.position + Vector3.up * upAmount;
        for (float t = 0.0f; t < time; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(transform.position, to, Time.deltaTime * 5);
            yield return null;
        }
        transform.position = to;
    }

    public void ShowRank(string rankStr)
    {
        rankText.gameObject.SetActive(true);
        rankText.text = rankStr;
    }

    public void HideRank()
    {
        rankText.text = "";
        rankText.gameObject.SetActive(false);
    }

    public void ShowChat(string chatMessage)
    {
        if (chatCoroutine != null)
        {
            StopCoroutine(chatCoroutine);
        }
        chatCoroutine = StartCoroutine(IEShowChat(chatMessage));
    }

    IEnumerator IEShowChat(string chatMessage)
    {
        chatTextArea.SetActive(true);
        chatText.text = chatMessage;
        yield return new WaitForSeconds(chatShowTime);
        chatTextArea.SetActive(false);
        chatCoroutine = null;
    }
}
