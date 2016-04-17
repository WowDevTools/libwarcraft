//
//  ADT.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2016 Jarl Gullberg
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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Warcraft.ADT.Chunks;
using Warcraft.ADT.Chunks.Subchunks;
using Warcraft.Core;

namespace Warcraft.ADT
{
	namespace ADT
	{
		/// <summary>
		/// A complete ADT object created from a file on disk.
		/// </summary>
		public class ADT
		{
			/// <summary>
			/// Contains the ADT version.
			/// </summary>
			public TerrainVersion Version;

			/// <summary>
			/// Contains the ADT Header with offsets. The header has offsets to the other chunks in the ADT.
			/// </summary>
			public TerrainHeader Header;

			/// <summary>
			/// Contains an array of offsets where MCNKs are in the file.
			/// </summary>
			public TerrainMapChunkOffsets MapChunkOffsets;

			/// <summary>
			/// Contains a list of all textures referenced by this ADT.
			/// </summary>
			public TerrainTextures Textures;

			/// <summary>
			/// Contains a list of all M2 models refereced by this ADT.
			/// </summary>
			public TerrainModels Models;

			/// <summary>
			/// Contains M2 model indexes for the list in ADTModels (MMDX chunk).
			/// </summary>
			public TerrainModelIndices ModelIndices;

			/// <summary>
			/// Contains a list of all WMOs referenced by this ADT.
			/// </summary>
			public TerrainWorldModelObjects WorldModelObjects;

			/// <summary>
			/// Contains WMO indexes for the list in ADTWMOs (MWMO chunk).
			/// </summary>
			public TerrainWorldObjectModelIndices WorldModelObjectIndices;

			/// <summary>
			/// Contains position information for all M2 models in this ADT.
			/// </summary>
			public TerrainModelPlacementInfo ModelPlacementInfo;

			/// <summary>
			/// Contains position information for all WMO models in this ADT.
			/// </summary>
			public TerrainWorldModelObjectPlacementInfo WorldModelObjectPlacementInfo;

			/// <summary>
			/// Contains water data for this ADT. This chunk is present in WOTLK chunks and above.
			/// </summary>
			public TerrainWater Water;

			/// <summary>
			/// Contains an array of all MCNKs in this ADT.
			/// </summary>
			public List<TerrainMapChunk> MapChunks = new List<TerrainMapChunk>();

			// TODO: Change to stream-based loading
			/// <summary>
			/// Creates a new ADT object from a file on disk
			/// </summary>
			/// <param name="Data">Byte array containing ADT data.</param>
			/// <returns>A parsed ADT file with objects for all chunks</returns>
			public ADT(byte[] Data)
			{
				using (MemoryStream ms = new MemoryStream(Data))
				{
					using (BinaryReader br = new BinaryReader(ms))
					{
						while (br.BaseStream.Position != br.BaseStream.Length)
						{
							ReadChunk(br);
						}
					}
				}


				/*
				//Is the file an ADT file?
				if (Data.EndsWith(".adt"))
				{                    
					Console.WriteLine("Found ADT, parsing...");

					Stream ADTStream = File.OpenRead(Data);
					BinaryReader br = new BinaryReader(ADTStream);

					//initialize the MCNK list
					MapChunks = new List<TerrainMapChunk>();

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

									this.Version = new TerrainVersion(Data, (int)br.BaseStream.Position);
									br.BaseStream.Position += this.Version.size;
									continue;
								}
							case "MHDR":
								{                                    
									Console.WriteLine("Found MHDR Chunk, parsing...");

									this.Header = new TerrainHeader(Data, (int)br.BaseStream.Position);
									br.BaseStream.Position += this.Header.size;
									continue;
								}
							case "MCIN":
								{
									Console.WriteLine("Found MCIN Chunk, parsing...");

									this.MapChunkOffsets = new TerrainMapChunkOffsets(Data, (int)br.BaseStream.Position);
									br.BaseStream.Position += this.MapChunkOffsets.size;
									continue;
								}
							case "MTEX":
								{
									Console.WriteLine("Found MTEX Chunk, parsing...");

									this.Textures = new TerrainTextures(Data, (int)br.BaseStream.Position);
									br.BaseStream.Position += this.Textures.size;
									continue;
								}
							case "MMDX":
								{
									Console.WriteLine("Found MMDX Chunk, parsing...");

									this.Models = new TerrainModels(Data, (int)br.BaseStream.Position);
									br.BaseStream.Position += this.Models.size;
									continue;
								}
							case "MMID":
								{
									Console.WriteLine("Found MMID Chunk, parsing...");

									this.ModelIndices = new TerrainModelIndices(Data, (int)br.BaseStream.Position);
									br.BaseStream.Position += this.ModelIndices.size;
									continue;
								}
							case "MWMO":
								{
									Console.WriteLine("Found MWMO Chunk, parsing...");

									this.WorldModelObjects = new TerrainWorldModelObjects(Data, (int)br.BaseStream.Position);
									br.BaseStream.Position += this.WorldModelObjects.size;
									continue;
								}
							case "MWID":
								{
									Console.WriteLine("Found MWID Chunk, parsing...");

									this.WorldModelObjectIndices = new TerrainWorldObjectModelIndices(Data, (int)br.BaseStream.Position);
									br.BaseStream.Position += this.WorldModelObjectIndices.size;
									continue;
								}
							case "MDDF":
								{
									Console.WriteLine("Found MDDF Chunk, parsing...");

									this.ModelPlacementInfo = new TerrainModelPlacementInfo(Data, (int)br.BaseStream.Position);
									br.BaseStream.Position += this.ModelPlacementInfo.size;
									continue;
								}
							case "MODF":
								{
									Console.WriteLine("Found MODF Chunk, parsing...");

									this.WorldModelObjectPlacementInfo = new TerrainWorldModelObjectPlacementInfo(Data, (int)br.BaseStream.Position);
									br.BaseStream.Position += this.WorldModelObjectPlacementInfo.size;
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
									TerrainMapChunk mcnk = new TerrainMapChunk(Data, (int)br.BaseStream.Position);
									MapChunks.Add(mcnk);

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
					throw new FileLoadException("The provided file was not in an ADT format.", Data);
				}
				*/

				Console.WriteLine(String.Format("Finished loading ADT with version {0}.", this.Version.Version.ToString()));
			}

			private void ReadChunk(BinaryReader br)
			{
				string Signature = new string(br.ReadChars(4));
				uint ChunkSize = br.ReadUInt32();

				switch (Signature)
				{
					case TerrainVersion.Signature:
						{
							this.Version = new TerrainVersion(br.ReadBytes((int)ChunkSize));
							break;
						}
					case TerrainHeader.Signature:
						{
							break;
						}
					default:
						{
							throw new FileLoadException("An unknown chunk with the signature \"" + Signature + "\" was encountered in the terrain file.");
						}
				}
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
						TerrainMapChunk currentChunk = GetMCNK(x, y);
						MapChunkAlphaMapDefinitions.MCLYEntry[] currentLayers = currentChunk.GetTextureLayers();
						MapChunkAlphaMaps currentAlphaChunk = currentChunk.Chunks.AlphaChunk;

                        
						//for each layer in the MCNK, read the texture ID.
						//then, write the data into the matching texture map.
						for (int i = 0; i < currentLayers.Length; i++)
						{
							MapChunkAlphaMapDefinitions.MCLYEntry currentLayer = currentLayers[i];

							if (currentAlphaChunk.size % 2048 != 0)
							{
								Console.WriteLine("MCAL was not divisible by 2048? Size was: " + currentAlphaChunk.size.ToString());
							}
							//does it use an alpha map?
							if (!currentLayer.flags.HasFlag(MapChunkAlphaMapDefinitions.MCLYFlags.UseAlpha))
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
												MapChunkAlphaMapDefinitions.MCLYEntry readingLayer = currentLayers[l];

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

														int alpha = (alphaSectionPixel.A - layerMapPixel.A).Clamp(0, 255);
														int red = (alphaSectionPixel.R - layerMapPixel.R).Clamp(0, 255);
														int green = (alphaSectionPixel.G - layerMapPixel.G).Clamp(0, 255);
														int blue = (alphaSectionPixel.B - layerMapPixel.B).Clamp(0, 255);

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
                                        
								Console.WriteLine("MCNK at " + x + "," + y + " :" + GetTextureByID(textureID));
							}
							else
							{
								//create a new alpha section
								Bitmap alphaSection = new Bitmap(64, 64, PixelFormat.Format32bppArgb);

								//this is our ID.
								int textureID = currentLayer.textureID;

                                

								//read the map (2048 or 4096 bytes) if there is a map
								if (currentAlphaChunk.size != 0)
								{
									if (currentAlphaChunk.size % 2048 != 0 && !currentLayer.flags.HasFlag(MapChunkAlphaMapDefinitions.MCLYFlags.CompressedAlpha))
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
						if (this.GetFlags().HasFlag(TerrainHeaderFlags.MHDR_Northrend) || repair)
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

			public Version GetADTVersion()
			{
				Version version = new Version(Version.Version.ToString());
				return version;
			}

			public TerrainHeader GetHeader()
			{
				return Header;
			}

			/// <summary>
			/// Get a MCNK by zero-based coordinate.
			/// </summary>
			public TerrainMapChunk GetMCNK(int x, int y)
			{
				int index = (y * 16) + x;

				TerrainMapChunk chunk = MapChunks[index];
				return chunk;
			}

			public int GetTextureCount()
			{
				return this.Textures.fileNames.Count;
			}

			/// <summary>
			/// Get a texture by zero-based index. NOT WORKING PROPERLY; FIX
			/// </summary>
			public string GetTextureByID(int ID)
			{
				//is the ID valid (i.e, between 0 and the filename count)
				if ((ID < this.Textures.fileNames.Count) && (ID > -1))
				{
					return this.Textures.fileNames.ElementAt(ID);
				}
				else
				{
					return null;
				}                
			}

			public string[] GetTextures()
			{
				return this.Textures.fileNames.ToArray();
			}

			public TerrainHeaderFlags GetFlags()
			{
				return this.Header.Flags;
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
