using StratzClone.Server.Models;

namespace StratzClone.Server.Interfaces
{
    public interface IConstantsCache
    {
        Item? GetItem(int id);
        Hero? GetHero(int id);
    }
}

