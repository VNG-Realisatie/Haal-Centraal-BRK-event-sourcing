namespace KadastraalOnroerendeZaken.API.Domain.Entities
{
    public abstract class Entity
    {
        public long Id { get; set; }

        protected Entity()
        {
        }

        protected Entity(long id)
            : this()
        {
            Id = id;
        }
    }
}
