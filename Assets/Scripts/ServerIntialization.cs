using UnityEngine;

public class ServerIntialization : MonoBehaviour
{
    private ServerConnection server;
    void Start() {
        server = FindObjectOfType<ServerConnection>();
        server.connectToServer();
    }
}
