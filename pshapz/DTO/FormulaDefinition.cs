using System.Collections.Generic;

namespace pshapz.DTO
{
  public class FormulaDefinition
  {
    public int NumberOfParameters { get; internal set; }
    public IEnumerable<Sequence> Sequence { get; internal set; }
  }
}
