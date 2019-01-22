//
//  Interpolation.cs
//
//  Copyright (c) 2018 Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;

namespace Warcraft.Core.Interpolation
{
    /// <summary>
    /// Holds various interpolation algorithms.
    /// </summary>
    public static class Interpolation
    {
        /// <summary>
        /// Interpolates linearly between <paramref name="a"/> and <paramref name="b"/> by <paramref name="alpha"/>.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        /// <param name="alpha">The distance to interpolate.</param>
        /// <returns>The interpolated value.</returns>
        public static float InterpolateLinear(float a, float b, float alpha)
        {
            return a + (alpha * (b - a));
        }

        /// <summary>
        /// Interpolates using the Hermite algorithm between <paramref name="a"/> and <paramref name="b"/> by
        /// <paramref name="alpha"/>, using the in/out tangents for a and b <paramref name="tangentA"/> and
        /// <paramref name="tangentB"/>, respectively.
        ///
        /// This implementation was taken from <a href="http://blog.demofox.org/2015/08/08/cubic-hermite-interpolation/"/>.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="tangentA">The in/out tangent of a.</param>
        /// <param name="b">The second point.</param>
        /// <param name="tangentB">The in/out tangent of b.</param>
        /// <param name="alpha">The distance to interpolate.</param>
        /// <returns>The interpolated value.</returns>
        public static float InterpolateHermite(float a, float tangentA, float b, float tangentB, float alpha)
        {
            var inter1 = (-a / 2.0f) + ((3.0f * tangentA) / 2.0f) - ((3.0f * b) / 2.0f) + (tangentB / 2.0f);
            var inter2 = a - ((5.0f * tangentA) / 2.0f) + (2.0f * b) - (tangentB / 2.0f);
            var inter3 = (-a / 2.0f) + (b / 2.0f);
            var inter4 = tangentB;

            return (float)((inter1 * Math.Pow(alpha, 3)) + (inter2 * Math.Pow(alpha, 2)) + (inter3 * alpha) + inter4);
        }

        /// <summary>
        /// Interpolates using the Bezier algorithm.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="tangentA">The tangent to the first point.</param>
        /// <param name="b">The second point.</param>
        /// <param name="tangentB">The tangent to the second point.</param>
        /// <param name="alpha">The alpha value.</param>
        /// <returns>The interpolated value.</returns>
        public static float InterpolateBezier(float a, float tangentA, float b, float tangentB, float alpha)
        {
            throw new NotImplementedException();
        }
    }
}
