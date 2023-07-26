using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;

public class ScoreScript : MonoBehaviourPunCallbacks
{
    public bool finish = false;
    public enum Score
    {
        BlueScore, RedScore
    }

    public TextMeshProUGUI BlueScoreTxt, RedScoreTxt;

    public UIManager uIManager;      // Enable back when in your unity

    public int MaxScore;

    #region Scores
    private int blueScore, redScore;
    private DatabaseManager dbManager;

    private int BlueScore
    {
        get { return blueScore; }
        set
        {
            blueScore = value;
            if (value == MaxScore) {
                determineWinner();
                uIManager.ShowRestartCanvas(true);  // Enable back when in your unity
                finish = true;
            }
        }
    }
    private int RedScore
    {
        get { return redScore; }
        set
        {
            redScore = value;
            if (value == MaxScore) {
                determineWinner();
                uIManager.ShowRestartCanvas(true);   // Enable back when in your unity
                finish = true;
            }
        }
    }

    #endregion

    private void Start() {
        dbManager = FindObjectOfType<DatabaseManager>();
        uIManager = GetComponent<UIManager>();
        if (PhotonNetwork.IsMasterClient) {
            ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable();
            roomProperties.Add("Player1Score", RedScore);
            roomProperties.Add("Player2Score", BlueScore);
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
        }
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {
        if (propertiesThatChanged.ContainsKey("Player1Score") || propertiesThatChanged.ContainsKey("Player2Score")) {
            updateScore();
        }
    }

    private void updateScore() {
        RedScore = (int)PhotonNetwork.CurrentRoom.CustomProperties["Player1Score"];
        BlueScore = (int)PhotonNetwork.CurrentRoom.CustomProperties["Player2Score"];
        RedScoreTxt.text = PhotonNetwork.CurrentRoom.CustomProperties["Player1Score"].ToString();
        BlueScoreTxt.text = PhotonNetwork.CurrentRoom.CustomProperties["Player2Score"].ToString();
    }

    private void determineWinner() {
        if (PhotonNetwork.IsMasterClient) {
            if (RedScore > BlueScore) dbManager.UpdateMatch(0);
            else dbManager.UpdateMatch(1);
        }
    }

    public void Increment(Score whichScore) {
        if (whichScore == Score.BlueScore)
            BlueScore++;
        else
            RedScore++;

        ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable();
        roomProperties["Player1Score"] = RedScore;
        roomProperties["Player2Score"] = BlueScore;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
    }

    public void ResetScores()
    {
        BlueScore = RedScore = 0;
        BlueScoreTxt.text = RedScoreTxt.text = "0";
    }
}
