using System.Collections;
using UnityEngine;
using Photon.Pun;

public class GoalScript : MonoBehaviourPunCallbacks {
    private ScoreScript ScoreScriptInstance;
    private AudioManager audioManager;
    
    private void Start() {
        ScoreScriptInstance = FindObjectOfType<ScoreScript>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.CompareTag("puck")) {
            if (PhotonNetwork.LocalPlayer == collider.gameObject.GetComponent<PhotonView>().Owner) {
                if (this.gameObject.tag == "BlueGoal") {
                    ScoreScriptInstance.Increment(ScoreScript.Score.RedScore);
                }
                else if (this.gameObject.tag == "RedGoal") {
                    ScoreScriptInstance.Increment(ScoreScript.Score.BlueScore);
                }
                audioManager.PlayGoal();
                collider.gameObject.GetComponent<PuckScript>().recenterPuck();
                StartCoroutine(activation());
            }
        }
    }

    private IEnumerator activation() {
        this.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSecondsRealtime(0.5f);
        this.GetComponent<Collider2D>().enabled = true;
    }
}
