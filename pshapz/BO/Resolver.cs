using pshapz.DTO;
using pshapz.Interfaces;
using System.Linq;
using System.Collections.Generic;

namespace pshapz.BO
{
  public class Resolver
  {

    public static FormulaDefinition GetFormula(IDataContext _context, int pos)
    {
      var l = _context.ShapeRegistration.ReadAll().ToArray();
      var v = l[pos];
      var nparameters = 0;
      v.Formulation.ForEach(o =>
      {
        if (IsMember(o))
        {
          nparameters += 1;
        }
      });
      return new FormulaDefinition
      {
        NumberOfParameters = nparameters,
        Sequence = v.Formulation
      };
    }

    private static bool IsMember(Sequence o)
    {
      return o.OperationType == OperationSequence.Asignation || o.OperationType == OperationSequence.Constant;
    }

    public static decimal ApplyFormula(FormulaDefinition perimeterFormula, List<decimal> values)
    {
      var stack = GetStack(perimeterFormula, values);
      foreach (var step in perimeterFormula.Sequence.Where(o => !IsMember(o)))
      {
        var last = stack.Count() - 1;
        SumIt(stack, step, last);
        MultiplyIt(stack, step, last);
        PowerOf2(stack, step, last);
        if (step.OperationType != OperationSequence.PowerOf2)
        {
          stack = RePopulate(stack);
        }
      }
      return stack[0];
    }

    private static void SumIt(List<decimal> stack, Sequence step, int last)
    {
      if (step.OperationType == OperationSequence.Sums)
      {
        stack[last - 1] = stack[last - 1] + stack[last];
      }
    }

    private static void MultiplyIt(List<decimal> stack, Sequence step, int last)
    {
      if (step.OperationType == OperationSequence.Times)
      {
        stack[last - 1] = stack[last - 1] * stack[last];
      }
    }

    private static void PowerOf2(List<decimal> stack, Sequence step, int last)
    {
      if (step.OperationType == OperationSequence.PowerOf2)
      {
        stack[last] = stack[last] * stack[last];
      }
    }

    private static List<decimal> RePopulate(List<decimal> stack)
    {
      var list = new List<decimal>();
      for (var i = 0; i < stack.Count() - 1; i++)
      {
        list.Add(stack[i]);
      }
      return list;
    }

    private static List<decimal> GetStack(FormulaDefinition perimeterFormula, List<decimal> values)
    {
      var stack = new List<decimal>();
      var pos = 0;
      foreach (var step in perimeterFormula.Sequence.Where(IsMember))
      {
        if (step.OperationType == OperationSequence.Constant)
        {
          stack.Add(step.ConstantValue);
        }
        else
        {
          stack.Add(values[pos]);
          pos++;
        }
      }
      return stack;
    }
  }
}
