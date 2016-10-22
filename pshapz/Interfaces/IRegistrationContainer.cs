using pshapz.DTO;
using System.Collections.Generic;

namespace pshapz.Interfaces
{
  public interface IRegistrationContainer
  {
    void Save(ShapeRegistration registration);
    IEnumerable<ShapeRegistration> ReadAll();
  }
}
