﻿using Advanced_Software_Engineering.Verbs.Flow;
using Advanced_Software_Engineering.Verbs.Value;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Advanced_Software_Engineering {

    /// <summary>
    /// The drawer is responsible for drawing on the graphics object.
    /// </summary>
    public class Drawer {

        /// <summary>
        /// Value storage is used for storing all of the variables and their IValues. See <see cref="ValueStorage"/> and see <see cref="IValue"/>
        /// </summary>
        private readonly ValueStorage valueStorage = new ValueStorage();

        /// <summary>
        /// List of verbChunks used to create for loops, if statements, while loops and methods.
        /// See <see cref="ForChunk"/>, <see cref="IfChunk"/>, <see cref="MethodChunk"/>, <see cref="WhileChunk"/> and <seealso cref="IVerbChunk"/>.
        /// </summary>
        public List<IVerbChunk> verbChunks = new List<IVerbChunk>();

        /// <summary>
        /// A veriable that represents wether the last verbchunk is a method. See <see cref="MethodChunk"/>
        /// </summary>
        public bool verbChunkGeneratingMethod = false;

        /// <summary>
        /// A list of methods. See <see cref="MethodChunk"/>
        /// </summary>
        public Dictionary<string, MethodChunk> Methods = new Dictionary<string, MethodChunk>();

        /// <summary>
        /// The starting color of the pen / brush
        /// </summary>
        protected static Color defaultColor = Color.Black;

        /// <summary>
        /// The deafult width of the pen
        /// </summary>
        protected static float defaultWidth = 1f;

        /// <summary>
        /// Weather enclosed spaces will have fill.
        /// </summary>
        protected bool fill = false;

        /// <summary>
        /// The Brush object
        /// </summary>
        protected SolidBrush brush;

        /// <summary>
        /// The Pen object
        /// </summary>
        protected Pen pen;

        /// <summary>
        /// The current position of the pen.
        /// </summary>
        protected Point penPosition;

        /// <summary>
        /// The graphics object
        /// </summary>
        protected Graphics graphics;

        /// <summary>
        /// Creates a new drawer object.
        /// </summary>
        /// <param name="graphics">Graphics context</param>
        public Drawer(Graphics graphics) {
            this.graphics = graphics;
            ResetDrawer();
        }

        /// <summary>
        /// Restart the drawer.
        /// </summary>
        public void ResetDrawer() {
            graphics.Clear(Color.White);
            penPosition = new Point(0, 0);
            pen = new Pen(defaultColor, defaultWidth);
            brush = new SolidBrush(defaultColor);
            fill = false;
            valueStorage.Reset();
            verbChunks = new List<IVerbChunk>();
            Methods = new Dictionary<string, MethodChunk>();
        }

        /// <summary>
        /// Set thhe graphics context.
        /// </summary>
        /// <param name="graphics"></param>
        public void SetGraphics(Graphics graphics) => this.graphics = graphics;

        /// <summary>
        /// Move the pen to the point.
        /// </summary>
        /// <param name="point">Position to move the pen to</param>
        public void MovePen(Point point) => penPosition = point;

        /// <summary>
        /// Returns the current pen position
        /// </summary>
        /// <returns>The curren pen position</returns>
        public Point GetPenPosition() => new Point(penPosition.X, penPosition.Y);

        /// <summary>
        /// Get current fill status
        /// </summary>
        /// <returns>current fill satus</returns>
        public bool GetFill() => fill;

        /// <summary>
        /// Allows the filling of objects.
        /// </summary>
        public void EnableFill() => fill = true;

        /// <summary>
        /// Disables the filling of objects.
        /// </summary>
        public void DisableFill() => fill = false;

        /// <summary>
        /// Set the filling of objects
        /// </summary>
        /// <param name="fill">Set fill</param>
        public void SetFill(bool fill) => this.fill = fill;

        /// <summary>
        /// Set the pen color
        /// </summary>
        /// <param name="color">Pen color</param>
        public void SetPenColor(Color color) => pen.Color = color;

        /// <summary>
        /// Set the width of the pen
        /// </summary>
        /// <param name="width">Pen width</param>
        public void SetPenWidth(float width) => pen.Width = width;

        /// <summary>
        /// Set fill color
        /// </summary>
        /// <param name="color">Fill color</param>
        public void SetFillColor(Color color) => brush.Color = color;

        /// <summary>
        /// Draws a line from the current pen position to the one specified in the parameter. This also sets the pens new position.
        /// </summary>
        /// <param name="point">Draw to point</param>
        public void DrawLine(Point point) {
            graphics.DrawLine(pen, penPosition, point);
            penPosition = point;
        }

        /// <summary>
        /// Draws multiple lines to multiple points. This doesn't change the pens position.
        /// </summary>
        /// <param name="points">Points to draw to</param>
        public void DrawLines(Point[] points) {
            if (points.Length < 2) {
                throw new Exception("Cannot have less than two points in a line");
            }

            //Make it relative to the pen
            for (int pointIndex = 0; pointIndex < points.Length; pointIndex++)
                points[pointIndex] = new Point(points[pointIndex].X + penPosition.X, points[pointIndex].Y + penPosition.Y);

            GraphicsPath path = new GraphicsPath {
                FillMode = FillMode.Winding
            };

            path.AddLines(points);
            path.AddLine(points[0], points.Last()); //Add final line

            DrawLines(path);
        }

        /// <summary>
        /// Draws multiple lines to multiple points. This doesn't change the pens position.
        /// </summary>
        /// <param name="path">GraphicsPath to draw to</param>
        public void DrawLines(GraphicsPath path) {
            //path.Widen(pen);
            if (fill) graphics.FillPath(brush, path);
            graphics.DrawPath(pen, path);
        }

        /// <summary>
        /// Draws a circle of a scale ath the pen's position
        /// </summary>
        /// <param name="scale">Scale of the circle</param>
        public void DrawCircle(double scale) {
            if (fill) graphics.FillEllipse(brush, penPosition.X - (float)(scale), penPosition.Y - (float)(scale), (float)scale * 2, (float)scale * 2);
            graphics.DrawEllipse(pen, penPosition.X - (float)(scale), penPosition.Y - (float)(scale), (float)scale * 2, (float)scale * 2);
        }

        /// <summary>
        /// Clears the canvas
        /// </summary>
        public void ClearCanvas() {
            graphics.Clear(Color.White);
        }

        /// <summary>
        /// Draws a single pixel with the pen's color at the pen's position.
        /// </summary>
        public void DrawDot() {
            graphics.FillRectangle(new SolidBrush(pen.Color), penPosition.X, penPosition.Y, 1, 1);
        }

        /// <summary>
        /// Gets the valueStorage object
        /// </summary>
        /// <returns></returns>
        public ValueStorage GetValueStorage() {
            return valueStorage;
        }
    }
}