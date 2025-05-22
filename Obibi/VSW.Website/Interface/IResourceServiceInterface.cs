namespace VSW.Website.Interface
{
    public interface IResourceServiceInterface
    {
        Task<string> ParseAsync(string content, HttpContext context);
    }
}
