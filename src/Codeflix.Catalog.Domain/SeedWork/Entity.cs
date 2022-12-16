namespace Codeflix.Catalog.Domain.SeedWork;
public abstract class Entity
{
    //Classe abstrata que é usada para enfatizar a entidade como uma raiz de agregação.
    public Guid Id { get; protected set; }

    public Entity() => Id = Guid.NewGuid();

}
