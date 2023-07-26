using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject CanvasGame;
    public GameObject CanvasRestart;

    [Header("CanvasRestart")]
    public GameObject RedWinTxt;
    public GameObject BlueWinTxt;

    [Header("Other")]
    public AudioManager audioManager;

    public ScoreScript scoreScript;

    public PuckScript puckScript;

    private void Start() {
        scoreScript = GetComponent<ScoreScript>();

    }

    public void ShowRestartCanvas(bool didRedWin)
    {
        CanvasGame.SetActive(false);
        CanvasRestart.SetActive(true);
        audioManager.PlayEndGame();

        if (didRedWin)
        {
            RedWinTxt.SetActive(true);
            BlueWinTxt.SetActive(false);
        }
        else
        {
            RedWinTxt.SetActive(false);
            BlueWinTxt.SetActive(true);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1;

        CanvasGame.SetActive(true);
        CanvasRestart.SetActive(false);

        scoreScript.ResetScores();
        puckScript.CenterPuck();
    }
}
