﻿using System;
using System.Drawing;

namespace Advanced_Software_Engineering.Verbs.Value {

    public class ExpressionValue : IValue {
        private const int ADD = 0;
        private const int SUBTRACT = 1;
        private const int MULTIPLY = 2;
        private const int DIVIDE = 3;

        private Drawer drawer;
        private IValue variable1;
        private IValue variable2;
        private int operation;

        private bool evaluated = false;
        private IValue evaluatedValue;

        public ExpressionValue(Drawer drawer, IValue variable1, IValue variable2, int operation) {
            this.drawer = drawer;
            this.variable1 = variable1;
            this.variable2 = variable2;
            this.operation = operation;

            //Check for bad input
            if (operation > 3 && operation < 0) throw new Exception("Bad operation");
            
        }

        private IValue Evaluate() {
            if (evaluated) return evaluatedValue;

            string outputType = variable1.GetOriginalType();

            switch (outputType) {
                case "bool":
                    //operations as standard for bool expressions eg
                    // (a + b) => a or b, (a * b) => a and b, (a ^ b) => a xor b
                    if (operation == ADD) evaluatedValue = ValueFactory.CreateValue(variable1.ToBool() || variable1.ToBool());
                    else if (operation == SUBTRACT) evaluatedValue = ValueFactory.CreateValue(variable1.ToBool() ^ variable1.ToBool());
                    else if (operation == MULTIPLY) evaluatedValue = ValueFactory.CreateValue(variable1.ToBool() && variable1.ToBool());
                    else if (operation == DIVIDE) throw new Exception("Bool cannot be divided");
                    else throw new Exception("Bad operation");
                    break;

                case "int":
                    if (operation == ADD) evaluatedValue = ValueFactory.CreateValue(variable1.ToInt() + variable1.ToInt());
                    else if (operation == SUBTRACT) evaluatedValue = ValueFactory.CreateValue(variable1.ToInt() - variable1.ToInt());
                    else if (operation == MULTIPLY) evaluatedValue = ValueFactory.CreateValue(variable1.ToInt() * variable1.ToInt());
                    else if (operation == DIVIDE) evaluatedValue = ValueFactory.CreateValue(variable1.ToInt() / variable1.ToInt());
                    else throw new Exception("Bad operation");
                    break;

                case "double":
                    if (operation == ADD) evaluatedValue = ValueFactory.CreateValue(variable1.ToDouble() + variable1.ToDouble());
                    else if (operation == SUBTRACT) evaluatedValue = ValueFactory.CreateValue(variable1.ToDouble() - variable1.ToDouble());
                    else if (operation == MULTIPLY) evaluatedValue = ValueFactory.CreateValue(variable1.ToDouble() * variable1.ToDouble());
                    else if (operation == DIVIDE) evaluatedValue = ValueFactory.CreateValue(variable1.ToDouble() / variable1.ToDouble());
                    else throw new Exception("Bad operation");
                    break;

                case "color":
                    throw new Exception("Colors are immutable");
            }
            evaluated = true;
            return evaluatedValue;
        }

        public string GetDescription() {
            string operation;
            switch (this.operation) {
                case ADD:
                    operation = "added";
                    break;

                case SUBTRACT:
                    operation = "subtracted";
                    break;

                case MULTIPLY:
                    operation = "multiplied";
                    break;

                case DIVIDE:
                    operation = "divided";
                    break;

                default:
                    throw new Exception("Bad operation");
            }
            return "Expression of the variables " + variable1 + " and " + variable2 + " being " + operation;
        }

        public string GetOriginalType() {
            return evaluatedValue.GetOriginalType();
        }

        public bool isInitialised() {
            return true;
        }

        public bool ToBool() {
            if (!evaluated) Evaluate(); 
            return evaluatedValue.ToBool();
        }

        public Color ToColor() {
            if (!evaluated) Evaluate();
            return evaluatedValue.ToColor();
        }

        public double ToDouble() {
            if (!evaluated) Evaluate();
            return evaluatedValue.ToDouble();
        }

        public int ToInt() {
            if (!evaluated) Evaluate();
            return evaluatedValue.ToInt();
        }
    }
}