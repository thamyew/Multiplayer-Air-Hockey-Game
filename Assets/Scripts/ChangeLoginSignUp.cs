using UnityEngine;

public class ChangeLoginSignUp : MonoBehaviour
{
    public GameObject LoginPanel;
    public GameObject SignUpPanel;
    private bool login = true;

    public void changeMenu() {
        login = !login;
        if (login) {
            LoginPanel.SetActive(true);
            SignUpPanel.SetActive(false);
        } else {
            LoginPanel.SetActive(false);
            SignUpPanel.SetActive(true);
        }
    }
}
