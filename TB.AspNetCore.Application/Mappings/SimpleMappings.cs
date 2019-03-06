using AutoMapper;
using System.Collections.Generic;
using TB.AspNetCore.Domain.Entitys;
using TB.AspNetCore.Domain.Models.Api;

namespace TB.AspNetCore.Application.Mappings
{
    /// <summary>
    /// model mapping
    /// </summary>
    public class SimpleMappings : Profile
    {
        public SimpleMappings()
        {
            CreateMap<MsgContent, MsgContentModel>().ReverseMap();
            CreateMap<MsgContentModel, MsgContent>().ReverseMap();
        }

    }
}
