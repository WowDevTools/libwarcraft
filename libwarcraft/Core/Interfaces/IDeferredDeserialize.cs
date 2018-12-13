using System.IO;

namespace Warcraft.Core.Interfaces
{
    /// <summary>
    /// Classes implementing this interface support
    /// </summary>
    public interface IDeferredDeserialize
    {
        /// <summary>
        /// Deserializes the data of the object using the provided <see cref="BinaryReader"/>.
        /// </summary>
        /// <param name="reader"></param>
        void DeserializeSelf(BinaryReader reader);
    }
}
