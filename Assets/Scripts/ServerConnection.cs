using Photon.Pun;
using System.Collections;

public class ServerConnection : MonoBehaviourPunCallbacks
{
    private SceneManage sceneManager;

    private void Start() {
        sceneManager = FindObjectOfType<SceneManage>();
    }

    public void connectToServer() {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable {
                { "fb_userID", References.userID }
            });
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby() {
        sceneManager.loadScene(2);
    }

    public void disconnect() {
        StartCoroutine(closeConnection());
    }

    private IEnumerator closeConnection() {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;
        sceneManager.loadScene(0);
    }
}
