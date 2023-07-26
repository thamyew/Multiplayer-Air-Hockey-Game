[System.Serializable]
public class Match {
    public string player1;
    public string player2;
    public int winIndex;
    public string time;

    public Match(string player1, string player2, int winIndex, string time) {
        this.player1 = player1;
        this.player2 = player2;
        this.winIndex = winIndex;
        this.time = time;
    }
}
