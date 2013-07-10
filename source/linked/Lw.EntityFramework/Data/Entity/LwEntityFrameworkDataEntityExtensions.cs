using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Metadata.Edm;

using Lw.Data.Objects;
using System.Linq.Expressions;
using Lw.Linq.Expressions;
using Lw.Linq;
using Lw.Collections.Generic;
using Lw;
using Lw.Data.Metadata.Edm;
using Lw.Reflection;
using System.Collections;
using System.Data;
using System.Reflection;

namespace Lw.Data.Entity
{
    // http://www.dotnetframework.org/default.aspx/Net/Net/3@5@50727@3053/DEVDIV/depot/DevDiv/releases/Orcas/SP/ndp/fx/src/DataEntity/System/Data/Common/Utils/MetadataHelper@cs/4/MetadataHelper@cs
    public static class Extensions
    {
        #region IDbContext Extensions
        public static void AttachModified<TEntity>(this IDbContext reference, TEntity entity) where TEntity : class
        {
            reference.Set<TEntity>().Attach(entity);
            reference.SetEntityState(entity, EntityState.Modified);
        }

        public static void AttachModified<TEntity>(this IDbContext reference, IEnumerable<TEntity> entities) where TEntity : class
        {
            foreach (var entity in entities) { reference.AttachModified(entity); }
        }

        public static void AttachRemoved<TEntity>(this IDbContext reference, TEntity entity) where TEntity : class
        {
            var dbSet = reference.Set<TEntity>();
            dbSet.Attach(entity);
            dbSet.Remove(entity);
        }

        public static void AttachRemoved<TEntity>(this IDbContext reference, IEnumerable<TEntity> entities) where TEntity : class
        {
            foreach (var entity in entities) { reference.AttachRemoved(entity); }
        }



        /// <summary>
        ///     Creates the properly typed parameter objext for the specified Enity Framework FunctionImport 
        ///     parameter.
        /// </summary>
        /// <param name="reference">
        ///     The <see cref="DbContext"/> that defines the FunctionImport.
        /// </param>
        /// <param name="functionImportName">
        ///     A <see cref="String"/>.
        /// </param>
        /// <param name="parameterName">
        ///     A <see cref="String"/>.
        /// </param>
        /// <returns>
        ///     A properly typed <see cref="ObjectParameter"/>.
        /// </returns>
        /// <remarks>
        ///     Inspects the metadata for the specifed parameter and configures the new <see cref="ObjectParameter"/>
        ///     with the neccesary type information.
        /// </remarks>
        public static ObjectParameter CreateFunctionImportParameter(this IDbContext reference, 
            string functionImportName, string parameterName)
        {
            return ((IObjectContextAdapter)reference).ObjectContext.CreateFunctionImportParameter(
                functionImportName, parameterName);
        }

        public static EntityTypeBase GetEntityType(this IDbContext reference, 
            Type clrType)
        {
            return ((IObjectContextAdapter)reference).ObjectContext.GetEntityType(clrType);
        }

        public static void Merge<TEntity>(this IDbContext reference,
                IEnumerable<TEntity> sourceEntities, 
                IEnumerable<TEntity> destinationEntities)
            where TEntity : class
        {
            // TODO: Lw.Data.Entity.Extensions.Merge -- Robust error message.
            ExceptionOperations.VerifyNonNull(sourceEntities, ()=>sourceEntities);
            ExceptionOperations.VerifyNonNull(destinationEntities, ()=>destinationEntities);


            var join = sourceEntities.FullOuterJoin(destinationEntities, (s, d) => s.Equals(d));


            if (!join.Select(j => object.Equals(j.Item1, null)).Any())
            {
                ExceptionOperations.ThrowArgumentException(()=>destinationEntities);
            }

            var entitySet = reference.Set<TEntity>();
            var entityType = reference.GetMetadata().GetEntityType(entitySet.ElementType);

            var mergeContext = new MergeContext();

            foreach (var tuple in join)
            {
                if (object.Equals(tuple.Item2, null))
                {
                    // TODO: Raise proper deleted execption.
                    throw new NotImplementedException();
                }

                mergeContext.VisitedObjects.Add(tuple.Item2);

                MergeEntity(reference, entityType, typeof(TEntity), tuple.Item1, tuple.Item2, mergeContext);
            }
        }
        #endregion IDbContext Extensions

        #region IDbSet Extensions
        public static void Add<TEntity>(this IDbSet<TEntity> reference,
                IEnumerable<TEntity> entities)
            where TEntity : class
        {
            foreach (var entity in entities) { reference.Add(entity); }
        }

        public static void Attach<TEntity>(this IDbSet<TEntity> reference,
                IEnumerable<TEntity> entities)
            where TEntity : class

        {
            foreach (var entity in entities) { reference.Attach(entity); }
        }

        public static void Remove<TEntity>(this IDbSet<TEntity> reference, 
                IEnumerable<TEntity> entities)
            where TEntity : class
        {
            foreach (var entity in entities) { reference.Remove(entity); }
        }
        #endregion IDbSet Extensions

        #region Private Types
        private class MergeContext
        {
            public MergeContext()
            {
                this.VisitedObjects = new HashSet<object>();
            }

            public ISet<object> VisitedObjects { get; set; }
        }
        #endregion Private Types

        #region Private Methods
        private static void MergeEntity(IDbContext reference,
            EntityTypeBase entityType, 
            Type clrType, 
            object sourceValue, 
            object destinationValue, 
            MergeContext mergeContext)
        {
            foreach (var member in entityType.Members)
            {
                object sourceMemberValue = sourceValue.GetPropertyValue(member.Name);

                if (member.IsSimpleMember())
                {
                    destinationValue.SetPropertyValue(member.Name, sourceMemberValue);
                }
                else // Complex Member
                {
                    var propertyType = (member.IsCollectionMember())
                        ? sourceMemberValue.GetType().GetGenericArguments()[0]
                        : destinationValue.GetType().GetProperty(member.Name).PropertyType;

                    object destinationMemberValue = destinationValue.GetPropertyValue(member.Name);

                    if (!member.IsCollectionMember())
                    {
                        bool isSourceValueNull = object.Equals(sourceMemberValue, null);
                        bool isDestinationValueNull = object.Equals(destinationMemberValue, null);

                        MergeEntityMember(
                            reference, propertyType, member, sourceMemberValue, destinationMemberValue, mergeContext);
                    }
                    else
                    {
                        var sourceCollection = (IEnumerable)sourceMemberValue;
                        var destinationCollection = (IEnumerable)destinationMemberValue;


                        var method = typeof(Lw.Data.Entity.Extensions).GetMethod(
                            "PrivateEquals", BindingFlags.NonPublic | BindingFlags.Static);

                        var equalsMethod = method.MakeGenericMethod(propertyType);

                        // http://stackoverflow.com/questions/10712726/create-an-expressionfunc-using-reflection
                        var funcType = typeof(Func<,,>).MakeGenericType(propertyType, propertyType, typeof(bool));

                        // http://stackoverflow.com/questions/2933221/can-you-get-a-funct-or-similar-from-a-methodinfo-object
                        ParameterExpression x = Expression.Parameter(propertyType, "x");
                        ParameterExpression y = Expression.Parameter(propertyType, "y");

                        var equalsFunc = Expression.Lambda(funcType, Expression.Call(equalsMethod, x, y), x, y).Compile();

                        IEnumerable join = (IEnumerable)(typeof(Lw.Linq.Extensions).InvokeGenericMethod(
                            new[] { propertyType, propertyType },
                            "FullOuterJoin",
                            sourceCollection,
                            destinationCollection,
                            equalsFunc));

                        var tupleType = typeof(Tuple<,>).MakeGenericType(propertyType, propertyType);

                        foreach (object item in join)
                        {
                            object first = item.GetPropertyValue("Item1");
                            object second = item.GetPropertyValue("Item2");

                            if (first == null && second == null) { new InternalErrorException().Throw(); }

                            MergeEntityMember(reference, propertyType, member, first, second, mergeContext);
                        }
                    }
                }
            }
        }

        private static void MergeEntityMember(
            IDbContext reference,
            Type clrType,
            EdmMember member,
            object sourceMemberValue,
            object destinationMemberValue,
            MergeContext mergeContext)
        {
            bool isSourceValueNull = object.Equals(sourceMemberValue, null);
            bool isDestinationValueNull = object.Equals(destinationMemberValue, null);

            if (   isSourceValueNull && isDestinationValueNull 
                || mergeContext.VisitedObjects.Contains(destinationMemberValue))
            {
                return;
            }

            mergeContext.VisitedObjects.Add(destinationMemberValue);



            var memberType = member.GetElementType();

            if (isSourceValueNull != isDestinationValueNull)
            {
                var entitySet =
                    typeof(IDbContext).GetMethod("Set").MakeGenericMethod(clrType).Invoke(reference, null);

                if (isSourceValueNull)
                {
                    entitySet.InvokeMethod("Remove");
                }
                else
                {
                    entitySet.InvokeMethod("Add");
                }

            }
            else
            {
                var entityType = (EntityType)memberType.EdmType;

                Lw.Data.Entity.Extensions.MergeEntity(
                    reference, entityType, clrType, sourceMemberValue, destinationMemberValue, mergeContext);
            }
        }


        private static bool PrivateEquals<T>(T x, T y)
        {
            return x.Equals(y);
        }
        #endregion Private Methods
    }
}
