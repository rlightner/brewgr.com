using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ctorx.Core.Linq
{
	public static class ExpressionExtensions
	{
		/// <summary>
		/// Gets a property name for y in (x => x.y)
		/// </summary>
		public static string GetPropertyName<TEntity, TProp>(this Expression<Func<TEntity, TProp>> expression)
		{
			var propInfo = expression.GetPropertyInfo();

			if (propInfo == null)
			{
				return null;
			}

			return propInfo.Name;
		}


		/// <summary>
		/// Gets a PropertyInfo for y in (x => x.y)
		/// </summary>
		public static PropertyInfo GetPropertyInfo<TEntity, TProp>(this Expression<Func<TEntity, TProp>> expression)
		{
			var propInfo = (PropertyInfo)((MemberExpression)(expression.Body)).Member;
			return propInfo;
		}


		/// <summary>
		/// Gets the type of x in (x => x.Property)
		/// </summary>
		public static string GetMemberTypeName<TEntity>(this Expression<Func<TEntity>> expression)
		{
			return ((MemberExpression)(expression.Body)).Member.ReflectedType.Name;
		}


		/// <summary>
		/// Gets the type of x in (x => x.Property)
		/// </summary>
		public static string GetMemberTypeName<TEntity, TProp>(this Expression<Func<TEntity, TProp>> expression)
		{
			return ((MemberExpression)(expression.Body)).Member.ReflectedType.Name;
		}
	}
}