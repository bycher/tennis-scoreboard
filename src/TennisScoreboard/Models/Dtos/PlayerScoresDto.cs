namespace TennisScoreboard.Models.Dtos;

public class PlayerScoresDto
{
    public int Points { get; set; }
    public int Games { get; set; }
    public List<int> Sets { get; set; } = [];

    public void ResetPoints() => Points = 0;

    public void ResetGames()
    {
        Sets.Add(Games);
        Games = 0;
    }

    public string PointsAsString => Points switch
    {
        0 => "0",
        1 => "15",
        2 => "30",
        3 => "40",
        4 => "AD",
        _ => throw new ArgumentException("Invalid point")
    };

    public static int PointsAsInt(string pointsString) => pointsString switch
    {
        "0" => 0,
        "15" => 1,
        "30" => 2,
        "40" => 3,
        "AD" => 4,
        _ => throw new ArgumentException("Invalid points string")
    };
}