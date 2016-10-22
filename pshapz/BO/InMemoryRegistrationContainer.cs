using System.Collections.Generic;
using pshapz.Interfaces;

namespace pshapz.DTO
{
  public class InMemoryRegistrationContainer : IRegistrationContainer
  {
    private List<ShapeRegistration> list = new List<ShapeRegistration>();

    public IEnumerable<ShapeRegistration> ReadAll()
    {
      return list;
    }

    public void Save(ShapeRegistration registration)
    {
      list.Add(registration);
    }
  }
}
