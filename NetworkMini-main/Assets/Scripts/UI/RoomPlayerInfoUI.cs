using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 방에 있는 플레이어 정보를 보여주는 UI
/// </summary>
public class RoomPlayerInfoUI : MonoBehaviour
{
    public TextMeshProUGUI playerNameText;
    public bool isUsed = false;

    public void SetPlayerInfo(Photon.Realtime.Player player)
    {
        isUsed = true;
        playerNameText.gameObject.SetActive(true);
        playerNameText.text = player.NickName;
    }

    public void DisableUI()
    {
        isUsed = false;
        playerNameText.gameObject.SetActive(false);
    }
}
