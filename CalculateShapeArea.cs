using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CalculateShapeArea
{
    /// <summary>
    /// An abstract class that calculates the area of shapes that can be described by a set of line segments. For example, sides of a triangle, radius of a circle, semi-axis of an ellipse, etc. 
    /// </summary>
    public abstract class Shape
    {
        /// <summary>
        /// A set of line segments that describe a shape
        /// </summary>
        protected List<double> _measurments;
        /// <summary>
        /// Sets and returns a non-editable list of line segments that describe the shape.
        /// </summary>
        /// <exception cref="ArgumentException">The line set being set does not describe the actual figure </exception>
        public ReadOnlyCollection<double> Measurments
        {
            set
            {
                if (IsValid(value.ToList()))
                {
                    _measurments = value.ToList();
                }
                else
                {
                    throw new ArgumentException("Shape measurments are invalid");
                }
            }
            get
            {
                return _measurments.AsReadOnly();
            }
        }
        /// <summary>
        /// property that returns the area of the shape
        /// </summary>
        public double Area { get { return CalculateArea(); } }
        /// <summary>
        /// An abstract method that calculates the area of a shape
        /// </summary>
        /// <returns>Figure area</returns>
        protected abstract double CalculateArea();
        /// <summary>
        /// An abstract method that checks the list of passed segments to see if they describe a real figure
        /// </summary>
        /// <param name="measurments">List of Lines to Check</param>
        /// <returns>True - the line list describes the actual figure, False - the line list does not describe the actual figure</returns>
        protected abstract bool IsValid(List<double> measurments);
    }

    /// <summary>
    ///A class that implements operations for creating and calculating the area for a circle 
    /// </summary>
    public class Circle : Shape
    {
        /// <summary>
        /// Constructor of a new Circle with the passed radius
        /// </summary>
        /// <param name="radius">Circle radius</param>
        public Circle(double radius)
        {
            Measurments = new List<double>() { radius }.AsReadOnly();
        }

        /// <summary>
        /// Checks the passed list of segments for a valid circle description: list length - 1, double.MaxValue> = length value> 0
        /// </summary>
        /// <param name="measurments">Line list containing radius </param>
        /// <returns>True -line list describes a real circle, False - line list does not describe a real circle</returns>
        protected override bool IsValid(List<double> measurments)
        {
            if (measurments.Count == 1 && measurments[0] > 0 && measurments[0] <= double.MaxValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Calculates the area of a circle from its radius
        /// </summary>
        /// <returns>Area of a circle</returns>
        protected override double CalculateArea()
        {
            return Math.PI * Measurments[0] * Measurments[0];
        }
    }

    /// <summary>
    /// A class that implements operations for creating and calculating the area for a triangle
    /// </summary>
    public class Triangle : Shape
    {
        /// <summary>
        /// Creates a new Triangle instance with the supplied sides
        /// </summary>
        /// <param name="side1">Side of triangle  A</param>
        /// <param name="side2">Side of triangle B</param>
        /// <param name="side3">Side of triangle  C</param>
        public Triangle(double side1, double side2, double side3)
        {
            Measurments = new List<double>() { side1, side2, side3 }.AsReadOnly();
        }

        /// <summary>
        /// Checks the passed list of line segments for the description of a real triangle: the length of the list is 3, double.MaxValue> = the value of the length of each side> 0,
        /// The sum of the lengths of each 2 sides is greater than the length of the third side.
        /// </summary>
        /// <param name = "measurments"> A list of segments containing the sides of the triangle </param>
        /// <returns> True - the line list describes a real triangle, False - the line list does not describe a real triangle </returns>
        protected override bool IsValid(List<double> measurments)
        {
            if (measurments.Count == 3 &&
                measurments[0] > 0 &&
                measurments[1] > 0 &&
                measurments[2] > 0 &&
                measurments[0] <= double.MaxValue &&
                measurments[1] <= double.MaxValue &&
                measurments[2] <= double.MaxValue &&
                measurments[0] + measurments[1] > measurments[2] &&
                measurments[0] + measurments[2] > measurments[1] &&
                measurments[1] + measurments[2] > measurments[0]
            )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if the triangle is rectangular.
        /// </summary>
        /// <returns> True - the triangle is right-angled, False - the triangle is not right-angled, null - it is impossible to determine the type of the triangle due to overflow double </returns>
        public bool? IsRightTriangle()
        {
            var orderedMeasurments = _measurments.OrderByDescending(m => m).ToList();

            double csqr = Math.Pow(orderedMeasurments[0], 2);
            double bsqr = Math.Pow(orderedMeasurments[1], 2);
            double asqr = Math.Pow(orderedMeasurments[2], 2);

            if (double.IsInfinity(csqr) || double.IsInfinity(bsqr + asqr))
            {
                return null;
            }

            if (csqr == bsqr + asqr)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Calculates the area of a triangle on its 3 sides
        /// </summary>
        /// <returns> The area of the triangle. Returns double.PositiveInfinity if double overflow occurs during computation </returns>
        protected override double CalculateArea()
        {
            double p = (Measurments[0] + Measurments[1] + Measurments[2]) / 2;
            return Math.Sqrt(p * (p - Measurments[0]) * (p - Measurments[1]) * (p - Measurments[2]));
        }
    }
}