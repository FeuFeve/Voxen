using Voxen.Entities;

namespace Voxen.Renderers;

public interface IRenderer<T> where T : IEntity
{
    #region Public methods

    public void Render(T entity);
    
    #endregion
}