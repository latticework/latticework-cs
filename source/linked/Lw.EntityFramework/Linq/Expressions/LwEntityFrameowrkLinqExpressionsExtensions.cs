using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Lw.Data.Entity;
using Lw.Data.Metadata.Edm;
using Lw.Reflection;
using System.Reflection;
using Lw.Collections.Generic;

namespace Lw.Linq.Expressions
{
    public static class EntityFrameowrkLinqExpressionsExtensions
    {
        public static Expression<Func<TEntity, bool>> BuildEntityInPredicate<TEntity>(this IDbContext reference,
            IEnumerable<TEntity> entities) where TEntity : class
        {
            ExceptionOperations.VerifyNonNull(entities, () => entities);


            var keyProperites = reference.GetEntityType(typeof(TEntity)).KeyMembers;

            if (keyProperites.IsNullOrEmpty())
            {
                throw new InvalidOperationException();
            }



            if (!entities.Any()) { return e0 => false; }


            ParameterExpression e = Expression.Parameter(typeof(TEntity), "e");

            BinaryExpression body = null;

            bool firstEntity = true;

            foreach (var entity in entities)
            {
                BinaryExpression entityExpression = null;
                bool firstProperty = true;

                foreach (var keyProperty in keyProperites)
                {
                    BinaryExpression propertyExpression;
                    var propertyName = keyProperty.Name;
                    var propertyType = typeof(TEntity).GetProperty(propertyName).PropertyType;
                    object propertyValue = entity.GetPropertyValue(propertyName);

                    propertyExpression = Expression.Equal(
                        Expression.Property(e, propertyName), Expression.Constant(propertyValue, propertyType));

                    if (firstProperty)
                    {
                        firstProperty = false;
                        entityExpression = propertyExpression;
                    }
                    else
                    {
                        entityExpression = Expression.And(entityExpression, propertyExpression);
                    }
                }

                if (firstEntity)
                {
                    body = entityExpression;
                }
                else
                {
                    body = Expression.Or(body, entityExpression);
                }
            }

            // http://social.msdn.microsoft.com/Forums/en-US/adodotnetentityframework/thread/095745fe-dcf0-4142-b684-b7e4a1ab59f0
            return Expression.Lambda<Func<TEntity, bool>>(body, e);
        }
    }
}
