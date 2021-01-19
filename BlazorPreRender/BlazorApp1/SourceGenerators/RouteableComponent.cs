namespace SourceGenerators
{
    public class RouteableComponent
    {
        public string Route { get; }
        public string Title { get; }

        public RouteableComponent(string route, string title)
        {
            Title = title;
            Route = route;
        }
    }
}