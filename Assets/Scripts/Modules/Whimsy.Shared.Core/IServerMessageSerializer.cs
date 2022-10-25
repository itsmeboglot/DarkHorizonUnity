namespace Whimsy.Shared.Core
{
    public interface IServerMessageSerializer
    {
        byte[] Serialize(object message);
        T Deserialize<T>(byte[] data);
    }
}