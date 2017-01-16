//
//  IPostLoad.cs
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

namespace Warcraft.Core.Interfaces
{
	/// <summary>
	/// This interface holds that the implementing class can load all of its serialized data after it has been
	/// constructed through the <see cref="PostLoad"/> method.
	/// </summary>
	/// <typeparam name="T">
	/// The loading parameters that the class takes. This is a class or structure which contains all required data
	/// to load the object.
	/// </typeparam>
	public interface IPostLoad<in T>
	{
		/// <summary>
		/// Determines whether or not this object has finished loading.
		/// </summary>
		/// <returns><value>true</value> if the object has finished loading; otherwise, <value>false</value>.</returns>
		bool HasFinishedLoading();

		/// <summary>
		/// Loads the object's data after construction, using the provided parameters. Typically, this is a byte
		/// array with serialized data.
		/// </summary>
		/// <param name="loadingParameters">The parameters required to fully load the object.</param>
		void PostLoad(T loadingParameters);
	}
}