using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryGenerator : MonoBehaviour {
    Dictionary<string, string> users = new Dictionary<string, string>();
    public GameObject listPrefab;
    public Transform parent;
    private DatabaseManager dbManager;

    private void Start() {
        dbManager = FindObjectOfType<DatabaseManager>();
        dbManager.RetrieveAllUser(users);

        StartCoroutine(createInformation());
    }

    private IEnumerator createInformation() {
        yield return new WaitForSecondsRealtime(1.0f);
        foreach (Match match in StatManager.matches) {
            GameObject listItem = Instantiate(listPrefab, Vector3.zero, Quaternion.identity);
            listItem.transform.SetParent(parent, false);

            listItem.GetComponent<GenerateInformation>().updateInformation(match, users[match.player1], users[match.player2]);
        }
    }
}
