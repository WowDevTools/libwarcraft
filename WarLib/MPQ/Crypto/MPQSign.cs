using System;

namespace Warcraft.MPQ.Crypto
{
	// TODO: Implement
	static class MPQSign
	{
		static MPQSign()
		{

		}

		public static void SignArchive(MPQ archive, Strength signingStrength)
		{

		}

		private static void InternalSignWeak(MPQ archive)
		{

		}

		private static void InteralSignString(MPQ archive)
		{

		}

		public static bool VerifyArchiveIntegrity(MPQ archive)
		{
			return true;
		}
	}

	public enum Strength : byte
	{
		Weak,
		Strong
	}
}

