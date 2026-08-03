// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2026 MindPort GmbH

using System.Collections.Generic;

namespace VRBuilder.Core.EntityOwners
{
    /// <summary>
    /// A generic version of <see cref="IEntityCollectionData"/>
    /// </summary>
    public interface IEntityCollectionData<out TEntity> : IEntityCollectionData where TEntity : IEntity
    {
        /// <summary>
        /// Returns the children of this entity.
        /// </summary>
        new IEnumerable<TEntity> GetChildren();
    }

    /// <summary>
    /// An entity's data which represents a collection of other entities.
    /// </summary>
    public interface IEntityCollectionData : IData
    {
        /// <summary>
        /// Returns the children of this entity as generic entities.
        /// </summary>
        IEnumerable<IEntity> GetChildren();
    }
}