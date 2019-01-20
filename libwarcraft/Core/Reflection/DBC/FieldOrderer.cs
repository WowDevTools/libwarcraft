//
//  FieldOrderer.cs
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
using System.Linq;
using System.Reflection;
using Warcraft.Core.Extensions;

namespace Warcraft.Core.Reflection.DBC
{
    /// <summary>
    /// Reorders fields in a record.
    /// </summary>
    public class FieldOrderer
    {
        /// <summary>
        /// Gets the Warcraft version that the orderer is for.
        /// </summary>
        public WarcraftVersion Version { get; }

        /// <summary>
        /// Gets the properties in their original order.
        /// </summary>
        public IReadOnlyList<PropertyInfo> OriginalProperties { get; }

        /// <summary>
        /// Gets the properties that will move.
        /// </summary>
        public IReadOnlyDictionary<PropertyInfo, RecordFieldOrderAttribute> MovingProperties { get; }

        /// <summary>
        /// Gets the precedence chains.
        /// </summary>
        public Dictionary<PropertyInfo, List<PropertyInfo>> PrecedenceChains { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldOrderer"/> class.
        /// </summary>
        /// <param name="version">The version that the orderer will be for.</param>
        /// <param name="originalProperties">The properties in their original order.</param>
        public FieldOrderer(WarcraftVersion version, IReadOnlyList<PropertyInfo> originalProperties)
        {
            Version = version;
            OriginalProperties = originalProperties;

            // Get the properties that should move, and their movement information
            MovingProperties = DBCInspector.GetMovedProperties(Version, originalProperties);

            // Build the precedence chains for the moving properties
            PrecedenceChains = BuildPrecedenceChains();
        }

        /// <summary>
        /// Reorders the properties.
        /// </summary>
        /// <returns>The properties, in their new order.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a cyclic move dependency is detected.</exception>
        public IEnumerable<PropertyInfo> ReorderProperties()
        {
            var originalProperties = new List<PropertyInfo>(OriginalProperties);

            // Check if any property exists in the precedence chain of any of its precedents
            if (MovingProperties.Any(p => HasCyclicMoveDependency(p.Key)))
            {
                throw new InvalidOperationException(
                    "A cyclical dependency was detected in the field move set. Verify that no fields are requesting invalid moves.");
            }

            // First, we'll order the original properties by declaration and inheritance
            var orderedProperties = originalProperties
                .OrderBy(p => p.DeclaringType, new InheritanceChainComparer())
                .ThenBy(p => p.MetadataToken)
                .ToList();

            // Then, we'll move the fields which need special handling. Order them by the length of their dependency
            // chains such that we are working our way down the chains.
            var movingPropertiesByDepth = PrecedenceChains.OrderBy(kvp => kvp.Value.Count);
            foreach (var propertyPair in movingPropertiesByDepth)
            {
                var property = propertyPair.Key;
                var propertyThatComesBeforeName = MovingProperties[property].ComesAfter;
                var propertyBefore = orderedProperties.First(p => p.Name == propertyThatComesBeforeName);

                // Find the original property in the list and remove it
                orderedProperties.Remove(property);

                // Then, find the preceding property
                int precedingPropertyIndex = orderedProperties.IndexOf(propertyBefore);

                // Finally, insert the original property after that one
                orderedProperties.Insert(precedingPropertyIndex + 1, property);
            }

            return orderedProperties;
        }

        /// <summary>
        /// Build precedence chains for all moving properties.
        /// </summary>
        /// <returns>The precedence chains.</returns>
        public Dictionary<PropertyInfo, List<PropertyInfo>> BuildPrecedenceChains()
        {
            // Build field precendence chains
            var precedenceChains = new Dictionary<PropertyInfo, List<PropertyInfo>>();
            foreach ((var property, var order) in MovingProperties)
            {
                var precedenceChain = GetPrecendenceChain(order, new List<PropertyInfo>()).ToList();
                precedenceChains.Add(property, precedenceChain);
            }

            return precedenceChains;
        }

        /// <summary>
        /// Checks if the given property depends on itself as a preceding property somewhere along the precedence chain.
        /// </summary>
        /// <param name="property">The property to check.</param>
        /// <returns>true if the property has a cyclic dependency; otherwise, false.</returns>
        public bool HasCyclicMoveDependency(PropertyInfo property)
        {
            if (!PrecedenceChains.ContainsKey(property))
            {
                return false;
            }

            var precedenceChain = PrecedenceChains[property];
            return precedenceChain.Contains(property);
        }

        /// <summary>
        /// Gets the chain of preceding properties for the given property.
        /// </summary>
        /// <param name="order">The order attribute.</param>
        /// <param name="yieldedProperties">The properties that have already been yielded. Used to break out if a cyclical dependency
        /// is encountered. This list should always be empty at the first non-recursive call.
        /// </param>
        /// <returns>The chain of preceding properties that will move, terminating with the first one that will not.</returns>
        public IEnumerable<PropertyInfo> GetPrecendenceChain
        (
            RecordFieldOrderAttribute order,
            List<PropertyInfo> yieldedProperties
        )
        {
            var precedingProperty = OriginalProperties.First(p => p.Name == order.ComesAfter);

            bool willPropertyMove = MovingProperties.ContainsKey(precedingProperty);
            if (willPropertyMove && !yieldedProperties.Contains(precedingProperty))
            {
                yieldedProperties.Add(precedingProperty);
                foreach (var value in GetPrecendenceChain(MovingProperties[precedingProperty], yieldedProperties))
                {
                    yieldedProperties.Add(value);
                    yield return value;
                }
            }

            yield return precedingProperty;
        }
    }
}
