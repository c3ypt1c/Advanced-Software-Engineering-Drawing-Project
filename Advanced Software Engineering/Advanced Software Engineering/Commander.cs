﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace Advanced_Software_Engineering {

    /// <summary>
    /// The commander class takes care of commands. It also takes control of the drawer.
    /// </summary>
    public class Commander {
        private Graphics graphics;
        private List<IVerb> commands = new List<IVerb>();
        private readonly Drawer drawer;
        private bool commands_ok = true;

        /// <summary>
        /// This command updates the graphics element (usually) when the page repaints.
        /// </summary>
        /// <param name="updateGraphicsElement">The graphics element to be updated</param>
        public void DrawAllCommands(Graphics updateGraphicsElement) {
            graphics = updateGraphicsElement;
            drawer.ResetDrawer();
            drawer.SetGraphics(updateGraphicsElement);

            foreach (IVerb IVerb in commands) {
                IVerb.ExecuteVerb();
            }
        }

        /// <summary>
        /// Create a new Commander instance.
        /// </summary>
        /// <param name="graphics">initial graphics object</param>
        public Commander(Graphics graphics) {
            this.graphics = graphics;
            this.drawer = new Drawer(this.graphics);
            drawer.ResetDrawer();
        }

        /// <summary>
        /// Creates a new Commander instance without a context. Should only be used for testing purpouses.
        /// </summary>
        public Commander() {
            Console.WriteLine("Warning! No graphics context!!!");
        }

        /// <summary>
        /// Creates a new Commander instance with a graphics context and commands to process right off the bat.
        /// </summary>
        /// <param name="graphics">Graphics contetx</param>
        /// <param name="rawCommands">Commands to process</param>
        public Commander(Graphics graphics, string rawCommands) {
            this.graphics = graphics;
            this.drawer = new Drawer(this.graphics);
            this.ProcessCommands(rawCommands);
            drawer.ResetDrawer();
        }

        /// <summary>
        /// Adds a IVerb to the command list to be executed.
        /// </summary>
        /// <param name="command">IVerb to be added. <seealso cref="IVerb"/></param>
        public void AddCommand(IVerb command) {
            commands.Add(command);
        }

        /// <summary>
        /// processes commands. Removes all commands if it fails parsing the raw commands.
        /// </summary>
        /// <param name="rawCommands">Processes commands seperated by \n or \r\n</param>
        /// <param name="pardonCommands">If the command is wrong, don't remove all of the commands</param>
        public void ProcessCommands(string rawCommands, bool pardonCommands = false) {
            //ignore case
            rawCommands = rawCommands.ToLower();
            Console.WriteLine("Processing:\n" + rawCommands);

            //remove windows' \r if they exist
            rawCommands = rawCommands.Replace("\r", "");

            //isolate commands
            string[] commands = rawCommands.Split("\n"[0]);

            Console.WriteLine("\nThe rawCommand processed:");
            int lineNumber = 0;
            bool failed = false;
            foreach (string rawCommand in commands) {
                string fullCommand = rawCommand;
                lineNumber++;
                //Remove comments
                if (fullCommand.Contains("@")) {
                    fullCommand = fullCommand.Substring(0, fullCommand.IndexOf("@"));
                }

                fullCommand = HelperFunctions.Strip(fullCommand);
                if (fullCommand == "") continue;

                try {
                    AddCommand(VerbFactory.MakeVerb(drawer, fullCommand));
                    Console.WriteLine(rawCommand);
                } catch (Exception e) {
                    Console.WriteLine("Error!");
                    new ErrorWindow(e.Message, e.Message + " at line " + lineNumber, "While trying to process '" + rawCommand + "' an exception occured.\nMessage:\n" + e.Message + "\nStack Trace:\n" + e.StackTrace, ErrorWindow.ERROR_MESSAGE).Show();
                    failed = true;
                    break;
                }
            }

            if (failed) {
                if (!pardonCommands) RemoveAllCommands();
                commands_ok = false;
            } else {
                commands_ok = true;
            }

            Console.WriteLine("\n\nHere is what the program is going to do:");
            Console.WriteLine("Set origin to 0, 0");
            foreach (IVerb IVerb in this.commands) {
                Console.WriteLine(IVerb.GetDescription());
            }
        }

        /// <summary>
        /// Processes the commands and executes them at the same time. It's the same as <see></see>
        /// </summary>
        /// <param name="rawCommands">Processes commands seperated by \n or \r\n</param>
        /// <param name="pardonCommands">If the command is wrong, don't remove all of the commands</param>
        public void ProcessCommandsAndExecute(string rawCommands, bool pardonCommands = false) {
            int start = commands.Count;
            ProcessCommands(rawCommands, pardonCommands);

            for (; start < commands.Count; start++) {
                try {
                    commands[start].ExecuteVerb();
                } catch (Exception e) {
                    IVerb tmp = commands[start];
                    if (!pardonCommands) RemoveAllCommands();
                    else commands.Remove(commands[start]);
                    new ErrorWindow("Runtime Error", "Failed to " + tmp.GetDescription(), "While trying to '" + tmp.GetDescription() + "' an exception occured.\nMessage:\n" + e.Message + "\nStack Trace:\n" + e.StackTrace, ErrorWindow.ERROR_MESSAGE).Show();
                    break;
                }
            }
        }

        /// <summary>
        /// Removes all commands from the Commander and resets the drawer.
        /// </summary>
        public void RemoveAllCommands() {
            commands = new List<IVerb>();
            drawer.ResetDrawer();
        }

        /// <summary>
        /// Explains all of the commands in order
        /// </summary>
        /// <returns>Explanation of all the commands</returns>
        public string ExplainCommands() {
            string desc = "";
            foreach (IVerb command in commands) {
                desc += command.GetDescription() + Environment.NewLine;
            }
            return desc;
        }

        /// <summary>
        /// Gets the status of the code that was processed
        /// </summary>
        /// <returns>False if there was no error. True if there was an error</returns>
        public bool GetLastStatus() {
            return commands_ok;
        }
    }
}