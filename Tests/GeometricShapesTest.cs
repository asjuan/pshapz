using Microsoft.VisualStudio.TestTools.UnitTesting;
using pshapz.BO;
using pshapz.DTO;
using pshapz.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
  [TestClass]
  public class GeometricShapesTest
  {
    private IDataContext _context;

    [TestInitialize]
    public void Init()
    {
      _context = new InMemoryContext();
      var side1 = new Sequence { OperationType = OperationSequence.Asignation, Literal = new Measure { Description = "Side 1", Literal = "s1" } };
      var side2 = new Sequence { OperationType = OperationSequence.Asignation, Literal = new Measure { Description = "Side 2", Literal = "s2" } };
      var side3 = new Sequence { OperationType = OperationSequence.Asignation, Literal = new Measure { Description = "Side 3", Literal = "s3" } };
      var additionOperation = new Sequence { OperationType = OperationSequence.Sums };
      var registration = new ShapeRegistration { Name = "Triangle Perimeter", Formulation = new List<Sequence> { side1, side2, side3, additionOperation, additionOperation } };
      _context.ShapeRegistration.Save(registration);
    }

    [TestMethod]
    public void ShouldRegisterMeasure()
    {
      Assert.AreEqual(_context.ShapeRegistration.ReadAll().ToList().Count(), 1);
    }

    [TestMethod]
    public void ShouldResolvePerimeterOfEquilateralTriangle()
    {
      var perimeterFormula = PerimeterResolver.GetFormula(_context, 0);
      var numberOfParams = perimeterFormula.NumberOfParameters;
      var values = GetValuesForParametersFromUI(numberOfParams);
      var perimeter = PerimeterResolver.ApplyFormula(perimeterFormula, values);
      Assert.AreEqual(perimeter, 3);
    }

    [TestMethod]
    public void ShouldResolvePerimeterOfOtherTriangle()
    {
      var perimeterFormula = PerimeterResolver.GetFormula(_context, 0);
      var perimeter = PerimeterResolver.ApplyFormula(perimeterFormula, new List<decimal> { 1, 2, 2.23M });
      Assert.AreEqual(perimeter, 5.23M);
    }

    private static List<decimal> GetValuesForParametersFromUI(int numberOfParams)
    {
      var values = new List<decimal>();
      for (var i = 0; i < numberOfParams; i++)
      {
        values.Add(1);
      }
      return values;
    }
  }
}
