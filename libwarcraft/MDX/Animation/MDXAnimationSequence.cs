//
//  MDXAnimationSequence.cs
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

using System.IO;
using Warcraft.Core;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;
using Warcraft.Core.Structures;
using Warcraft.DBC.Definitions;

namespace Warcraft.MDX.Animation
{
    /// <summary>
    /// An animation definition for an <see cref="MDX"/> model.
    /// </summary>
    public class MDXAnimationSequence : IVersionedClass
    {
        /// <summary>
        /// The animation ID, pointing to additional data in <see cref="AnimationDataRecord"/>.
        /// </summary>
        public uint AnimationID;

        /// <summary>
        /// The start timestamp of this animation. This field was rendered obsolete in versions > Burning Crusade.
        /// </summary>
        public uint StartTimestamp;

        /// <summary>
        /// The end timestamp of this animation. This field was rendered obsolete in versions > Burning Crusade.
        /// </summary>
        public uint EndTimestamp;

        /// <summary>
        /// The duration (in milliseconds) of this animation. Only present after Burning Crusade.
        /// </summary>
        public uint Duration;

        /// <summary>
        /// The speed at which the model can move during the animation.
        /// </summary>
        public float MovementSpeed;

        /// <summary>
        /// A set of modifying flags.
        /// </summary>
        public MDXAnimationSequenceFlags Flags;

        /// <summary>
        /// The probability that the animation will be played. For all animations of the same type, this will add up to
        /// 0x7FFF (32767).
        /// </summary>
        public short Probability;

        /// <summary>
        /// A bit of padding, or an unknown field.
        /// </summary>
        public ushort Padding;

        /// <summary>
        /// A range from which the client will pick a number of repetitions, once the animation is played.
        /// Both the upper and lower values may be 0 to denote no repetitions.
        /// </summary>
        public IntegerRange ReplayRange;

        /// <summary>
        /// The time taken to blend between this animation and the next.
        /// </summary>
        public uint BlendTime;

        /// <summary>
        /// A bounding box, which encompasses the maximum size the animation may deform the model to.
        /// </summary>
        public Box BoundingBox;

        /// <summary>
        /// A bounding radius which (when used with the center of <see cref="BoundingBox"/>) forms a
        /// bounding sphere which encompasses the maximum size the animation may deform the model to.
        /// </summary>
        public float BoundingSphereRadius;

        /// <summary>
        /// The ID of the following variant of this animation which is to be played, or -1 for none.
        /// </summary>
        public short NextVariation;

        /// <summary>
        /// The ID of the following aninmation. This field is used if this animation is an alias
        /// (that is, <see cref="Flags"/> has <see cref="MDXAnimationSequenceFlags.IsAliasedAndHasFollowupAnimation"/>
        /// set.
        /// </summary>
        public ushort NextAliasedAnimationID;

        /// <summary>
        /// Deserializes an <see cref="MDXAnimationSequence"/> from a given data stream.
        /// </summary>
        /// <param name="br"></param>
        /// <param name="version"></param>
        public MDXAnimationSequence(BinaryReader br, WarcraftVersion version)
        {
            this.AnimationID = br.ReadUInt32();

            if (version <= WarcraftVersion.BurningCrusade)
            {
                this.StartTimestamp = br.ReadUInt32();
                this.EndTimestamp = br.ReadUInt32();
            }
            else
            {
                this.Duration = br.ReadUInt32();
            }

            this.MovementSpeed = br.ReadSingle();
            this.Flags = (MDXAnimationSequenceFlags)br.ReadUInt32();
            this.Probability = br.ReadInt16();
            this.Padding = br.ReadUInt16();
            this.ReplayRange = br.ReadIntegerRange();

            this.BlendTime = br.ReadUInt32();
            this.BoundingBox = br.ReadBox();
            this.BoundingSphereRadius = br.ReadSingle();
            this.NextVariation = br.ReadInt16();
            this.NextAliasedAnimationID = br.ReadUInt16();
        }
    }
}

