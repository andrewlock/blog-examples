namespace CustomMiddleware.Tests
{
    public class Http2CustomHeaderFixture<TStartup> : CustomHeadersFixtureBase<TStartup>
        where TStartup : class
    {
        public Http2CustomHeaderFixture() : base("https://localhost:5000")
        {
        }
    }
}