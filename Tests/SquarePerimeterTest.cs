using Microsoft.VisualStudio.TestTools.UnitTesting;
using pshapz.BO;
using pshapz.DTO;
using pshapz.Interfaces;
using System.Collections.Generic;

namespace Tests
{
  [TestClass]
  public class SquarePerimeterTest
  {
    private IDataContext _context;

    [TestInitialize]
    public void Init()
    {
      _context = new InMemoryContext();
      var side = new Sequence { OperationType = OperationSequence.Asignation, Literal = new Measure { Description = "Side", Literal = "s" } };
      var numberOfSides = new Sequence { OperationType = OperationSequence.Constant, ConstantValue = 4M };
      var multiplicationOperation = new Sequence { OperationType = OperationSequence.Times };
      var registration = new ShapeRegistration { Name = "Square perimeter", Formulation = new List<Sequence> { side, numberOfSides, multiplicationOperation } };
      _context.ShapeRegistration.Save(registration);
    }

    [TestMethod]
    public void ShouldResolveSquarePerimeter()
    {
      var perimeterFormula = Resolver.GetFormula(_context, 0);
      var numberOfParams = perimeterFormula.NumberOfParameters;
      var perimeter = Resolver.ApplyFormula(perimeterFormula, GetValuesForParametersFromUI(numberOfParams));
      Assert.AreEqual(perimeter, 16);
    }

    private static List<decimal> GetValuesForParametersFromUI(int numberOfParams)
    {
      var values = new List<decimal>();
      values.Add(4);
      return values;
    }
  }
}
