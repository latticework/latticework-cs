using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Lw.Linq.Expressions
{
    /// <summary>
    /// 
    /// </summary>
    public static class LambdaExpressionOperations
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lambdaExpression"></param>
        /// <returns></returns>
        public static string FindMemberName<T>(Expression<Func<T>> lambdaExpression)
        {
            Expression expressionToCheck = lambdaExpression;
            bool done = false;
            while (!done)
            {
                switch (expressionToCheck.NodeType)
                {
                    case ExpressionType.Convert:
                        expressionToCheck = ((UnaryExpression)expressionToCheck).Operand;
                        break;
                    case ExpressionType.Lambda:
                        expressionToCheck = lambdaExpression.Body;
                        break;
                    case ExpressionType.MemberAccess:
                        var name = ((MemberExpression)expressionToCheck).Member.Name;
                        return name;
                    default:
                        done = true;
                        break;
                }
            }
            return null;
        }

        public static string FindMemberName<TObject, TProperty>(
            Expression<Func<TObject, TProperty>> lambdaExpression)
        {
            Expression expressionToCheck = lambdaExpression;
            bool done = false;
            while (!done)
            {
                switch (expressionToCheck.NodeType)
                {
                    case ExpressionType.Convert:
                        expressionToCheck = ((UnaryExpression)expressionToCheck).Operand;
                        break;
                    case ExpressionType.Lambda:
                        expressionToCheck = lambdaExpression.Body;
                        break;
                    case ExpressionType.MemberAccess:
                        var name = ((MemberExpression)expressionToCheck).Member.Name;
                        return name;
                    default:
                        done = true;
                        break;
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lambdaExpression"></param>
        /// <returns></returns>
        public static PropertyInfo FindPropertyInfo<T>(Expression<Func<T>> lambdaExpression)
        {
            Expression expressionToCheck = lambdaExpression;
            bool done = false;
            while (!done)
            {
                switch (expressionToCheck.NodeType)
                {
                    case ExpressionType.Convert:
                        expressionToCheck = ((UnaryExpression)expressionToCheck).Operand;
                        break;
                    case ExpressionType.Lambda:
                        expressionToCheck = lambdaExpression.Body;
                        break;
                    case ExpressionType.MemberAccess:
                        var propertyInfo = ((MemberExpression)expressionToCheck).Member as PropertyInfo;
                        return propertyInfo;
                    default:
                        done = true;
                        break;
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="lambdaExpression"></param>
        /// <returns></returns>
        public static PropertyInfo FindPropertyInfo<TObject, TProperty>(
            Expression<Func<TObject, TProperty>> lambdaExpression)
        {
            Expression expressionToCheck = lambdaExpression;
            bool done = false;
            while (!done)
            {
                switch (expressionToCheck.NodeType)
                {
                    case ExpressionType.Convert:
                        expressionToCheck = ((UnaryExpression)expressionToCheck).Operand;
                        break;
                    case ExpressionType.Lambda:
                        expressionToCheck = lambdaExpression.Body;
                        break;
                    case ExpressionType.MemberAccess:
                        var propertyInfo = ((MemberExpression)expressionToCheck).Member as PropertyInfo;
                        return propertyInfo;
                    default:
                        done = true;
                        break;
                }
            }
            return null;
        }

    }
}
