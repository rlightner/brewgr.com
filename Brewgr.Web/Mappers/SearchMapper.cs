using System;
using System.Linq;
using AutoMapper;
using Brewgr.Web.Core.Model;
using Brewgr.Web.Models;
using ctorx.Core.Mapping;

namespace Brewgr.Web.Mappers
{
	public class SearchMapper : IMappingDefinition
	{
		public void DefineMappings()
		{
			Mapper.CreateMap<SearchResult, SearchViewModel>();
		}
	}
}