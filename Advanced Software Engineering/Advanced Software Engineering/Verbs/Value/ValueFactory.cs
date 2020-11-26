﻿using Advanced_Software_Engineering.Verbs.Value.ValueTypes;
using System;

namespace Advanced_Software_Engineering.Verbs.Value {
    class ValueFactory {
        /// <summary>
        /// Creates a constant Value to be used later during execution
        /// </summary>
        /// <param name="value">Text representation of the value</param>
        /// <param name="type">What type the value should be</param>
        /// <returns></returns>
        public IValue CreateValue(string value, string type) {
            switch (type) {
                case "int":
                    return new IntValue(ValueHelper.ConvertToInt(value));

                case "double":
                    return new DoubleValue(ValueHelper.ConvertToDouble(value));

                default:
                    throw new Exception("Unknown expected type: " + type.ToString());

            }
        }




    }
}