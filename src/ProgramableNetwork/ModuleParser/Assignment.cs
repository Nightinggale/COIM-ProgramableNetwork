﻿using System;
using System.Collections.Generic;

namespace ProgramableNetwork.Python
{
    public class Assignment : IStatement
    {
        private IExpression target;
        private IExpression value;

        public Assignment(IExpression qualifiedName, IExpression expression)
        {
            this.target = qualifiedName;
            this.value = expression;
        }

        public string Name => target is QualifiedName name ? name.Concat : throw new InvalidCastException("Target is not qualified name");

        public IExpression Value => value;

        public void Execute(IDictionary<string, dynamic> context)
        {
            target.GetReference(context).Value = value.GetValue(context);
        }
    }
}