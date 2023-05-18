namespace Forum.Data.Models;
/// <summary>
/// Used to store the user preferences WRT the font, sizing, color etc
/// </summary>
public class UserPreferences {
    public int Id { get; set; }
    public bool TrigglyPuff { get; set; }
    public required virtual ForumUser User { get; set; }
}
