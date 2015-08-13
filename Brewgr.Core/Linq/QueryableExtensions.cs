using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ctorx.Core.Linq
{
	public static class QueryableExtensions
	{
		private static Expression<Func<TElement, bool>> GetWhereInExpression<TElement, TValue>(Expression<Func<TElement, TValue>> propertySelector, IEnumerable<TValue> values)
		{
			var p = propertySelector.Parameters.Single();
			if (!values.Any())
				return e => false;

			var equals = values.Select(value => (Expression)Expression.Equal(propertySelector.Body, Expression.Constant(value, typeof(TValue))));
			var body = equals.Aggregate<Expression>(Expression.Or);

			return Expression.Lambda<Func<TElement, bool>>(body, p);
		}

		private static Expression<Func<TElement, bool>> GetWhereNotInExpression<TElement, TValue>(Expression<Func<TElement, TValue>> propertySelector, IEnumerable<TValue> values)
		{
			var p = propertySelector.Parameters.Single();
			if (!values.Any())
				return e => false;

			var equals = values.Select(value => (Expression)Expression.NotEqual(propertySelector.Body, Expression.Constant(value, typeof(TValue))));
			var body = equals.Aggregate<Expression>(Expression.And);

			return Expression.Lambda<Func<TElement, bool>>(body, p);
		}

		/// <summary>  
		/// Return the element that the specified property's value is contained in the specifiec values  
		/// </summary>   
		public static IQueryable<TElement> WhereIn<TElement, TValue>(this IQueryable<TElement> source, Expression<Func<TElement, TValue>> propertySelector, params TValue[] values)
		{
			return source.Where(GetWhereInExpression(propertySelector, values));
		}

		/// <summary>  
		/// Return the element that the specified property's value is contained in the specifiec values  
		/// </summary>  
		public static IQueryable<TElement> WhereIn<TElement, TValue>(this IQueryable<TElement> source, Expression<Func<TElement, TValue>> propertySelector, IEnumerable<TValue> values)
		{
			return source.Where(GetWhereInExpression(propertySelector, values));
		}

		/// <summary>  
		/// Return the element that the specified property's value is contained in the specifiec values  
		/// </summary>   
		public static IQueryable<TElement> WhereNotIn<TElement, TValue>(this IQueryable<TElement> source, Expression<Func<TElement, TValue>> propertySelector, params TValue[] values)
		{
			return source.Where(GetWhereNotInExpression(propertySelector, values));
		}

		/// <summary>  
		/// Return the element that the specified property's value is contained in the specifiec values  
		/// </summary>  
		public static IQueryable<TElement> WhereNotIn<TElement, TValue>(this IQueryable<TElement> source, Expression<Func<TElement, TValue>> propertySelector, IEnumerable<TValue> values)
		{
			return source.Where(GetWhereNotInExpression(propertySelector, values));
		}
	} 
}