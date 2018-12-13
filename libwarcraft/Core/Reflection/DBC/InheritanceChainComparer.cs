//
//  StringReference.cs
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
using System.Collections.Generic;

namespace Warcraft.Core.Reflection.DBC
{
    /// <inheritdoc />
    /// <summary>
    /// Compares types by their inheritance chain. A subclass is considered more (that is, more derived) than a parent
    /// class.
    /// </summary>
    public class InheritanceChainComparer : IComparer<Type>
    {
        /// <inheritdoc />
        public int Compare(Type x, Type y)
        {
            if (x.IsSubclassOf(y))
            {
                return 1;
            }

            if (y.IsSubclassOf(x))
            {
                return -1;
            }

            return 0;
        }
    }
}
