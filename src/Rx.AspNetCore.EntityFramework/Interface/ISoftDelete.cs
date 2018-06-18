namespace Rx.AspNetCore.EntityFramework
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
