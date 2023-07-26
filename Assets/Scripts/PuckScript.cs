using System.Collections;
using Photon.Pun;
using UnityEngine;

public class PuckScript : MonoBehaviourPunCallbacks
{
    private ScoreScript ScoreScriptInstance;
    private Rigidbody2D rb;
    public float MaxSpeed;
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ScoreScriptInstance = FindObjectOfType<ScoreScript>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        audioManager.PlayPuckCollision();
        if (collision.gameObject.GetComponent<PhotonView>()) {
            this.GetComponent<PhotonView>().TransferOwnership(collision.gameObject.GetComponent<PhotonView>().Owner);
        }
    }

    public void recenterPuck() {
        StartCoroutine(ResetPuck());
    }

    private IEnumerator ResetPuck() {
        yield return new WaitForSecondsRealtime(0.5f);
        this.GetComponent<Collider2D>().enabled = false;
        rb.velocity = rb.position = new Vector2(0, 0);
        yield return new WaitForSecondsRealtime(0.5f);
        this.GetComponent<Collider2D>().enabled = true;
    }

    public void CenterPuck()
    {
        rb.position = new Vector2(0, 0);
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, MaxSpeed);
    }

}
