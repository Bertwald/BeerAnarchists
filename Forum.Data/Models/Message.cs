namespace Forum.Data.Models;

public abstract class Message {
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public DateTime Created { get; set; }
    public required virtual ForumUser Sender { get; set; }
}

public class PrivateMessage : Message {
    public required virtual ForumUser Reciever { get; set; }
}
public class GroupMessage : Message {
    public virtual Group? RecievingGroup { get; set; }
    public required virtual IEnumerable<ForumUser> Recievers { get; set; }
}
