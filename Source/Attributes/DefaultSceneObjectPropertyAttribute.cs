using System;

namespace VRBuilder.Core.Attributes
{
    /// <summary>
    /// An attribute used to specify a default concrete type for a specific scene object property interface.
    /// </summary>
    /// <remarks>
    /// The attribute is used to determine which concrete type should be used as the default when the automatic scene object setup,
    /// also known as the fix-it button, is activated. An example use case is if you inherit from an existing scene object property
    /// such as a grabbable property to create a grabbable with additional functionality. For example, a pencil.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public class DefaultSceneObjectPropertyAttribute : Attribute
    {
        public DefaultSceneObjectPropertyAttribute(Type concreteType)
        {
            ConcreteType = concreteType;
        }

        public Type ConcreteType { get; }
    }
}