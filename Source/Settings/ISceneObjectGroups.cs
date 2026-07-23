using System;
using System.Collections.Generic;

namespace VRBuilder.Core.Settings
{
    public interface ISceneObjectGroups
    {
        string GetLabel(Guid guid);
        bool GroupExists(Guid guid);
        bool ContainsAny(IEnumerable<Guid> guids);
        bool CanCreateGroup(string newGroup);
        event Action Changed;
    }
}