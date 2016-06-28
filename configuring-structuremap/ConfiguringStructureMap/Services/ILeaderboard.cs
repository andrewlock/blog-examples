namespace ConfiguringStructureMap
{
    public interface ILeaderboard<T>
    {
        int GetPosition(object user);
    }
}
