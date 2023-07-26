using UnityEngine;
using TMPro;

public class GenerateInformation : MonoBehaviour {
    public TMP_Text player1Name;
    public TMP_Text player1ID;
    public TMP_Text player2Name;
    public TMP_Text player2ID;
    public TMP_Text matchTime;
    public GameObject winner1;
    public GameObject winner2;

    public void updateInformation(Match match, string player1Username, string player2Username) {
        player1Name.text = player1Username;
        player1ID.text = match.player1;
        player2Name.text = player2Username;
        player2ID.text = match.player2;
        matchTime.text = match.time;

        if (match.winIndex == 0) winner1.SetActive(true);
        else winner2.SetActive(true);
    }
}
