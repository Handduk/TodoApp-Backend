using API.Dtos;
using AutoMapper;
using Entity;

namespace Api.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Todo, TodoDto>();

        }
    }
}