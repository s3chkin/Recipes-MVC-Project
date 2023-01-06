namespace Recipes.Services
{
    public interface IShortStringService
    {
        string GetShort(string str, int maxLen);
    }
}
