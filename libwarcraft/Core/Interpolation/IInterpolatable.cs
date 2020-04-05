//
//  IInterpolatable.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
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

namespace Warcraft.Core.Interpolation
{
    /// <summary>
    /// Specifies that the type which implements this interface can be interpolated between itself and a target point.
    /// </summary>
    /// <typeparam name="T">The type to interpolate.</typeparam>
    public interface IInterpolatable<T>
    {
        /// <summary>
        /// Interpolates the instance between itself and the <paramref name="target"/> object by an alpha factor,
        /// using the interpolation algorithm specified in <paramref name="interpolationType"/>.
        /// </summary>
        /// <param name="target">The target point.</param>
        /// <param name="alpha">The alpha factor.</param>
        /// <param name="interpolationType">The interpolation algorithm to use.</param>
        /// <returns>An interpolated object.</returns>
        T Interpolate(T target, float alpha, InterpolationType interpolationType);
    }
}
