using System.Collections.Generic;
using pshapz.DTO;
using pshapz.Interfaces;
using System.IO;
using Newtonsoft.Json;

namespace pshapz.BO
{
  internal class FileRegistrationContainer : IRegistrationContainer
  {
    private string _fileName;

    public FileRegistrationContainer(string fileName)
    {
      _fileName = fileName;
    }

    public IEnumerable<ShapeRegistration> ReadAll()
    {
      return GetList();
    }

    public void Save(ShapeRegistration registration)
    {
      var list = GetList();
      list.Add(registration);
      var json = JsonConvert.SerializeObject(list.ToArray());
      File.WriteAllText(_fileName, json);
    }

    private List<ShapeRegistration> GetList()
    {
      var list = new List<ShapeRegistration>();
      using (StreamReader r = new StreamReader(_fileName))
      {
        var json = r.ReadToEnd();
        list = JsonConvert.DeserializeObject<List<ShapeRegistration>>(json);
      }
      return list;
    }
  }
}