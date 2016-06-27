namespace ConfiguringBuiltInContainer
{
    public interface ILeaderboard<T>
    {
        int GetPosition(object user);
    }
}
