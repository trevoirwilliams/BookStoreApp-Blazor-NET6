namespace BookStoreApp.Blazor.Server.UI.Services.Base
{
    public partial interface IClient
    {
        public HttpClient HttpClient { get; }
    }
}
