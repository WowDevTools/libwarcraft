using System;

namespace Warcraft.Core.Reflection.DBC
{
	[AttributeUsage(AttributeTargets.Property)]
	public class RecordFieldArrayAttribute : RecordFieldAttribute
	{
		public uint Count { get; set; }

		public RecordFieldArrayAttribute(WarcraftVersion introducedIn)
			: base(introducedIn)
		{
		}
	}
}
