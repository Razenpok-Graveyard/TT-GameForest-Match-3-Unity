using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

    class Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }

        public Point Sum(Point other)
        {
            return new Point(X + other.X, Y + other.Y);
        }
    }
