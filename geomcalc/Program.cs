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
    static void Main(string[] args)
    {
      IDataContext context = new InMemoryContext();
      Init(context);
      var option = string.Empty;
      while (!option.Equals("4"))
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

      }
    }

    private static void PrintAddFormula(string option, IDataContext context)
    {
      if (option.Equals("2"))
      {
        var list = new List<Sequence>();
        Console.WriteLine("Enter name for that formula:");
        var formulaName = Console.ReadLine();
        var formulaMenu = string.Empty;
        while (!formulaMenu.Equals("4"))
        {
          Console.WriteLine("  Options:");
          Console.WriteLine("  1. Add Step");
          Console.WriteLine("  4. Exit Step");
          formulaMenu = Console.ReadLine();
          if (formulaMenu.Equals("1"))
          {
            Console.WriteLine("  Type: x to enter variable and + to sum");
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
        Console.WriteLine("----------------------");
      }
    }

    private static void PrintMenu()
    {
      Console.WriteLine("Geometric Calculator");
      Console.WriteLine("1. List formulas");
      Console.WriteLine("2. Add formula");
      Console.WriteLine("3. Select formula");
      Console.WriteLine("4. Exit");
      Console.WriteLine("Type a number:");
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
