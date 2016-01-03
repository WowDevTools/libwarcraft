using System;
using System.IO;

namespace WarLib.Exceptions
{
	/// <summary>
	/// An exception thrown when the input file for an ADT construct did not end in the .adt extension
	/// </summary>
	public class UnsupportedFileException : Exception
	{
		public UnsupportedFileException (string fileName)
		{
			Console.WriteLine (String.Format ("UnsupportedFileException: Selected file did not end in an ADT extension: {0}", Path.GetFileName (fileName)));
		}
	}
}

