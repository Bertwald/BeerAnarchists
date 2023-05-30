namespace Forum.Data;
public interface IMessage {
    public IEnumerable<Forum.Data.Models.Message> GetAllMessages(string userId);
    public IEnumerable<Forum.Data.Models.PrivateMessage> GetPrivateMessages(string userId);
    public IEnumerable<Forum.Data.Models.GroupMessage> GetGroupMessages(int groupId);
    public Task SendPrivateMessage(Forum.Data.Models.PrivateMessage message);
    public Task SendGroupMessage(Forum.Data.Models.GroupMessage message);
}
