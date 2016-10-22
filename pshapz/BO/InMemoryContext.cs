using System;
using pshapz.DTO;
using pshapz.Interfaces;

namespace pshapz.BO
{
  public class InMemoryContext : IDataContext
  {
    private InMemoryRegistrationContainer _registration;

    public InMemoryContext()
    {
      _registration = new InMemoryRegistrationContainer();
    }

    IRegistrationContainer IDataContext.ShapeRegistration
    {
      get
      {
        return _registration;
      }
      set
      {
        throw new NotImplementedException();
      }
    }
  }
}
