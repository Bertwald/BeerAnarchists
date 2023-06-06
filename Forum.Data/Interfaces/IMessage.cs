namespace Forum.Data.Interfaces;
public interface IMessage
{
    public IEnumerable<Models.Message> GetAllMessages(string userId);
    public IEnumerable<Models.PrivateMessage> GetPrivateMessages(string userId);
    public IEnumerable<Models.GroupMessage> GetGroupMessages(int groupId);
    public Task SendPrivateMessage(Models.PrivateMessage message);
    public Task SendGroupMessage(Models.GroupMessage message);
}
