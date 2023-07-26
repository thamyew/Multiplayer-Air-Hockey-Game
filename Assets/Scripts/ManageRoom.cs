using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

public class ManageRoom : MonoBehaviourPunCallbacks 
{
    private DatabaseManager dbManager;
    private SceneManage sceneManager;
    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    void Start() {
        dbManager = FindObjectOfType<DatabaseManager>();
        sceneManager = FindObjectOfType<SceneManage>();
        PhotonNetwork.EnableCloseConnection = true;
    }

    public void CreateRoom() {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(createInput.text, roomOptions);
        dbManager.CreateMatch(References.userID, "-", -1);
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer) {
        if (!newPlayer.Equals(PhotonNetwork.LocalPlayer)) {
            if (newPlayer.CustomProperties.TryGetValue("fb_userID", out object customIdentifier)) {
                string identifier = (string)customIdentifier;
                dbManager.UpdateMatch(identifier);
            }
        }
    }

    public void JoinRoom() {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom() {
        PhotonNetwork.LoadLevel("GameScene");
    }

    public override void OnLeftRoom() {
        PhotonNetwork.JoinLobby();
    }

    public void closeGame() {
        if (PhotonNetwork.IsMasterClient) {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            Photon.Realtime.Player[] player = PhotonNetwork.PlayerListOthers;
            foreach (Photon.Realtime.Player other in player) {
                PhotonNetwork.CloseConnection(other);
            }
        }
        PhotonNetwork.LeaveRoom();
    }
}
