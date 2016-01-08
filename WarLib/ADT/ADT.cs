using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;

using WarLib.Exceptions;
using WarLib.Core;

namespace WarLib
{
	namespace ADT
	{
		/// <summary>
		/// A complete ADT object created from a file on disk.
		/// </summary>
		public class ADT
		{
			/// <summary>
			/// Contains the ADT name.
			/// </summary>
			internal string ADTName;

			/// <summary>
			/// Contains the ADT version.
			/// </summary>
			internal ADT.MVER ADTVersion;

			/// <summary>
			/// Contains the ADT Header with offsets. The header has offsets to the other chunks in the ADT.
			/// </summary>
			internal ADT.MHDR ADTHeader;

			/// <summary>
			/// Contains an array of offsets where MCNKs are in the file.
			/// </summary>
			internal ADT.MCIN ADTMCNKOffsets;

			/// <summary>
			/// Contains a list of all textures referenced by this ADT.
			/// </summary>
			internal ADT.MTEX ADTTextures;

			/// <summary>
			/// Contains a list of all M2 models refereced by this ADT.
			/// </summary>
			internal ADT.MMDX ADTModels;

			/// <summary>
			/// Contains M2 model indexes for the list in ADTModels (MMDX chunk).
			/// </summary>
			internal ADT.MMID ADTModelIndexes;

			/// <summary>
			/// Contains a list of all WMOs referenced by this ADT.
			/// </summary>
			internal ADT.MWMO ADTWMOs;

			/// <summary>
			/// Contains WMO indexes for the list in ADTWMOs (MWMO chunk).
			/// </summary>
			internal ADT.MWID ADTWMOIndexes;

			/// <summary>
			/// Contains position information for all M2 models in this ADT.
			/// </summary>
			internal ADT.MDDF ADTModelPlacementInfo;

			/// <summary>
			/// Contains position information for all WMO models in this ADT.
			/// </summary>
			internal ADT.MODF ADTWMOPlacementInfo;

			/// <summary>
			/// Contains water data for this ADT. This chunk is present in WOTLK chunks and above.
			/// </summary>
			internal ADT.MH2O ADTWOTLKWater;


			/// <summary>
			/// Contains an array of all MCNKs in this ADT.
			/// </summary>
			internal List<MCNK> ADTMCNKs;

			/// <summary>
			/// Creates a new ADT object from a file on disk
			/// </summary>
			/// <param name="filePath">Path to an .adt file on disk</param>
			/// <returns>A parsed ADT file with objects for all chunks</returns>
			public ADT(string filePath)
			{
				//Is the file an ADT file?
				if (filePath.EndsWith(".adt"))
				{                    
					Console.WriteLine("Found ADT, parsing...");

					ADTName = Path.GetFileNameWithoutExtension(filePath);

					Stream ADTStream = File.OpenRead(filePath);
					BinaryReader br = new BinaryReader(ADTStream);

					//initialize the MCNK list
					ADTMCNKs = new List<MCNK>();

					//get the size of the entire binary ADT
					int ADTSize = (int)br.BaseStream.Length;

					//create a buffer for finding chunks inside the ADT
					byte[] chunkRecognitionBuffer = new byte[4];

					while (br.BaseStream.Position < ADTSize)
					{
						chunkRecognitionBuffer[3] = br.ReadByte();
						chunkRecognitionBuffer[2] = br.ReadByte();
						chunkRecognitionBuffer[1] = br.ReadByte();
						chunkRecognitionBuffer[0] = br.ReadByte();

						switch (Encoding.ASCII.GetString(chunkRecognitionBuffer))
						{
							case "MVER":
								{
									Console.WriteLine("Found MVER Chunk, parsing...");

									this.ADTVersion = new MVER(filePath, (int)br.BaseStream.Position);
									br.BaseStream.Position += this.ADTVersion.size;
									continue;
								}
							case "MHDR":
								{                                    
									Console.WriteLine("Found MHDR Chunk, parsing...");

									this.ADTHeader = new MHDR(filePath, (int)br.BaseStream.Position);
									br.BaseStream.Position += this.ADTHeader.size;
									continue;
								}
							case "MCIN":
								{
									Console.WriteLine("Found MCIN Chunk, parsing...");

									this.ADTMCNKOffsets = new MCIN(filePath, (int)br.BaseStream.Position);
									br.BaseStream.Position += this.ADTMCNKOffsets.size;
									continue;
								}
							case "MTEX":
								{
									Console.WriteLine("Found MTEX Chunk, parsing...");

									this.ADTTextures = new MTEX(filePath, (int)br.BaseStream.Position);
									br.BaseStream.Position += this.ADTTextures.size;
									continue;
								}
							case "MMDX":
								{
									Console.WriteLine("Found MMDX Chunk, parsing...");

									this.ADTModels = new MMDX(filePath, (int)br.BaseStream.Position);
									br.BaseStream.Position += this.ADTModels.size;
									continue;
								}
							case "MMID":
								{
									Console.WriteLine("Found MMID Chunk, parsing...");

									this.ADTModelIndexes = new MMID(filePath, (int)br.BaseStream.Position);
									br.BaseStream.Position += this.ADTModelIndexes.size;
									continue;
								}
							case "MWMO":
								{
									Console.WriteLine("Found MWMO Chunk, parsing...");

									this.ADTWMOs = new MWMO(filePath, (int)br.BaseStream.Position);
									br.BaseStream.Position += this.ADTWMOs.size;
									continue;
								}
							case "MWID":
								{
									Console.WriteLine("Found MWID Chunk, parsing...");

									this.ADTWMOIndexes = new MWID(filePath, (int)br.BaseStream.Position);
									br.BaseStream.Position += this.ADTWMOIndexes.size;
									continue;
								}
							case "MDDF":
								{
									Console.WriteLine("Found MDDF Chunk, parsing...");

									this.ADTModelPlacementInfo = new MDDF(filePath, (int)br.BaseStream.Position);
									br.BaseStream.Position += this.ADTModelPlacementInfo.size;
									continue;
								}
							case "MODF":
								{
									Console.WriteLine("Found MODF Chunk, parsing...");

									this.ADTWMOPlacementInfo = new MODF(filePath, (int)br.BaseStream.Position);
									br.BaseStream.Position += this.ADTWMOPlacementInfo.size;
									continue;
								}
							case "MH2O":
								{
									Console.WriteLine("Found MH2O Chunk, parsing...");

									//read the size of the MH2O chunk
									int skip = br.ReadInt32();

									Console.WriteLine("Forcibly skipping MH2O...");

									Console.WriteLine(String.Format("Position before: {0}", br.BaseStream.Position.ToString()));

									br.BaseStream.Position += skip;

									Console.WriteLine(String.Format("Position after: {0}", br.BaseStream.Position.ToString()));

									continue;
								}
							case "MCNK":
								{
									MCNK mcnk = new MCNK(filePath, (int)br.BaseStream.Position);
									ADTMCNKs.Add(mcnk);
									string count = String.Format("Current MCNK count: {0}", ADTMCNKs.Count.ToString());

									Console.WriteLine("Found MCNK Chunk, parsing...");

									br.BaseStream.Position += mcnk.Header.size;
									continue;
								}
							case "MFBO":
								{
									Console.WriteLine("Found MFBO Chunk, parsing...");
									continue;
								}
							case "MTXF":
								{
									Console.WriteLine("Found MTXF Chunk, parsing...");
									continue;
								}
						}
					}
				}
				else
				{
					throw new UnsupportedFileException(filePath);
				}

				Console.WriteLine(String.Format("Finished loading ADT with version {0}.", this.ADTVersion.version.ToString()));
			}

			/// <summary>
			/// MVER Chunk - Contains the ADT version
			/// </summary>
			public struct MVER
			{
				/// <summary>
				/// Size of the MVER chunk
				/// </summary>
				public int size;

				/// <summary>
				/// ADT version from MVER
				/// </summary>
				public int version;

				/// <summary>
				/// Creates a new MVER object from a file path and offset into the file
				/// </summary>
				/// <param name="adtFile">Path to the file on disk</param>                
				/// <param name="position">Offset into the file where the MVER chunk begins</param>
				/// <returns>An MVER object containing the ADT version</returns>
				public MVER(string adtFile, int position)
				{
					Stream adtStream = File.OpenRead(adtFile);
					BinaryReader br = new BinaryReader(adtStream);
					br.BaseStream.Position = position;

					this.size = br.ReadInt32();
					this.version = br.ReadInt32();

					br.Close();
					adtStream.Close();
				}
			}

			/// <summary>
			/// MHDR Chunk - Contains offset for all major chunks in the ADT. All offsets are from the start of the MHDR + 4 bytes to compensate for the size field.
			/// </summary>
			public struct MHDR
			{
				/// <summary>
				/// Flags for the ADT.
				/// </summary>
				[Flags]
				public enum MHDRFlags
				{
					/// <summary>
					/// Flag if the ADT contains an MFBO chunk
					/// </summary>
					MHDR_MFBO = 1,

					/// <summary>
					/// Flag if the ADT is from Northrend. This flag is not always set.
					/// </summary>
					MHDR_Northrend = 2,
				}

				/// <summary>
				/// Size of the MHDR chunk
				/// </summary>
				public int size;

				/// <summary>
				/// Flags for this ADT
				/// </summary>
				public MHDRFlags flags;

				/// <summary>
				/// Offset into the file where the MCIN Chunk can be found. 
				/// </summary>
				public int MCINOffset;
				/// <summary>
				/// Offset into the file where the MTEX Chunk can be found. 
				/// </summary>
				public int MTEXOffset;

				/// <summary>
				/// Offset into the file where the MMDX Chunk can be found. 
				/// </summary>
				public int MMDXOffset;
				/// <summary>
				/// Offset into the file where the MMID Chunk can be found. 
				/// </summary>
				public int MMIDOffset;

				/// <summary>
				/// Offset into the file where the MWMO Chunk can be found. 
				/// </summary>
				public int MWMOOffset;
				/// <summary>
				/// Offset into the file where the MWID Chunk can be found. 
				/// </summary>
				public int MWIDOffset;

				/// <summary>
				/// Offset into the file where the MMDF Chunk can be found. 
				/// </summary>
				public int MDDFOffset;
				/// <summary>
				/// Offset into the file where the MODF Chunk can be found. 
				/// </summary>
				public int MODFOffset;

				/// <summary>
				/// Offset into the file where the MFBO Chunk can be found. This is only set if the Flags contains MDHR_MFBO.
				/// </summary>
				public int MFBOOffset;

				/// <summary>
				/// Offset into the file where the MH2O Chunk can be found. 
				/// </summary>
				public int MH2OOffset;
				/// <summary>
				/// Offset into the file where the MTXF Chunk can be found. 
				/// </summary>
				public int MTXFOffset;

				/// <summary>
				/// Undefined int 1 - its use is currently unknown. It is commonly set to 0.
				/// </summary>
				public int unknown1;
				/// <summary>
				/// Undefined int 2 - its use is currently unknown. It is commonly set to 0.
				/// </summary>
				public int unknown2;
				/// <summary>
				/// Undefined int 3 - its use is currently unknown. It is commonly set to 0.
				/// </summary>
				public int unknown3;
				/// <summary>
				/// Undefined int 4 - its use is currently unknown. It is commonly set to 0.
				/// </summary>
				public int unknown4;

				/// <summary>
				/// Creates a new MHDR object from a file path and offset into the file
				/// </summary>
				/// <param name="adtFile">Path to the file on disk</param>                
				/// <param name="position">Offset into the file where the MHDR chunk begins</param>
				/// <returns>An MHDR object containing the offsets for all major chunks</returns>
				public MHDR(string adtFile, int position)
				{
					Stream adtStream = File.OpenRead(adtFile);
					BinaryReader br = new BinaryReader(adtStream);
					br.BaseStream.Position = position;

					//read size
					this.size = br.ReadInt32();

					//read values
					this.flags = (MHDRFlags)br.ReadInt32();

					this.MCINOffset = br.ReadInt32();
					this.MTEXOffset = br.ReadInt32();

					this.MMDXOffset = br.ReadInt32();
					this.MMIDOffset = br.ReadInt32();

					this.MWMOOffset = br.ReadInt32();
					this.MWIDOffset = br.ReadInt32();

					this.MDDFOffset = br.ReadInt32();
					this.MODFOffset = br.ReadInt32();

					bool bMFBOExists = (flags & MHDRFlags.MHDR_MFBO) == MHDRFlags.MHDR_MFBO;
					if (bMFBOExists)
					{
						this.MFBOOffset = br.ReadInt32();
					}
					else
					{
						//0 means it's disabled
						this.MFBOOffset = 0;
					}

					this.MH2OOffset = br.ReadInt32();
					this.MTXFOffset = br.ReadInt32();

					this.unknown1 = br.ReadInt32();
					this.unknown2 = br.ReadInt32();
					this.unknown3 = br.ReadInt32();
					this.unknown4 = br.ReadInt32();

					br.Close();
					adtStream.Close();
				}
			}

			/// <summary>
			/// MCIN Chunk - Contains a list of all MCNKs with associated information in the ADT file.
			/// </summary>
			public struct MCIN
			{
				/// <summary>
				/// Size of the MCIN chunk
				/// </summary>
				public int size;

				/// <summary>
				/// A struct containing information about the referenced MCNK
				/// </summary>
				public struct MCINEntry
				{
					/// <summary>
					/// Absolute offset of the MCNK
					/// </summary>
					public int MCNKOffset;
					/// <summary>
					/// Size of the MCNK
					/// </summary>
					public int size;
					/// <summary>
					/// Flags of the MCNK. This is only set on the client, and is as such always 0.
					/// </summary>
					public int flags;
					/// <summary>
					/// Async loading ID of the MCNK. This is only set on the client, and is as such always 0.
					/// </summary>
					public int asyncID;
				}

				/// <summary>
				/// An array of 256 MCIN entries, containing MCNK offsets and sizes.
				/// </summary>
				public List<MCINEntry> entries;

				/// <summary>
				/// Creates a new MCIN object from a file path and offset into the file
				/// </summary>
				/// <param name="adtFile">Path to the file on disk</param>                
				/// <param name="position">Offset into the file where the MCIN chunk begins</param>
				/// <returns>An MCIN object containing an array with information about all MCNK chunks</returns>
				public MCIN(string adtFile, int position)
				{
					Stream adtStream = File.OpenRead(adtFile);
					BinaryReader br = new BinaryReader(adtStream);
					br.BaseStream.Position = position;

					//read size, n of entries is size / 16
					this.size = br.ReadInt32();                    
					int nEntries = size / 16;
					entries = new List<MCINEntry>();

					for (int i = 0; i < nEntries; i++)
					{
						MCINEntry entry = new MCINEntry();

						entry.MCNKOffset = br.ReadInt32();
						entry.size = br.ReadInt32();
						entry.flags = br.ReadInt32();
						entry.asyncID = br.ReadInt32();

						entries.Add(entry);
					}
					br.Close();
					adtStream.Close();
				}
			}

			/// <summary>
			/// MTEX Chunk - Contains a list of all referenced textures in this ADT.
			/// </summary>
			public struct MTEX
			{
				/// <summary>
				/// Size of the MTEX chunk.
				/// </summary>
				public int size;

				/// <summary>
				///A list of full paths to the textures referenced in this ADT.
				/// </summary>
				public List<string> fileNames;

				/// <summary>
				/// Creates a new MTEX object from a file path and offset into the file
				/// </summary>
				/// <param name="adtFile">Path to the file on disk</param>                
				/// <param name="position">Offset into the file where the MTEX chunk begins</param>
				/// <returns>An MTEX object containing a list of full texture paths</returns>
				public MTEX(string adtFile, int position)
				{
					Stream adtStream = File.OpenRead(adtFile);
					BinaryReader br = new BinaryReader(adtStream);
					br.BaseStream.Position = position;

					//read the MTEX size
					this.size = br.ReadInt32();

					//create an empty list
					this.fileNames = new List<string>();

					string str = "";
					//we add four, because otherwise we miss out on the last string because of the fact that the chunk identifier (MTEX) 
					//needs to be included in the calculations
					while (br.BaseStream.Position <= position + this.size + 4)
					{
						char letterChar = br.ReadChar();
						if (letterChar != Char.MinValue)
						{
							str = str + letterChar.ToString();
						}
						else
						{
							this.fileNames.Add(str);
							//clear string for a new filename
							str = "";
						}
					}

					br.Close();
					adtStream.Close();
				}

				public string GetFileName(int nameIndex, bool keepExtension)
				{
					string fileName = "";
					if (nameIndex <= this.fileNames.Count)
					{
						if (keepExtension == false)
						{
							fileName = this.fileNames[nameIndex].Substring(this.fileNames[nameIndex].LastIndexOf(@"\") + 1).Replace(".blp", "");
						}
						else
						{
							fileName = this.fileNames[nameIndex].Substring(this.fileNames[nameIndex].LastIndexOf(@"\") + 1);
						}                        
					}
					else
					{

					}   
					return fileName;
				}
			}

			/// <summary>
			/// MMDX Chunk - Contains a list of all referenced M2 models in this ADT.
			/// </summary>
			public struct MMDX
			{
				/// <summary>
				/// Size of the MMDX chunk.
				/// </summary>
				public int size;

				/// <summary>
				///A list of full paths to the M2 models referenced in this ADT.
				/// </summary>
				public List<string> fileNames;

				/// <summary>
				/// Creates a new MMDX object from a file path and offset into the file
				/// </summary>
				/// <param name="adtFile">Path to the file on disk</param>                
				/// <param name="position">Offset into the file where the MMDX chunk begins</param>
				/// <returns>An MMDX object containing a list of full M2 model paths</returns>
				public MMDX(string adtFile, int position)
				{
					Stream adtStream = File.OpenRead(adtFile);
					BinaryReader br = new BinaryReader(adtStream);
					br.BaseStream.Position = position;

					//read the MMDX size
					this.size = br.ReadInt32();

					//create an empty list
					this.fileNames = new List<string>();

					string str = "";
					while (br.BaseStream.Position < position + this.size)
					{
						char letterChar = br.ReadChar();
						if (letterChar != Char.MinValue)
						{
							str = str + letterChar.ToString();
						}
						else
						{
							this.fileNames.Add(str);
							//clear string for a new filename
							str = "";
						}
					}

					br.Close();
					adtStream.Close();
				}
			}

			/// <summary>
			/// MMID Chunk - Contains a list of M2 model indexes
			/// </summary>
			public struct MMID
			{
				/// <summary>
				/// Size of the MMID chunk.
				/// </summary>
				public int size;

				/// <summary>
				/// List indexes for models in an MMID chunk
				/// </summary>
				public List<int> ModelIndex;

				public MMID(string adtFile, int position)
				{
					Stream adtStream = File.OpenRead(adtFile);
					BinaryReader br = new BinaryReader(adtStream);
					br.BaseStream.Position = position;

					//read size
					this.size = br.ReadInt32();

					//create new empty list
					this.ModelIndex = new List<int>();

					int i = 0;
					while (i > this.size)
					{
						this.ModelIndex.Add(br.ReadInt32());
						i += 4;
					}
					br.Close();
					adtStream.Close();
				}
			}

			/// <summary>
			/// MWMO Chunk - Contains a list of all referenced WMO models in this ADT.
			/// </summary>
			public struct MWMO
			{
				/// <summary>
				/// Size of the MWMO chunk.
				/// </summary>
				public int size;

				/// <summary>
				///A list of full paths to the M2 models referenced in this ADT.
				/// </summary>
				public List<string> fileNames;

				public MWMO(string adtFile, int position)
				{
					Stream adtStream = File.OpenRead(adtFile);
					BinaryReader br = new BinaryReader(adtStream);
					br.BaseStream.Position = position;

					//read the MWMO size
					this.size = br.ReadInt32();

					//create an empty list
					this.fileNames = new List<string>();

					string str = "";
					int i = 0;

					while (i < +this.size)
					{
						char letterChar = br.ReadChar();
						if (letterChar != Char.MinValue)
						{
							str = str + letterChar.ToString();
						}
						else
						{
							this.fileNames.Add(str);
							//clear string for a new filename
							str = "";
						}
						i += 1;
					}

					br.Close();
					adtStream.Close();
				}
			}

			/// <summary>
			/// MMID Chunk - Contains a list of WMO model indexes
			/// </summary>
			public struct MWID
			{
				/// <summary>
				/// Size of the MWID chunk.
				/// </summary>
				public int size;

				/// <summary>
				/// List indexes for WMO models in an MWMO chunk
				/// </summary>
				public List<int> WMOIndex;

				public MWID(string adtFile, int position)
				{
					Stream adtStream = File.OpenRead(adtFile);
					BinaryReader br = new BinaryReader(adtStream);
					br.BaseStream.Position = position;

					//read size
					this.size = br.ReadInt32();

					//create new empty list
					this.WMOIndex = new List<int>();

					int i = 0;
					while (i > this.size)
					{
						this.WMOIndex.Add(br.ReadInt32());
						i += 4;
					}
					br.Close();
					adtStream.Close();
				}
			}

			/// <summary>
			/// MMDF Chunk - Contains M2 model placement information
			/// </summary>
			public struct MDDF
			{
				/// <summary>
				/// Flags for the model
				/// </summary>
				[Flags]
				public enum MDDFFlags
				{
					/// <summary>
					/// Biodome flag
					/// </summary>
					MDDF_BioDome = 1,
					/// <summary>
					/// BRING ME A SHRUBBERY
					/// </summary>
					MMDF_Shrubbery = 2,
				}

				/// <summary>
				/// An entry struct containing information about the model
				/// </summary>
				public struct MDDFEntry
				{
					/// <summary>
					/// Specifies which model to use via the MMID chunk
					/// </summary>
					public int MMIDEntry;
					/// <summary>
					/// A unique actor ID for the model. Blizzard implements this as game global, but it's only checked in loaded tiles
					/// </summary>
					public int uniqueID;

					/// <summary>
					/// Position of the model
					/// </summary>
					public Vector3f position;
					/// <summary>
					/// Rotation of the model
					/// </summary>
					public Rotator rotation;

					/// <summary>
					/// Scale of the model. 1024 is the default value, equating to 1.0f. There is no uneven scaling of objects
					/// </summary>
					public int scale;
					/// <summary>
					/// MMDF flags for the object
					/// </summary>
					public MDDFFlags flags;
				}

				/// <summary>
				/// Size of the MDDF chunk
				/// </summary>
				public int size;

				/// <summary>
				/// Contains a list of MDDF entries with model placement information
				/// </summary>
				public List<MDDFEntry> entries;


				public MDDF(string adtFile, int position)
				{
					Stream adtStream = File.OpenRead(adtFile);
					BinaryReader br = new BinaryReader(adtStream);
					br.BaseStream.Position = position;

					//read size
					this.size = br.ReadInt32();

					//create a new empty list
					this.entries = new List<MDDFEntry>();

					int i = 0;
					while (i < this.size)
					{
						MDDFEntry entry = new MDDFEntry();

						entry.MMIDEntry = br.ReadInt32();
						entry.uniqueID = br.ReadInt32();

						entry.position.X = br.ReadSingle();
						entry.position.Y = br.ReadSingle();
						entry.position.Z = br.ReadSingle();

						entry.rotation.Pitch = br.ReadSingle();
						entry.rotation.Yaw = br.ReadSingle();
						entry.rotation.Roll = br.ReadSingle();

						entry.scale = br.ReadInt16();
						entry.flags = (MDDFFlags)br.ReadInt16();

						this.entries.Add(entry);

						i += Marshal.SizeOf(entry);
					}

					br.Close();
					adtStream.Close();
				}
			}

			/// <summary>
			/// MODF Chunk - Contains WMO model placement information
			/// </summary>
			public struct MODF
			{

				/// <summary>
				/// Flags for the WMO
				/// </summary>
				[Flags]
				public enum MODFFlags
				{
					/// <summary>
					/// Flag if the WMO is a destructible WMO
					/// </summary>
					MODF_Destructible = 1,
				}

				/// <summary>
				/// An entry struct containing information about the WMO
				/// </summary>
				public struct MODFEntry
				{
					/// <summary>
					/// Specifies which WHO to use via the MMID chunk
					/// </summary>
					public uint MWIDEntry;
					/// <summary>
					/// A unique actor ID for the model. Blizzard implements this as game global, but it's only checked in loaded tiles
					/// </summary>
					public uint uniqueID;

					/// <summary>
					/// Position of the WMO
					/// </summary>
					public Vector3f position;
					/// <summary>
					/// Rotation of the model
					/// </summary>
					public Rotator rotation;

					/// <summary>
					/// Lower bounds of the model
					/// </summary>
					public Vector3f lowerBounds;
					/// <summary>
					/// Upper of the model
					/// </summary>
					public Vector3f upperBounds;

					/// <summary>
					/// Flags of the model
					/// </summary>
					public MODFFlags flags;
					/// <summary>
					/// Doodadset of the model
					/// </summary>
					public int doodadSet;
					/// <summary>
					/// Nameset of the model
					/// </summary>
					public int nameSet;
					/// <summary>
					/// A bit of padding in the chunk
					/// </summary>
					public int padding;
				}

				/// <summary>
				/// Size of the MODF chunk.
				/// </summary>
				public int size;

				/// <summary>
				/// An array of WMO model information entries
				/// </summary>
				public List<MODFEntry> entries;

				public MODF(string adtFile, int position)
				{
					Stream adtStream = File.OpenRead(adtFile);
					BinaryReader br = new BinaryReader(adtStream);
					br.BaseStream.Position = position;

					//read size
					this.size = br.ReadInt32();

					//create a new empty list
					this.entries = new List<MODFEntry>();

					int i = 0;
					while (i < this.size)
					{
						MODFEntry entry = new MODFEntry();

						entry.MWIDEntry = br.ReadUInt32();
						entry.uniqueID = br.ReadUInt32();

						entry.position.X = br.ReadSingle();
						entry.position.Y = br.ReadSingle();
						entry.position.Z = br.ReadSingle();

						entry.rotation.Pitch = br.ReadSingle();
						entry.rotation.Yaw = br.ReadSingle();
						entry.rotation.Roll = br.ReadSingle();

						entry.lowerBounds.X = br.ReadSingle();
						entry.lowerBounds.Y = br.ReadSingle();
						entry.lowerBounds.Z = br.ReadSingle();

						entry.upperBounds.X = br.ReadSingle();
						entry.upperBounds.Y = br.ReadSingle();
						entry.upperBounds.Z = br.ReadSingle();

						entry.flags = (MODFFlags)br.ReadUInt16();
						entry.doodadSet = br.ReadUInt16();
						entry.nameSet = br.ReadUInt16();
						entry.padding = br.ReadUInt16();

						this.entries.Add(entry);

						i += Marshal.SizeOf(entry);
					}

					br.Close();
					adtStream.Close();
				}
			}

			/// <summary>
			/// MH2O Chunk - Contains liquid information about the ADT file, superseding the older MCLQ chunk.
			/// </summary>
			public struct MH2O
			{
				//TODO
			}

			/// <summary>
			/// MCNK Chunk - Main map chunk which contains a number of smaller subchunks. 256 of these are present in an ADT file.
			/// </summary>
			public struct MCNK
			{
				/// <summary>
				/// Header contains information about the MCNK and its subchunks such as offsets, position and flags.
				/// </summary>
				public MCNKHeader Header;
				/// <summary>
				/// Chunks contains references to the different subchunks of the MCNK.
				/// </summary>
				public MCNKChunks Chunks;

				/// <summary>
				/// Flags available for this MCNK
				/// </summary>
				public enum MCNKFlags
				{
					/// <summary>
					/// Flags the MCNK as containing a static shadow map
					/// </summary>
					MCHNK_MCSH = 1,
					/// <summary>
					/// Flags the MCNK as impassible
					/// </summary>
					MCNK_Impassible = 2,
					/// <summary>
					/// Flags the MCNK as a river
					/// </summary>
					MCNK_River = 4,
					/// <summary>
					/// Flags the MCNK as an ocean
					/// </summary>
					MCNK_Ocean = 8,
					/// <summary>
					/// Flags the MCNK as magma
					/// </summary>
					MCNK_Magma = 16,
					/// <summary>
					/// Flags the MCNK as containing an MCCV chunk
					/// </summary>
					MCNK_MCCV = 32,
					/// <summary>
					/// Flags the MCNK for high-resolution holes. Introduced in WoW 5.3
					/// </summary>
					MCNK_HighResHoles = 64,
					//introduced in WoW 5.3. Shouldn't be needed.
				}

				/// <summary>
				/// The header of the MCNK
				/// </summary>
				public struct MCNKHeader
				{
					//header
					/// <summary>
					/// Size of the MCNK
					/// </summary>
					public int size;

					/// <summary>
					/// Flags for the MCNK
					/// </summary>
					public int flags;
					/// <summary>
					/// Zero-based X position of the MCNK
					/// </summary>
					public int PositionX;
					/// <summary>
					/// Zero-based Y position of the MCNK
					/// </summary>
					public int PositionY;
					/// <summary>
					/// Alpha map layers in the MCNK
					/// </summary>
					public int layers;
					/// <summary>
					/// Number of doodad references in the MCNK
					/// </summary>
					public int DoodadReferences;

					/// <summary>
					/// MCNK-based Offset of the MCVT Heightmap Chunk
					/// </summary>
					public int HeightChunkOffset;
					/// <summary>
					/// MCNK-based Offset of the MMCNR Normal map Chunk
					/// </summary>
					public int NormalChunkOffset;
					/// <summary>
					/// MCNK-based Offset of the MCLY Alpha Map Layer Chunk
					/// </summary>
					public int LayerChunkOffset;
					/// <summary>
					/// MCNK-based Offset of the MCRF Object References Chunk
					/// </summary>
					public int MapReferencesChunkOffset;

					/// <summary>
					/// MCNK-based Offset of the MCAL Alpha Map Chunk
					/// </summary>
					public int AlphaMapsChunkOffset;
					/// <summary>
					/// Size of the Alpha Map chunk
					/// </summary>
					public int sizeAlpha;

					/// <summary>
					/// MCNK-based Offset of the MCSH Static shadow Chunk. This is only set with flags MCNK_MCSH.
					/// </summary>
					public int ShadowMapChunkOffset;
					/// <summary>
					/// Size of the shadow map chunk.
					/// </summary>
					public int sizeShadow;

					/// <summary>
					/// Area ID for the MCNK.
					/// </summary>
					public int areaID;
					/// <summary>
					/// Number of object references in this MCNK.
					/// </summary>
					public int mapObjectReferences;
					/// <summary>
					/// Holes in the MCNK.
					/// </summary>
					public int holes;

					/// <summary>
					/// A low-quality texture map of the MCNK. Used with LOD.
					/// </summary>
					public UInt16[] LowQTextureMap;

					/// <summary>
					/// It is not yet known what predTex does.
					/// </summary>
					public int predTex;
					/// <summary>
					/// It is not yet known what noEffectDoodad does.
					/// </summary>
					public int noEffectDoodad;

					/// <summary>
					/// MCNK-based Offset of the MCSE Sound Emitters Chunk
					/// </summary>
					public int SoundEmittersChunkOffset;
					/// <summary>
					/// Number of sound emitters in the MCNK
					/// </summary>
					public int nSoundEmitters;

					/// <summary>
					/// MCNK-based Offset of the MCLQ Liquid Chunk
					/// </summary>
					public int LiquidChunkOffset;
					/// <summary>
					/// Size of the liquid chunk. This is 8 when not used - if it is 8, use the newer MH2O chunk.
					/// </summary>
					public int sizeLiquid;

					/// <summary>
					/// A Vector3f of the position.
					/// </summary>
					public Vector3f Position;

					/// <summary>
					/// MCNK-based Offset of the MCCV Chunk
					/// </summary>
					public int MCCVChunkOffset;
					/// <summary>
					/// MCNK-based Offset of the MCLV Chunk. Introduced in Cataclysm.
					/// </summary>
					public int MCLVChunkOffset;

					/// <summary>
					/// Unknown int 1.
					/// </summary>
					public int unknown1;
					/// <summary>
					/// Unknown int 2.
					/// </summary>
					public int unknown2;
				}

				/// <summary>
				/// Subchunks of the MCNK
				/// </summary>
				public struct MCNKChunks
				{
					/// <summary>
					/// Heightmap Chunk
					/// </summary>
					public MCVT HeightChunk;
					/// <summary>
					/// Normal map chunk
					/// </summary>
					//public MCNR NormalChunk;
					/// <summary>
					/// Alphamap Layer chunk
					/// </summary>
					public MCLY LayerChunk;
					/// <summary>
					/// Map Object References chunk
					/// </summary>
					//public MCRF MapReferencesChunk;
					/// <summary>
					/// Alphamap chunk
					/// </summary>
					public MCAL AlphaChunk;
					/// <summary>
					/// Sound Emitter Chunk
					/// </summary>
					//public MCSE SoundEmitterChunk;
					/// <summary>
					/// Liquid Chunk
					/// </summary>
					//public MCLQ LiquidChunk;
					/// <summary>
					/// MCCV chunk
					/// </summary>
					//public MCCV MCCVChunk;
					/// <summary>
					/// MCLV chunk
					/// </summary>
					//public MCLV MCLVChunk;
				}


				/// <summary>
				/// Creates a new MCNK object from a file on disk and an offset into the file.
				/// </summary>
				/// <param name="adtFile">Path to the file on disk</param>                
				/// <param name="position">Offset into the file where the MCNK chunk begins</param>
				/// <returns>An MCNK object containing a header and the subchunks</returns>
				public MCNK(string adtFile, int position)
				{
					Stream adtStream = File.OpenRead(adtFile);
					BinaryReader br = new BinaryReader(adtStream);
					br.BaseStream.Position = position;
					int MCNKStart = (int)br.BaseStream.Position;

					//read size of the chunk
					this.Header.size = br.ReadInt32();



					//read the ADT flags
					this.Header.flags = br.ReadInt32();

					//read the X coordinate
					this.Header.PositionX = br.ReadInt32();

					//read the Y coordinate
					this.Header.PositionY = br.ReadInt32();

					//read the number of alpha layers
					this.Header.layers = br.ReadInt32();

					//read the number of doodad references
					this.Header.DoodadReferences = br.ReadInt32();

					//read height data offset - this is a local offset, not a global one.
					this.Header.HeightChunkOffset = br.ReadInt32();

					//read vertex normals offset - this is a local offset, not a global one.
					this.Header.NormalChunkOffset = br.ReadInt32();

					//read texture layer definitions offset - this is a local offset, not a global one.
					this.Header.LayerChunkOffset = br.ReadInt32();

					//read M2 and WMO references. This is used to determine what is drawn in every MCNK
					this.Header.MapReferencesChunkOffset = br.ReadInt32();

					//read alpha maps offset - this is a local offset, not a global one.
					this.Header.AlphaMapsChunkOffset = br.ReadInt32();
					//make sure we're at offset 0x028 from MCNK start here - the above chunks should manage that and work on the local stream
					this.Header.sizeAlpha = br.ReadInt32();

					//read static shadow maps offset - this is a local offset, not a global one.
					this.Header.ShadowMapChunkOffset = br.ReadInt32();
					//make sure we're at offset 0x030 here, same reason as above
					this.Header.sizeShadow = br.ReadInt32();

					//read the Area ID
					this.Header.areaID = br.ReadInt32();

					//read map object reference count
					this.Header.mapObjectReferences = br.ReadInt32();

					//read holes (count?)
					this.Header.holes = br.ReadInt32();

					//read LQ texture map
					UInt16[] LQTexBuilder = new UInt16[16];
					for (int i = 0; i < 16; i++)
					{
						LQTexBuilder[i] = (ushort)br.ReadInt16();
					}
					this.Header.LowQTextureMap = LQTexBuilder;

					//read predTex
					this.Header.predTex = br.ReadInt32();

					//read noEffectDoodad
					this.Header.noEffectDoodad = br.ReadInt32();

					//read sound emitters offset - this is a local offset, not a global one.
					this.Header.SoundEmittersChunkOffset = br.ReadInt32();
					//read sound emitter count - make sure MCSE puts us at offset 0x05C
					this.Header.nSoundEmitters = br.ReadInt32();

					//read liquid data offset - this is a local offset, not a global one.
					this.Header.LiquidChunkOffset = br.ReadInt32();
					//read liquid size - make sure MCLQ puts us at offset 0x064
					//if this is 8, we'll be using the new MH2O chunk instead.
					this.Header.sizeLiquid = br.ReadInt32();

					//read position
					float x = br.ReadSingle();
					float y = br.ReadSingle();
					float z = br.ReadSingle();

					this.Header.Position = new Vector3f(x, y, z);

					//read final offsets - these are local offsets, not global ones.
					this.Header.MCCVChunkOffset = br.ReadInt32();
					this.Header.MCLVChunkOffset = br.ReadInt32();

					this.Header.unknown1 = br.ReadInt32();
					this.Header.unknown2 = br.ReadInt32();


					//Fill the chunks
					this.Chunks.HeightChunk = new MCVT(adtFile, MCNKStart + this.Header.HeightChunkOffset);
					this.Chunks.LayerChunk = new MCLY(adtFile, MCNKStart + this.Header.LayerChunkOffset);
					this.Chunks.AlphaChunk = new MCAL(adtFile, MCNKStart + this.Header.AlphaMapsChunkOffset);

				}

				public MCLY.MCLYEntry[] GetTextureLayers()
				{
					int layerCount = this.Chunks.LayerChunk.Layer.Count;
					MCLY.MCLYEntry[] layers = new MCLY.MCLYEntry[layerCount];

					for (int i = 0; i < layerCount; i++)
					{
						layers[i] = this.Chunks.LayerChunk.Layer[i];
					}

					return layers;
				}
			}

			/// <summary>
			/// MCVT Chunk - Contains heightmap information
			/// </summary>
			public struct MCVT
			{
				/// <summary>
				/// An array of vertices
				/// </summary>
				public float[] vertices;

				/// <summary>
				/// Creates a new MCVT object from a file on disk and an offset into the file.
				/// </summary>
				/// <param name="adtFile">Path to the file on disk</param>                
				/// <param name="position">Offset into the file where the MCVT chunk begins</param>
				/// <returns>An MCVT object containing an array of vertices</returns>
				public MCVT(string adtFile, int position)
				{
					Stream adtStream = File.OpenRead(adtFile);
					BinaryReader br = new BinaryReader(adtStream);
					br.BaseStream.Position = position;

					vertices = new float[144];

					for (int i = 0; i < 144; i++)
					{
						vertices[i] = br.ReadSingle();
					}
				}
			}

			/// <summary>
			/// MCLV Chunk - Contains vertice colour data
			/// </summary>
			public struct MCLV
			{
				//vertex colours. Not needed.
			}

			/// <summary>
			/// MCCV Chunk - Contains vertex shading data
			/// </summary>
			public struct MCCV
			{
				//vertex shading chunk. Not needed.
			}

			/// <summary>
			/// MCNR Chunk - Contains normal vectors for shading
			/// </summary>
			public struct MCNR
			{
				//normal vectors for shading. Not needed.
			}

			/// <summary>
			/// MCLY Chunk - Contains definitions for the alpha map layers.
			/// </summary>
			public struct MCLY
			{
				public int size;

				/// <summary>
				/// Chunk flags
				/// </summary>
				[Flags]
				public enum MCLYFlags
				{
					Anim45Rot = 0x001,

					Anim90Rot = 0x002,

					Anim180Rot = 0x004,

					AnimSpeed1 = 0x008,

					AnimSpeed2 = 0x010,

					AnimSpeed3 = 0x020,

					AnimDeferred = 0x040,

					EmissiveLayer = 0x080,

					UseAlpha = 0x100,

					CompressedAlpha = 0x200,

					SkyboxReflection = 0x400,
				}

				/// <summary>
				/// An array of alpha map layers in this MCNK.
				/// </summary>
				public List<MCLYEntry> Layer;

				/// <summary>
				/// A struct defining a layer entry
				/// </summary>
				public struct MCLYEntry
				{
					/// <summary>
					/// Index of the texture in the MTEX chunk
					/// </summary>
					public int textureID;
					/// <summary>
					/// Flags for the texture. Used for animation data.
					/// </summary>
					public MCLYFlags flags;
					/// <summary>
					/// Offset into MCAL where the alpha map begins.
					/// </summary>
					public int offsetMCAL;
					/// <summary>
					/// Ground effect ID. This is actually a padded Int16.
					/// </summary>
					public int effectID;
				}

				/// <summary>
				/// Creates a new MCLY object from a file on disk and an offset into the file.
				/// </summary>
				/// <param name="adtFile">Path to the file on disk</param>                
				/// <param name="position">Offset into the file where the MCLY chunk begins</param>
				/// <returns>An MCLY object containing an array of layer entries</returns>
				public MCLY(string adtFile, int position)
				{
					Stream adtStream = File.OpenRead(adtFile);
					BinaryReader br = new BinaryReader(adtStream);
					br.BaseStream.Position = position;

					this.size = br.ReadInt32();
					int nLayers = this.size / 16;
					Layer = new List<MCLYEntry>();

					for (int i = 0; i < nLayers; i++)
					{
						MCLYEntry newEntry = new MCLYEntry();
						newEntry.textureID = br.ReadInt32();
						newEntry.flags = (MCLYFlags)br.ReadInt32();
						newEntry.offsetMCAL = br.ReadInt32();
						newEntry.effectID = br.ReadInt32();

						Layer.Add(newEntry);
					}
				}

				public MCLYEntry GetEntryForTextureID(int ID)
				{
					MCLYEntry matchingEntry = new MCLYEntry();
					bool foundEntry = false;
					//set the offset for the data that corresponds to texture i

					for (int l = 0; l < Layer.Count; l++)
					{
						if (ID == Layer.ElementAt(l).textureID)
						{
							matchingEntry = Layer.ElementAt(l);
							foundEntry = true;
						}
					}

					if (foundEntry == false)
					{
						//set all values to -1 to denote a missing or disabled chunk
						matchingEntry.effectID = -1;
						matchingEntry.flags = (MCLYFlags)0;
						matchingEntry.offsetMCAL = -1;
						matchingEntry.textureID = -1;

						return matchingEntry;
					}
					else
					{
						return matchingEntry;
					}                    
				}
			}

			/// <summary>
			/// MCRF Chunk - Contains model rendering references
			/// </summary>
			public struct MCRF
			{
				//MMDF and MODF model references. Not needed.
			}

			/// <summary>
			/// MCSH Chunk - Contains a static shadow map
			/// </summary>
			public struct MCSH
			{
				//static shadow map. Not needed.
			}

			/// <summary>
			/// MCAL Chunk - Contains alpha map data in one of three forms - uncompressed 2048, uncompressed 4096 and compressed.
			/// </summary>
			public struct MCAL
			{
				//chunk (and data) size
				public int size;

				//unformatted data contained in MCAL
				public byte[] data;

				public MCAL(string adtFile, int position)
				{
					Stream adtStream = File.OpenRead(adtFile);
					BinaryReader br = new BinaryReader(adtStream);
					br.BaseStream.Position = position;

					this.size = br.ReadInt32();

					this.data = br.ReadBytes(this.size);
				}
				//4 layers of alpha maps. This is the magic right here.
				//each layer is a 32x64 array of alpha values
				//can be formatted according to one of three types:

				/* 
                 * Uncompressed with a size of 4096 (post WOTLK)
                 * Uncompressed with a size of 2048 (pre WOTLK)
                 * Compressed - this is only for WOTLK chunks. Size is not very important when dealing with compressed alpha maps,
                 * considering you are constantly checking if you've extracted 4096 bytes of data. Here's how you do it, according to the wiki:
                 * 
                 * Read a byte.
                 * Check for a sign bit.
                 * If it's set, we're in fill mode. If not, we're in copy mode.
                 * 
                 * 1000000 = set sign bit, fill mode
                 * 0000000 = unset sign bit, copy mode
                 * 
                 * 0            1 0 1 0 1 0 1
                 * sign bit,    7 lesser bits
                 * 
                 * Take the 7 lesser bits of the first byte as a count indicator,
                 * If we're in fill mode, read the next byte and fill it by count in your resulting alpha map
                 * If we're in copy mode, read the next count bytes and copy them to your resulting alpha map
                 * If the alpha map is complete (4096 bytes), we're finished. If not, go back and start at 1 again.
                 */
			}

			/// <summary>
			/// MCLQ Chunk - Contains liquid data.
			/// </summary>
			public struct MCLQ
			{
				//water level values. Not needed.
			}

			/// <summary>
			/// MCLV Chunk - Contains sound emitter data.
			/// </summary>
			public struct MCSE
			{
				//sound emitters. Not needed.
			}

			/// <summary>
			/// MCLV Chunk - Contains bounding box data
			/// </summary>
			public struct MFBO
			{
				//bounding box for flying. Not needed.
			}

			/// <summary>
			/// MCLV Chunk - Contains additional texture rendering data
			/// </summary>
			public struct MTXF
			{
				//integer array of textures in MTEX. If the array is set to 1, the texture needs to be rendered differently. Not needed.
			}

			public Bitmap[] GetAlphaMaps()
			{
				//initialize n alpha maps, one for each texture.
				int textureCount = this.GetTextureCount();
				Bitmap[] maps = new Bitmap[textureCount];

				//create a new map object in each array element
				for (int i = 0; i < textureCount; i++)
				{
					maps[i] = new Bitmap(1024, 1024, PixelFormat.Format32bppArgb);
				}

				//loop over each MCNK
				//X axis
				for (int y = 0; y < 16; y++)
				{
					//y axis
					for (int x = 0; x < 16; x++)
					{                        
						MCNK currentChunk = GetMCNK(x, y);
						MCLY.MCLYEntry[] currentLayers = currentChunk.GetTextureLayers();
						MCAL currentAlphaChunk = currentChunk.Chunks.AlphaChunk;

                        
						//for each layer in the MCNK, read the texture ID.
						//then, write the data into the matching texture map.
						for (int i = 0; i < currentLayers.Length; i++)
						{
							MCLY.MCLYEntry currentLayer = currentLayers[i];

							if (currentAlphaChunk.size % 2048 != 0)
							{
								Console.WriteLine("MCAL was not divisible by 2048? Size was: " + currentAlphaChunk.size.ToString());
							}
							//does it use an alpha map?
							if (!currentLayer.flags.HasFlag(MCLY.MCLYFlags.UseAlpha))
							{

								//first layer is always rendered with full opacity.
								//thus, we need to read all maps above it and then calculate what it would look like.

								//read and store all alpha maps above this layer (1 to 3)
								//add their values pixel by pixel to a fourth map

								//subtract this fourth map from a fully opaque bitmap, creating a map of what would be visible

								//as an example, the maps above contain pixels with the values 40, 20 and 100, forming a map value of 160.
								//we subtract this from the fully opaque map, returning a final value of 255 - 160 = 95


								//create a new alpha section
								Bitmap alphaSection = new Bitmap(64, 64, PixelFormat.Format32bppArgb);

								Console.WriteLine("DEBUG: Layer " + i.ToString()
									+ " (" + GetTextureByID(currentLayer.textureID).ToString()
									+ ") does not use alpha mapping, according to the flags.");

                                
								//this is our texture ID.
								int textureID = currentLayer.textureID;

								using (Graphics g = Graphics.FromImage(maps[textureID]))
								{
									using (Graphics g2 = Graphics.FromImage(alphaSection))
									{
										if (currentLayers.Length != 1)
										{
											g2.Clear(Color.FromArgb(255, 255, 255, 255));

											for (int l = 1; l < currentLayers.Length; l++)
											{
												//we're using a separate currentLayer variable here for more clarity. The principle is the same.
												MCLY.MCLYEntry readingLayer = currentLayers[l];

												//buffer to store the alpha map
												byte[] buffer = new byte[2048];
												for (int p = 0; p < 2048; p++)
												{
													buffer[p] = currentAlphaChunk.data[readingLayer.offsetMCAL + p];
												}

												Bitmap layerMap = Read2048AlphaMap(buffer, true);

												//loop through each pixel in the layerMap and subtract it from the alphaSection
												for (int map_y = 0; map_y < 64; map_y++)
												{
													for (int map_x = 0; map_x < 64; map_x++)
													{
														Color layerMapPixel = layerMap.GetPixel(map_x, map_y);
														Color alphaSectionPixel = alphaSection.GetPixel(map_x, map_y);

														//subtract the above layer colour from the alphaSection colour
														//also clamp values to range 0 - 255


														int alpha = ExtensionMethods.Clamp<int>(alphaSectionPixel.A - layerMapPixel.A, 0, 255);
														int red = ExtensionMethods.Clamp<int>(alphaSectionPixel.R - layerMapPixel.R, 0, 255);
														int green = ExtensionMethods.Clamp<int>(alphaSectionPixel.G - layerMapPixel.G, 0, 255);
														int blue = ExtensionMethods.Clamp<int>(alphaSectionPixel.B - layerMapPixel.B, 0, 255);

														Color newPixel = Color.FromArgb
                                                            (
															                 alpha,
															                 red,
															                 green,
															                 blue
														                 );

														//set the new colour
														alphaSection.SetPixel(map_x, map_y, newPixel);
													}
												}
											}

											g.DrawImage(alphaSection, x * 64, y * 64);
										}
										else
										{
											g2.Clear(Color.Black);
										}
									}
								}                                
                                        
								Console.WriteLine("MCNK at " + x.ToString() + "," + y.ToString() + " :" + GetTextureByID(textureID));
							}
							else
							{
								//create a new alpha section
								Bitmap alphaSection = new Bitmap(64, 64, PixelFormat.Format32bppArgb);

								//this is our ID.
								int textureID = currentLayer.textureID;

                                

								//read the map (2048 or 4096 bytes) if there is a map
								if (!(currentAlphaChunk.size == 0))
								{
									if (currentAlphaChunk.size % 2048 != 0 && !currentLayer.flags.HasFlag(MCLY.MCLYFlags.CompressedAlpha))
									{
										//buffer to store the alpha map
										byte[] buffer = new byte[4096];
										Console.WriteLine("Alpha was uncompressed 4096-map?");
										for (int p = 0; p < 4096; p++)
										{
											buffer[p] = currentAlphaChunk.data[currentLayer.offsetMCAL + p];
										}

										//set the section to the map
										alphaSection = Read4096AlphaMap(buffer);
									}
									else
									{
										//buffer to store the alpha map
										byte[] buffer = new byte[2048];
										for (int p = 0; p < 2048; p++)
										{
											buffer[p] = currentAlphaChunk.data[currentLayer.offsetMCAL + p];
										}

										//set the section to the map
										alphaSection = Read2048AlphaMap(buffer, true);
									}
                                    

									//draw it on the larger map
									using (Graphics g = Graphics.FromImage(maps[textureID]))
									{
										g.DrawImage(alphaSection, x * 64, y * 64);
									}

								}
								else //make it transparent
								{
									using (Graphics g = Graphics.FromImage(maps[textureID]))
									{
										using (Graphics g2 = Graphics.FromImage(alphaSection))
										{
											g2.Clear(Color.Transparent);
											g.DrawImage(alphaSection, x * 64, y * 64);
										}
									}
								}

								Console.WriteLine("MCNK at " + x.ToString() + "," + y.ToString() + " :" + GetTextureByID(textureID));
							}
                                                                    
                            
						}
                        
					}
				}

				//return the created maps
				return maps;
			}

			private Bitmap Read2048AlphaMap(byte[] buffer, bool repair)
			{
				Console.WriteLine("Reading 2048 uncompressed alphamap.");

				Bitmap alphaSection = new Bitmap(64, 64, PixelFormat.Format32bppArgb);
				
				for (int y = 0; y < 64; y++)
				{
					for (int x = 0; x < 32; x++)
					{
						//check if it has a northrend flag, or if there's a manual repair override
						if (this.GetFlags().HasFlag(MHDR.MHDRFlags.MHDR_Northrend) || repair)
						{
							//the ADT was from Northrend, but had a 2048 mapformat.
							if (y == 63)
							{
								int yminus = y - 1;

								//attempt to repair map on horizontal axis

								byte pixel1 = 0;
								byte pixel2 = 0;

								pixel1 = Convert.ToByte(buffer[x + yminus * 32] & 0xf0);
								pixel2 = Convert.ToByte(buffer[x + 1 + yminus * 32] << 4 & 0xf0);

								byte pixel1_map = (byte)Map(pixel1, 0, 240, 0, 255);
								byte pixel2_map = (byte)Map(pixel2, 0, 240, 0, 255);
								byte alpha1 = (byte)Map(pixel1, 0, 240, 0, 255);
								byte alpha2 = (byte)Map(pixel2, 0, 240, 0, 255);

								alphaSection.SetPixel(x * 2 + 1, y, Color.FromArgb(alpha1, 255 - pixel1_map, 255 - pixel1_map, 255 - pixel1_map));
								alphaSection.SetPixel(x * 2, y, Color.FromArgb(alpha2, 255 - pixel2_map, 255 - pixel2_map, 255 - pixel2_map));

							}
							else if (x == 31)
							{
								//attempt to repair map on vertical axis
								byte pixel = Convert.ToByte(buffer[x + y * 32] << 4 & 0xf0);

								byte pixel_map = (byte)Map(pixel, 0, 240, 0, 255);
								byte alpha = (byte)Map(pixel, 0, 240, 0, 255);

								alphaSection.SetPixel(x * 2 + 1, y, Color.FromArgb(alpha, 255 - pixel_map, 255 - pixel_map, 255 - pixel_map));
								alphaSection.SetPixel(x * 2, y, Color.FromArgb(alpha, 255 - pixel_map, 255 - pixel_map, 255 - pixel_map));
							}
							else
							{
								//fill in normally

								byte pixel1 = 0;
								byte pixel2 = 0;

								pixel1 = Convert.ToByte(buffer[x + y * 32] & 0xf0);
								pixel2 = Convert.ToByte(buffer[x + y * 32] << 4 & 0xf0);

								byte pixel1_map = (byte)Map(pixel1, 0, 240, 0, 255);
								byte pixel2_map = (byte)Map(pixel2, 0, 240, 0, 255);
								byte alpha1 = (byte)Map(pixel1, 0, 240, 0, 255);
								byte alpha2 = (byte)Map(pixel2, 0, 240, 0, 255);

								alphaSection.SetPixel(x * 2 + 1, y, Color.FromArgb(alpha1, 255 - pixel1_map, 255 - pixel1_map, 255 - pixel1_map));
								alphaSection.SetPixel(x * 2, y, Color.FromArgb(alpha2, 255 - pixel2_map, 255 - pixel2_map, 255 - pixel2_map));
							}
						}
						else
						{
							//standard Vanilla or Outland map.
							//fill in normally

							byte pixel1 = 0;
							byte pixel2 = 0;

							pixel1 = Convert.ToByte(buffer[x + y * 32] & 0xf0);
							pixel2 = Convert.ToByte(buffer[x + y * 32] << 4 & 0xf0);

							byte pixel1_map = (byte)Map(pixel1, 0, 240, 0, 255);
							byte pixel2_map = (byte)Map(pixel2, 0, 240, 0, 255);
							byte alpha1 = (byte)Map(pixel1, 0, 240, 0, 255);
							byte alpha2 = (byte)Map(pixel2, 0, 240, 0, 255);

							alphaSection.SetPixel(x * 2 + 1, y, Color.FromArgb(alpha1, 255 - pixel1_map, 255 - pixel1_map, 255 - pixel1_map));
							alphaSection.SetPixel(x * 2, y, Color.FromArgb(alpha2, 255 - pixel2_map, 255 - pixel2_map, 255 - pixel2_map));
						}                        
					}
				}
				return alphaSection;
			}

			private Bitmap Read4096AlphaMap(byte[] buffer)
			{
				Bitmap alphaSection = new Bitmap(64, 64, PixelFormat.Format32bppArgb);

				byte pixel = 0;

				for (int y = 0; y < 64; y++)
				{
					for (int x = 0; x < 64; x++)
					{
						pixel = Convert.ToByte(buffer[x + y * 64]);

						byte pixel_map = (byte)Map(pixel, 0, 240, 0, 255);
						byte alpha = (byte)Map(pixel, 0, 240, 0, 255);

						alphaSection.SetPixel(x, y, Color.FromArgb(alpha, 255 - pixel_map, 255 - pixel_map, 255 - pixel_map));
					}
				}
				return alphaSection;
			}

			public string GetName()
			{
				return ADTName;
			}

			public Version GetADTVersion()
			{
				Version version = new Version(ADTVersion.version.ToString());
				return version;
			}

			public MHDR GetHeader()
			{
				return ADTHeader;
			}

			/// <summary>
			/// Get a MCNK by zero-based coordinate.
			/// </summary>
			public MCNK GetMCNK(int x, int y)
			{
				int index = (y * 16) + x;

				MCNK chunk = ADTMCNKs[index];
				return chunk;
			}

			public int GetTextureCount()
			{
				return this.ADTTextures.fileNames.Count;
			}

			/// <summary>
			/// Get a texture by zero-based index. NOT WORKING PROPERLY; FIX
			/// </summary>
			public string GetTextureByID(int ID)
			{
				//is the ID valid (i.e, between 0 and the filename count)
				if ((ID < this.ADTTextures.fileNames.Count) && (ID > -1))
				{
					return this.ADTTextures.fileNames.ElementAt(ID);
				}
				else
				{
					return null;
				}                
			}

			public string[] GetTextures()
			{
				return this.ADTTextures.fileNames.ToArray();
			}

			public ADT.MHDR.MHDRFlags GetFlags()
			{
				return this.ADTHeader.flags;
			}

			private Bitmap InvertBitmap(Bitmap bitmapImage)
			{
				byte A, R, G, B;
				Color pixelColor;

				for (int y = 0; y < bitmapImage.Height; y++)
				{
					for (int x = 0; x < bitmapImage.Width; x++)
					{
						pixelColor = bitmapImage.GetPixel(x, y);
						A = (byte)(255 - pixelColor.A);
						R = pixelColor.R;
						G = pixelColor.G;
						B = pixelColor.B;
						bitmapImage.SetPixel(x, y, Color.FromArgb((int)A, (int)R, (int)G, (int)B));
					}
				}
				return bitmapImage;
			}

			private int Map(int x, int in_min, int in_max, int out_min, int out_max)
			{
				return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
			}
		}
	}
}
