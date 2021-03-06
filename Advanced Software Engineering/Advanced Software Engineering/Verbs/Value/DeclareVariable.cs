﻿using Advanced_Software_Engineering.Verbs.Value.ValueTypes;
using System;

namespace Advanced_Software_Engineering.Verbs.Value {

    /// <summary>
    /// Declares a variable
    /// </summary>
    public class DeclareVariable : IVerb {
        private readonly ValueStorage storage;
        private readonly IValue value;
        private readonly string name;

        //Common characters that people might try
        private static readonly char[] illegalCharacters = "1234567890!?/+- \"'@#~;:><,.`¬|[]{}\\£$%^&*()".ToCharArray();

        private static readonly string[] illegalNames =
            { "var",
              "move", "moveto",
              "drawto", "line", "lineto",
              "regularpolygon", "rp",
              "square",
              "quadrilateral",
              "rectangle",
              "circle",
              "triangle",
              "dot",
              "clear",
              "reset", "resetpen",
              "fillon", "filloff",
              "pen",
              "fill",
              "if", "def", "ret"
            };

        /// <summary>
        /// Checks if the name provided is valid
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <returns>True if the name is valid, otherwise false</returns>
        private bool CheckName(string name) {
            foreach (char character in illegalCharacters) {
                if (name.Contains(character.ToString())) return false;
            }

            foreach (string illegalName in illegalNames) {
                if (illegalName == name) return false;
            }

            return true;
        }

        /// <summary>
        /// Creates a new Declare variable
        /// </summary>
        /// <param name="storage">The storage to use</param>
        /// <param name="assignment">The assignment</param>
        public DeclareVariable(ValueStorage storage, string assignment) {
            this.storage = storage;

            //seperate assignment characters from everything else
            foreach (string op in new string[] { "=", "+", "-", "*", "/" }) {
                assignment = assignment.Replace(op, " " + op + " ");
            }

            // Get variable name
            // assignment looks something like this at this point:
            // i = 20
            // or
            // i = 2 = 5
            // so after splitting it should look like
            // ["i", "=", "20"]
            assignment = HelperFunctions.Strip(assignment);

            //seperate on spaces
            string[] assignmentStrings = HelperFunctions.StripStringArray(assignment.Split(" "[0])).ToArray();

            //check if is actually assignment
            if (assignmentStrings.Length > 1 && !(assignmentStrings[1] == "=")) throw new Exception("No assignment in " + assignment);

            name = assignmentStrings[0];
            if (storage.CheckVariableExists(name)) throw new Exception("Declaration failed. " + name + " has been declared before");
            if (!CheckName(name)) throw new Exception("'" + name + "' name not allowed");

            //preassign name
            storage.SetVariable(name, null);

            //for single number assignments ["i", "=", "20"]
            if (assignmentStrings.Length == 3) value = ValueFactory.CreateValue(storage, assignmentStrings[2]);

            //for expressions ["i", "=", "20", "+", "30"]
            //or some comparisons ["i", "=", "20", ">", "30"]
            else if (assignmentStrings.Length == 5) {
                string op = assignmentStrings[3];
                IValue value1 = ValueFactory.CreateValue(storage, assignmentStrings[2]);
                IValue value2 = ValueFactory.CreateValue(storage, assignmentStrings[4]);

                switch (op) {
                    case "+":
                    case "-":
                    case "/":
                    case "*":
                        value = new ExpressionValue(value1, value2, op);
                        break;

                    case ">":
                    case "<":
                    case "=":
                    case "!":
                        value = new ComparisonValue(value1, value2, op);
                        break;

                    default:
                        throw new Exception("Bad operation");
                }
            }
        }

        /// <summary>
        /// Sets the variable to null
        /// </summary>
        public void ExecuteVerb() {
            if (value != null) storage.SetVariable(name, value.Clone());
            else storage.SetVariable(name, value);
        }

        /// <summary>
        /// Gets the description of the verb
        /// </summary>
        /// <returns></returns>
        public string GetDescription() {
            return "Declares the variable " + name;
        }

        /// <summary>
        /// Gets the name of the variable to assign
        /// </summary>
        /// <returns></returns>
        public string GetName() {
            return name;
        }
    }
}