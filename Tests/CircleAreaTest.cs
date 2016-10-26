using Microsoft.VisualStudio.TestTools.UnitTesting;
using pshapz.BO;
using pshapz.DTO;
using pshapz.Interfaces;
using System.Collections.Generic;

namespace Tests
{
  [TestClass]
  public class CircleAreaTest
  {
    private IDataContext _context;

    [TestInitialize]
    public void Init()
    {
      _context = new InMemoryContext();
      var radius = new Sequence { OperationType = OperationSequence.Asignation, Literal = new Measure { Description = "Radius", Literal = "r" } };
      var pi = new Sequence { OperationType = OperationSequence.Constant, ConstantValue = 3.14159M };
      var powerOf2Operation = new Sequence { OperationType = OperationSequence.PowerOf2 };
      var multiplicationOperation = new Sequence { OperationType = OperationSequence.Times };
      var registration = new ShapeRegistration { Name = "Square perimeter", Formulation = new List<Sequence> { pi, radius, powerOf2Operation, multiplicationOperation } };
      _context.ShapeRegistration.Save(registration);
    }

    [TestMethod]
    public void ShouldResolveCircleArea()
    {
      var perimeterFormula = Resolver.GetFormula(_context, 0);
      var perimeter = Resolver.ApplyFormula(perimeterFormula, new List<decimal> { 2 });
      Assert.AreEqual(perimeter, 12.56636M);
    }
  }
}
