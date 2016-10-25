using pshapz.BO;
using pshapz.DTO;
using pshapz.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace geomcalc
{
  class Program
  {
    private const string ExitCode = "e";

    static void Main(string[] args)
    {
      IDataContext context = new InMemoryContext();
      Init(context);
      var option = string.Empty;
      while (!option.Equals(ExitCode))
      {
        PrintMenu();
        option = Console.ReadLine();
        PrintFormulas(context, option);
        PrintAddFormula(option, context);
        PrintUseFormula(context, option);
      }
    }

    private static void PrintUseFormula(IDataContext context, string option)
    {
      if (option.Equals("3"))
      {
        Console.WriteLine("Select formula");
        var formulaId = Console.ReadLine();
        var folulas = context.ShapeRegistration.ReadAll().ToArray();
        var formula = folulas[int.Parse(formulaId) - 1];
        Console.WriteLine($"{formula.Name}");
        var steps = formula.Formulation;
        var list = new List<decimal>();
        foreach(var step in steps.Where(o=>o.OperationType== OperationSequence.Asignation))
        {
          Console.WriteLine($"==={step.Literal.Description}===");
          Console.Write($"{step.Literal.Literal} = ");
          var l = Console.ReadLine();
          list.Add(decimal.Parse(l));
        }
        var result = PerimeterResolver.ApplyFormula(PerimeterResolver.GetFormula(context, int.Parse(formulaId) - 1), list);
        Console.WriteLine($"***The result is {result}***");
      }
    }

    private static void PrintAddFormula(string option, IDataContext context)
    {
      if (option.Equals("2"))
      {
        var list = new List<Sequence>();
        Console.WriteLine("Enter name for that formula:");
        var formulaName = Console.ReadLine();
        Console.WriteLine("  Options:");
        Console.WriteLine("  +  To perform addition");
        Console.WriteLine("  x  To requiere a variable");
        Console.WriteLine($"  {ExitCode}. Exit Step");
        var formulaMenu = string.Empty;
        while (!formulaMenu.Equals(ExitCode))
        {
          formulaMenu = Console.ReadLine();
          if (formulaMenu.Equals("+"))
          {
            list.Add(new Sequence { OperationType = OperationSequence.Sums });
          }
          if (formulaMenu.Equals("x"))
          {
            Console.WriteLine("Enter a description for that variable");
            var xName = Console.ReadLine();
            Console.WriteLine("Enter short name for that literal");
            var lName = Console.ReadLine();
            list.Add(new Sequence { OperationType = OperationSequence.Asignation, Literal = new Measure { Literal = lName, Description = xName } });
          }
        }
        context.ShapeRegistration.Save(new ShapeRegistration { Name = formulaName, Formulation = list });
      }
    }

    private static void PrintFormulas(IDataContext context, string option)
    {
      if (option.Equals("1"))
      {
        Console.WriteLine("--Available Formulas--");
        var list = context.ShapeRegistration.ReadAll().ToList();
        for (var i = 0; i < list.Count(); i++)
        {
          var item = list[i];
          Console.WriteLine($"{i + 1}.- {item.Name}");
        }
        Console.WriteLine("--Available Formulas--");
      }
    }

    private static void PrintMenu()
    {
      Console.WriteLine("Geometric Calculator");
      Console.WriteLine($"1. List formulas | 2. Add formula |3. Select formula | {ExitCode}. Exit");
      Console.Write("Type a number:");
    }

    private static void Init(IDataContext context)
    {
      var side1 = new Sequence { OperationType = OperationSequence.Asignation, Literal = new Measure { Description = "Side 1", Literal = "s1" } };
      var side2 = new Sequence { OperationType = OperationSequence.Asignation, Literal = new Measure { Description = "Side 2", Literal = "s2" } };
      var side3 = new Sequence { OperationType = OperationSequence.Asignation, Literal = new Measure { Description = "Side 3", Literal = "s3" } };
      var additionOperation = new Sequence { OperationType = OperationSequence.Sums };
      var registration = new ShapeRegistration { Name = "Triangle Perimeter", Formulation = new List<Sequence> { side1, side2, side3, additionOperation, additionOperation } };
      context.ShapeRegistration.Save(registration);
    }

  }
}
