using System;

namespace Warcraft.MPQ
{
	/// <summary>
	/// Possible flags for a block entry, designating different flags for a stored file.
	/// </summary>
	[Flags]
	public enum BlockFlags : uint
	{
		BLF_IsFile = 0x80000000,
		BLF_HasChecksums = 0x04000000,
		BLF_IsDeleted = 0x02000000,
		BLF_IsSingleUnit = 0x01000000,
		BLF_HasAdjustedEncryptionKey = 0x00020000,
		BLF_IsEncrypted = 0x00010000,
		BLF_IsCompressed = 0x00000200,
		BLF_IsImploded = 0x00000100
	}
}

