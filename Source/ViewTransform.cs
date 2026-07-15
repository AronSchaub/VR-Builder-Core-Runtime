using System.Runtime.Serialization;
using VRBuilder.Core.Primitives;

namespace VRBuilder.Core
{
    /// <summary>
    /// Stores position and scale in a viewport.
    /// </summary>
    [DataContract(IsReference = true)]
    public class ViewTransform
    {
        [DataMember]
        public IVector3 Position { get; set; }

        [DataMember]
        public IVector3 Scale { get; set; }

        public ViewTransform(IVector3 position, IVector3 scale)
        {
            Position = position;
            Scale = scale;
        }
    }
}
