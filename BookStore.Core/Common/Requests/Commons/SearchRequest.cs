namespace BookStore.Core.Common.Requests.Commons
{
    public class SearchRequest<TEntity, TFilter> where TEntity:IIdentifier where TFilter : IFilter<TEntity>
    {
        public Pagination Pagination { get; set; } = new Pagination();
        public Ordering Ordering { get; set; } = new Ordering();
        public TFilter Filtering { get; set; }
    }
}