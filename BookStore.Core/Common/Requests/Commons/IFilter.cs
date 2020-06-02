using System.Linq;

namespace BookStore.Core.Common.Requests.Commons
{
    public interface IFilter<T> where T: IIdentifier
    {
        public string SearchTerm { get; set; }
        IQueryable<T> Filter(IQueryable<T> filterQuery);
    }
}