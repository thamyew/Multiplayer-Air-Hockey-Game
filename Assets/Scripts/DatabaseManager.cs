using System.Collections;
using Firebase.Database;
using UnityEngine;
using System;
using System.Collections.Generic;

public class DatabaseManager : MonoBehaviour
{
    private DatabaseReference dbReference;
    private StatManager statManager;
    private FirebaseAuthManager firebaseAuth;
    private SceneManage sceneManager;
    private string _username;

    void Start() {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        firebaseAuth = FindObjectOfType<FirebaseAuthManager>();
        sceneManager = FindObjectOfType<SceneManage>();
        statManager = FindObjectOfType<StatManager>();
    }

    public void CreateUser(string userID, string username) {
        User newUser = new User(username);
        string json = JsonUtility.ToJson(newUser);

        dbReference.Child("users").Child(userID).SetRawJsonValueAsync(json);
    }

    public IEnumerator GetName(Action<string> onCallback) {
        var userNameData = dbReference.Child("users").Child(References.userID).Child("username").GetValueAsync();
        yield return new WaitUntil(predicate: () => userNameData.IsCompleted);

        if (userNameData != null) {
            DataSnapshot snapshot = userNameData.Result;
            onCallback.Invoke(snapshot.Value.ToString());
        }

        sceneManager.loadScene(1);
    }

    public void CreateMatch(string userID1, string userID2, int winIndex) {
        string currentTime = DateTime.Now.ToString();
        MatchReference.host = userID1;
        MatchReference.time = currentTime;
        Match newMatch = new Match(userID1, userID2, winIndex, currentTime);
        string json = JsonUtility.ToJson(newMatch);

        MatchReference.matchID = dbReference.Child("matches").Push().Key;

        dbReference.Child("matches").Child(MatchReference.matchID).SetRawJsonValueAsync(json);
    }

    public void UpdateMatch(string userID2) {
        dbReference.Child("matches").Child(MatchReference.matchID).Child("player2").SetValueAsync(userID2);
    }

    public void UpdateMatch(int winIndex) {
        dbReference.Child("matches").Child(MatchReference.matchID).Child("winIndex").SetValueAsync(winIndex);
    }

    public void RetrieveMatchesForUser(List<Match> tempMatches) {
        tempMatches.Clear();
        dbReference.Child("matches").GetValueAsync().ContinueWith(task => {
            if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot matchSnapshot in snapshot.Children) {
                    string player1 = matchSnapshot.Child("player1").GetValue(true).ToString();
                    string player2 = matchSnapshot.Child("player2").GetValue(true).ToString();

                    if (player1 == References.userID || player2 == References.userID) {
                        Match tempMatch = new Match(player1, player2, int.Parse(matchSnapshot.Child("winIndex").GetValue(true).ToString()), matchSnapshot.Child("time").GetValue(true).ToString());
                        tempMatches.Add(tempMatch);
                    }
                }
            }
        });
    }

    public void RetrieveAllUser(Dictionary<string, string> users) {
        users.Clear();
        dbReference.Child("users").GetValueAsync().ContinueWith(task => {
            if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot userSnapshot in snapshot.Children) {
                    users.Add(userSnapshot.Key, userSnapshot.Child("username").Value.ToString());
                }
            }
        });
    }

    public string username {
        get { return _username; }
        set { _username = value; }
    }
}