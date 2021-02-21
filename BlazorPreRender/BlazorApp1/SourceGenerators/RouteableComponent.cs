namespace SourceGenerators
{
    public class RouteableComponent
    {
        public string Route { get; }
        public string Title { get; }
        public string Icon { get; }
        public int Order { get; }

        public RouteableComponent(string route, string title, string icon, int order)
        {
            Title = title;
            Icon = icon;
            Order = order;
            Route = route;
        }
    }
}