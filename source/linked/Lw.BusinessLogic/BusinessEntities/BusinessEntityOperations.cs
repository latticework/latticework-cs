
namespace Lw.BusinessEntities
{
    public static class BusinessEntityOperations
    {
        public static TKey CreateKey<TKey>(string keyToken)
            where TKey : IBusinessEntityKey, new()
        {
            var key = new TKey();
            key.LoadToken(keyToken);
            return key;
        }
    }
}
