namespace BookStoreApp.Blazor.WebAssembly.UI.Services.Base
{
    public partial interface IClient
    {
        public HttpClient HttpClient { get; }
    }
}
