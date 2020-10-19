﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Advanced_Software_Engineering {
    class Commander {

        Graphics graphics;
        List<Verb> commands = new List<Verb>();
        Drawer drawer;

        public void DrawAllCommands() {
            foreach (Verb verb in this.commands) {
                verb.ExecuteVerb();
            }
        }

        public Commander(Graphics graphics) {
            this.graphics = graphics;
            this.drawer = new Drawer(this.graphics);
        }

        public Commander(Graphics graphics, string rawCommands) {
            this.graphics = graphics;
            this.drawer = new Drawer(this.graphics);
            this.ProcessCommands(rawCommands);
        }

        public void AddCommand(Verb command) {
            commands.Add(command);
        }

        public void ProcessCommands(string rawCommands) {
            //ignore case
            rawCommands = rawCommands.ToLower();
            Console.WriteLine("Processing:\n" + rawCommands);

            //isolate commands
            string[] commands = rawCommands.Split(Environment.NewLine.ToCharArray());

            Console.WriteLine("\nThe rawCommand processed:");
            foreach (string rawCommand in commands) {
                if (rawCommand == "") continue;
                AddCommand(VerbFactory.MakeVerb(drawer, rawCommand));
                Console.WriteLine(rawCommand);
            }

            Console.WriteLine("\n\nHere is what the program is going to do:");
            Console.WriteLine("Set origin to 0, 0");
            foreach(Verb verb in this.commands) {
                Console.WriteLine(verb.GetDescription());
            }
        }

        public void ProcessCommandsAndExecute(string rawCommands) {
            int start = commands.Count;
            ProcessCommands(rawCommands);

            for (; start < commands.Count; start++) {
                commands[start].ExecuteVerb();
            }
        }

        public void RemoveAllCommands() {
            commands = new List<Verb>();
        }

    }

    class Drawer {
        protected static Color defaultColor = Color.Black;
        protected static float defaultWidth = 1f;
        protected bool fill = false;


        protected Brush brush;
        protected Pen pen;


        protected Point penPosition;
        protected Graphics graphics;

        public Drawer(Graphics graphics) {
            this.graphics = graphics;
            pen = new Pen(defaultColor, defaultWidth);
            brush = new SolidBrush(defaultColor);
        }

        public void MovePen(Point point) {
            this.penPosition = point;
        }

        public bool GetFill() => fill;
        public void EnableFill() => fill = false;
        public void DisableFill() => fill = true;
        public void SetFill(bool down) => fill = down;

        public void SetPenColor(Color color) => pen.Color = color;
        public void SetPenWidth(float width) => pen.Width = width;

        public void SetFillColor(Color color) => ((SolidBrush) brush).Color = color;

        public void DrawLine(Point point) {
            graphics.DrawLine(pen, penPosition, point);
            penPosition = point;
        }

        public void DrawLines(Point[] points) {
            if (points.Length < 2) {
                throw new Exception("Cannot have less than two points in a line");
            }

            GraphicsPath path = new GraphicsPath();

            for (int i = 1; i < points.Length; i++) {
                path.AddLine(points[i - 1], points[i]);
            }

            path.AddLine(points[0], points.Last());

            DrawLines(path);

        }

        //Doesn't move pen
        public void DrawLines(GraphicsPath path) {
            path.Widen(pen);
            if(fill) graphics.FillPath(brush, path);
            graphics.DrawPath(pen, path);
        }

    }

}