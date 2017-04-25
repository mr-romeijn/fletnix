namespace fletnix.Models
{
    public class FletnixRepository : IFletnixRepository
    {
        private FLETNIXContext _context;

        public FletnixRepository(FLETNIXContext context)
        {
            _context = context;
        }
    }
}