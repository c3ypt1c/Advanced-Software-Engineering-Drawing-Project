﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Advanced_Software_Engineering {
    public class VerbFactory {
        public static Verb MakeVerb(Drawer drawer, string fullCommand) {

            Dictionary<string, string[]> commandAndParameters = SettingsAndHelperFunctions.CommandAndParameterParser(fullCommand);

            if (!commandAndParameters.Keys.Contains("command")) throw new Exception("There is no command to process");

            string command = commandAndParameters["command"][0];
            string[] parameters = commandAndParameters.Keys.Contains("parameters") ? commandAndParameters["parameters"] : new string[] { };

            switch (command) {
                case "move":
                case "moveto":
                    //Check parameters
                    if (parameters.Length == 2) {
                        return new MoveTo(drawer,
                            Value.ConvertToInt(parameters[0]),
                            Value.ConvertToInt(parameters[1]));

                    } else throw new Exception("Command has an incorrect number of parameters");

                case "drawto":
                case "line":
                case "lineto":
                    //Check parameters
                    if (parameters.Length == 2) {
                        return new
                            DrawTo(drawer,
                            Value.ConvertToInt(parameters[0]),
                            Value.ConvertToInt(parameters[1]));
                    } else throw new Exception("Command has an incorrect number of parameters");
                case "regularpolygon":
                case "rp":
                    //Check parameters
                    if (parameters.Length == 2) {
                        return new
                            RegularPolygon(drawer,
                            Value.ConvertToInt(parameters[0]),
                            Value.ConvertToDouble(parameters[1])
                            );
                    } else if (parameters.Length == 3) {
                        return new RegularPolygon(drawer,
                            Value.ConvertToInt(parameters[0]),
                            Value.ConvertToDouble(parameters[1]),
                            Value.ConvertToDouble(parameters[2])
                            );
                    } else throw new Exception("Command has an incorrect number of parameters");
                case "square":
                    //Check parameters
                    if (parameters.Length == 1) {
                        return new
                            Square(drawer,
                            Value.ConvertToDouble(parameters[0])
                            );
                    } else if (parameters.Length == 2) {
                        return new Square(drawer,
                            Value.ConvertToDouble(parameters[0]),
                            Value.ConvertToDouble(parameters[1])
                            );
                    } else throw new Exception("Command has an incorrect number of parameters");
                case "quadrilateral":
                    //Check parameters
                    if (parameters.Length == 8) {
                        return new Quadrilateral(drawer,
                            Value.ConvertToPoint(parameters[0], parameters[1]),
                            Value.ConvertToPoint(parameters[2], parameters[3]),
                            Value.ConvertToPoint(parameters[4], parameters[5]),
                            Value.ConvertToPoint(parameters[6], parameters[7])
                            );
                    } else throw new Exception("Command has an incorrect number of parameters");
                case "rectangle":
                    //Check parameters
                    if (parameters.Length == 2) {
                        return new Rectangle(drawer,
                            Value.ConvertToDouble(parameters[0]),
                            Value.ConvertToDouble(parameters[1])
                            );
                    } else if (parameters.Length == 8) {
                        return new Quadrilateral(drawer,
                            Value.ConvertToPoint(parameters[0], parameters[1]),
                            Value.ConvertToPoint(parameters[2], parameters[3]),
                            Value.ConvertToPoint(parameters[4], parameters[5]),
                            Value.ConvertToPoint(parameters[6], parameters[7])
                            );
                    } else if (parameters.Length == 4) {
                        return new Rectangle(drawer,
                            Value.ConvertToPoint(parameters[0], parameters[1]),
                            Value.ConvertToPoint(parameters[2], parameters[3])
                            );
                    } else throw new Exception("Command has an incorrect number of parameters");
                case "circle":
                    //Check parameters
                    if (parameters.Length == 1) {
                        return new
                            Circle(drawer,
                            Value.ConvertToDouble(parameters[0])
                            );
                    } else throw new Exception("Command has an incorrect number of parameters");
                case "triangle":
                    //Check parameters
                    if (parameters.Length == 1) {
                        return new
                            Triangle(drawer,
                            Value.ConvertToDouble(parameters[0])
                            );
                    } else if (parameters.Length == 2) {
                        return new
                            Triangle(drawer,
                            Value.ConvertToDouble(parameters[0]),
                            Value.ConvertToDouble(parameters[1])
                            );
                    } else if (parameters.Length == 6) {
                        return new Triangle(drawer,
                            Value.ConvertToPoint(parameters[0], parameters[1]),
                            Value.ConvertToPoint(parameters[2], parameters[3]),
                            Value.ConvertToPoint(parameters[4], parameters[5])
                            );
                    } else throw new Exception("Command has an incorrect number of parameters");

                case "dot":
                case "clear":
                    return new Clear(drawer);
                case "pen":
                case "fill":
                    throw new Exception("Not implemented");

                default:
                    throw new Exception("Command not found");
            }

        }
    }
}