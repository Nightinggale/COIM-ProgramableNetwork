﻿using Mafi.Core.Factory.Lifts;
using System.Collections.Generic;
using UnityEngine;

namespace ProgramableNetwork.Python
{
    public class NotEqualExpression : ABinaryOperatorExpression, IComparison
    {
        public NotEqualExpression(IExpression left, IExpression right) : base(left, right)
        {
        }

        public IExpression Left => left;

        public IExpression Right => right;

        protected override object Evaluate(object left, object right)
        {
            if (left is null && right is null)
            {
                return true;
            }
            if (left is null || right is null)
            {
                return false;
            }
            return Expressions.__ne__(left, right);
        }
    }
}