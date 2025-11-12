namespace ApplicationCoreLibrary.Entities.Base
{
    public interface IBaseEntity
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
