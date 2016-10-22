﻿using pshapz.DTO;
using pshapz.Interfaces;
using System.Linq;
using System;
using System.Collections.Generic;

namespace pshapz.BO
{
  public class PerimeterResolver
  {

    public static FormulaDefinition GetFormula(IDataContext _context, int pos)
    {
      var l = _context.ShapeRegistration.ReadAll().ToArray();
      var v = l[pos];
      var nparameters = 0;
      v.Formulation.ForEach(o =>
      {
        if (o.OperationType == OperationSequence.Asignation)
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

    public static decimal ApplyFormula(FormulaDefinition perimeterFormula, List<decimal> values)
    {
      var stack = GetStack(perimeterFormula, values);
      foreach (var step in perimeterFormula.Sequence.Where(o => o.OperationType == OperationSequence.Sums))
      {
        stack[1] = stack[0] + stack[1];
        stack = RePopulate(stack);
      }
      return stack[0];
    }

    private static List<decimal> RePopulate(List<decimal> stack)
    {
      var list = new List<decimal>();
      for (var i = 1; i < stack.Count(); i++)
      {
        list.Add(stack[i]);
      }
      return list;
    }

    private static List<decimal> GetStack(FormulaDefinition perimeterFormula, List<decimal> values)
    {
      var stack = new List<decimal>();
      var pos = 0;
      foreach (var step in perimeterFormula.Sequence.Where(o => o.OperationType == OperationSequence.Asignation))
      {
        stack.Add(values[pos]);
        pos++;
      }
      return stack;
    }
  }
}