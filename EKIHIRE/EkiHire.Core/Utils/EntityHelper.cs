using EkiHire.Core.Domain.Entities;
using EkiHire.Core.Exceptions;
using EkiHire.Core.Reflection;
using System;
using System.Reflection;
using EkiHire.Core.Domain.Entities.Common;

namespace EkiHire.Core.Utils
{
    public static class EntityHelper
    {
        public static bool IsEntity(Type type)
        {
            return ReflectionHelper.IsAssignableToGenericType(type, typeof(IEntity<>));
        }

        public static Type GetPrimaryKeyType<TEntity>()
        {
            return GetPrimaryKeyType(typeof(TEntity));
        }

        /// <summary>
        /// Gets primary key type of given entity type
        /// </summary>
        public static Type GetPrimaryKeyType(Type entityType)
        {
            foreach (var interfaceType in entityType.GetTypeInfo().GetInterfaces()) {
                if (interfaceType.GetTypeInfo().IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEntity<>)) {
                    return interfaceType.GenericTypeArguments[0];
                }
            }

            throw new LMEGenericException("Can not find primary key type of given entity type: " + entityType + ". Be sure that this entity type implements IEntity<TPrimaryKey> interface");
        }
    }
}
