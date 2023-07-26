using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerRed;
    public GameObject playerBlue;
    public GameObject puck;
    private GoalScript[] goals;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Vector2 redPosition = new Vector2(0, -4);
            PhotonNetwork.Instantiate(playerRed.name, redPosition, Quaternion.identity);
        }
        else
        {
            Vector2 bluePosition = new Vector2(0, 4);
            PhotonNetwork.Instantiate(playerBlue.name, bluePosition, Quaternion.identity);
        }
        
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2) {
            PhotonNetwork.Instantiate(puck.name, Vector2.zero, Quaternion.identity);
        }
    }
}
