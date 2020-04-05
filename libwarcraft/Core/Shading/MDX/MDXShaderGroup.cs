//
//  MDXShaderGroup.cs
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

namespace Warcraft.Core.Shading.MDX
{
    /// <summary>
    /// Used as a grouping class for an internal shader lookup table.
    /// </summary>
    internal class MDXShaderGroup
    {
        /// <summary>
        /// Gets or sets the vertex shader algorithm.
        /// </summary>
        public MDXVertexShaderType VertexShader { get; set; }

        /// <summary>
        /// Gets or sets the control shader algorithm.
        /// </summary>
        public MDXControlShaderType ControlShader { get; set; }

        /// <summary>
        /// Gets or sets the evaluation shader algorithm.
        /// </summary>
        public MDXEvaluationShaderType EvaluationShader { get; set; }

        /// <summary>
        /// Gets or sets the fragment shader algorithm.
        /// </summary>
        public MDXFragmentShaderType FragmentShader { get; set; }

        /// <summary>
        /// Gets or sets the colour operations.
        /// </summary>
        public uint ColourOperations { get; set; }

        /// <summary>
        /// Gets or sets the alpha operations.
        /// </summary>
        public uint AlphaOperations { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MDXShaderGroup"/> class.
        /// </summary>
        /// <param name="frag">The fragment shader algorithm.</param>
        /// <param name="vertex">The vertex shader algorithm.</param>
        /// <param name="control">The control shader algorithm.</param>
        /// <param name="eval">The evaluation shader algorithm.</param>
        /// <param name="colourOp">The colour operations.</param>
        /// <param name="alphaOp">The alpha operations.</param>
        public MDXShaderGroup
        (
            MDXFragmentShaderType frag,
            MDXVertexShaderType vertex,
            MDXControlShaderType control,
            MDXEvaluationShaderType eval,
            uint colourOp,
            uint alphaOp
        )
        {
            VertexShader = vertex;
            ControlShader = control;
            EvaluationShader = eval;
            FragmentShader = frag;
            ColourOperations = colourOp;
            AlphaOperations = alphaOp;
        }
    }
}
