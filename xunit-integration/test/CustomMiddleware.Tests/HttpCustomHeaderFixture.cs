namespace CustomMiddleware.Tests
{
    public class HttpCustomHeaderFixture<TStartup> : CustomHeadersFixtureBase<TStartup>
        where TStartup : class
    {
        public HttpCustomHeaderFixture() : base("http://localhost:5000")
        {
        }
    }
}