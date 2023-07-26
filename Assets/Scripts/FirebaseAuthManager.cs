using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;

public class FirebaseAuthManager : MonoBehaviour {
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public static FirebaseAuth auth;
    public static FirebaseUser user;

    [Space]
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;

    [Space]
    [Header("Registration")]
    public TMP_InputField nameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField confirmPasswordRegisterField;

    private ChangeLoginSignUp changeLogin;
    private SceneManage sceneManager;
    private DatabaseManager dbManager;

    private void Start() {
        if (auth == null) {
            StartCoroutine(CheckAndFixDependenciesAsync());
        }

        changeLogin = FindObjectOfType<ChangeLoginSignUp>();
        sceneManager = FindObjectOfType<SceneManage>();
        dbManager = FindObjectOfType<DatabaseManager>();
    }
    
    private IEnumerator CheckAndFixDependenciesAsync() {
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => dependencyTask.IsCompleted);

        dependencyStatus = dependencyTask.Result;

        if (dependencyStatus == DependencyStatus.Available) {
            InitializeFirebase();
        } else {
            Debug.LogError("Could not resolve all firebase dependencies: " + dependencyStatus);
        }
    }

    void InitializeFirebase() {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void Login() {
        StartCoroutine(LoginAsync(emailLoginField.text, passwordLoginField.text));
    }

    private IEnumerator LoginAsync(string email, string password) {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null) {
            Debug.LogError(loginTask.Exception);

            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;

            string failedMessage = "Login Failed! Because ";
            switch(authError) {
                case AuthError.InvalidEmail:
                    failedMessage += "Email is invalid";
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "Wrong password";
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "Email is missing";
                    break; 
                case AuthError.MissingPassword:
                    failedMessage += "Password is missing";
                    break;
                default:
                    failedMessage += "Login Failed";
                    break;
            }
            Debug.Log(failedMessage);
        } else {
            user = loginTask.Result.User;
            Debug.LogFormat("{0} You are successfully logged in", user.DisplayName);

            References.userID = user.UserId;
            StartCoroutine(dbManager.GetName((string name) => {
                References.username = name;
            }));
        }
    }

    public void Logout() {
        if (auth != null && user != null) {
            auth.SignOut();
        }
    }

    public void Register() {
        StartCoroutine(RegisterAsync(nameRegisterField.text, emailRegisterField.text, passwordRegisterField.text, confirmPasswordRegisterField.text));
    }

    private IEnumerator RegisterAsync(string name, string email, string password, string confirmedPassword) {
        if (name == "") {
            Debug.LogError("Username is empty");
        } else if (email == "") {
            Debug.LogError("Email is empty");
        } else if (password != confirmedPassword) {
            Debug.LogError("Password does not match");
        } else {
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(() => registerTask.IsCompleted);

            if (registerTask.Exception != null) {
                Debug.LogError(registerTask.Exception);

                FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;

                string failedMessage = "Registration Failed! Because ";
                switch(authError) {
                    case AuthError.InvalidEmail:
                        failedMessage += "Email is invalid";
                        break;
                    case AuthError.WrongPassword:
                        failedMessage += "Wrong Password";
                        break;
                    case AuthError.MissingEmail:
                        failedMessage += "Email is missing";
                        break;
                    case AuthError.MissingPassword:
                        failedMessage += "Password is missing";
                        break;
                    default:
                        failedMessage += "Registration Failed";
                        break;
                }

                Debug.Log(failedMessage);
            } else {
                user = auth.CurrentUser;
                Debug.Log("Registration Successful! Welcome " + name);

                dbManager.CreateUser(user.UserId, name);
                auth.SignOut();

                if (changeLogin) {
                    changeLogin.changeMenu();
                }
            }
        }
    }
}
