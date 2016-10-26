using System;
using pshapz.Interfaces;

namespace pshapz.BO
{
  public class FileContext : IDataContext
  {
    
    private FileRegistrationContainer _registration;

    public FileContext()
    {
      _registration = new FileRegistrationContainer(@"shapes.json");
    }

    public IRegistrationContainer ShapeRegistration
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
