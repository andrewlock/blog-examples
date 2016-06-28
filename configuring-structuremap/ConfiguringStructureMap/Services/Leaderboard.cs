namespace ConfiguringStructureMap
{
    public class Leaderboard<T> : ILeaderboard<T>
    {
        public int GetPosition(object user)
        {
            return 1;
        }
    }
}
