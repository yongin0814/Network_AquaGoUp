using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using TMPro;

public class GoUpResultUI : MonoBehaviour
{
    [System.Serializable]
    public class PlayerResultPanel
    {
        public GameObject panel;
        public TextMeshProUGUI playerNameText;
        public TextMeshProUGUI playerCorrectCountText;
    }

    public GoUpPlayer[] players;
    public PlayerResultPanel[] resultPanels;

    void Start()
    {
        Debug.Assert(players.Length == resultPanels.Length, "Error : player result UIs are incorrect");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].gameObject.activeSelf)
            {
                resultPanels[i].playerNameText.text = players[i].nameText.text;
                resultPanels[i].playerCorrectCountText.text =
                    players[i].correctNumber + " / " + GoUpGameSystem.Instance.MaxRound;
            }
            else
            {
                resultPanels[i].panel.gameObject.SetActive(false);
            }
        }
    }
}
