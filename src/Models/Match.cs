namespace TennisScoreboard.Models;

public class Match {
    public int ID { get; set; }
    public int FirstPlayerID { get; set; }
    public int SecondPlayerID { get; set; }
    public int WinnerID { get; set; }
}