namespace Bookstore.API.Models
{
    public interface IEntity<TKey>
    {
        TKey GetID();
    }
}
