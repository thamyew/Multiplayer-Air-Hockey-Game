using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class StatManager : MonoBehaviour
{
    private DatabaseManager dbManager;
    public static List<Match> matches = new List<Match>();
    public TMP_Text totalMatch;
    public TMP_Text totalMatchWin;
    public TMP_Text winRate;

    private void Start() {
        dbManager = FindObjectOfType<DatabaseManager>();
        dbManager.RetrieveMatchesForUser(matches);
        StartCoroutine(updateStat());
    }

    private IEnumerator updateStat() {
        yield return new WaitForSeconds(1);

        int totalMatchNum = matches.Count;
        int totalMatchWinNum = 0;

        if (totalMatchNum != 0) {
            for (int count = 0; count < totalMatchNum; count++) {
                Match match = matches[count];
                string winner = "";
                if (match.winIndex == 0) winner = match.player1;
                else winner = match.player2;

                if (winner == References.userID) totalMatchWinNum++;
            }

            float winRateNum = (float)totalMatchWinNum / (float)totalMatchNum * 100.0f;

            totalMatch.text = totalMatchNum.ToString();
            totalMatchWin.text = totalMatchWinNum.ToString();
            winRate.text = winRateNum.ToString("0.00") + " %";
        } else {
            totalMatch.text = "0";
            totalMatchWin.text = "0";
            winRate.text = "N/A";
        }
    }
}
