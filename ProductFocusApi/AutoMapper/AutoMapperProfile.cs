using AutoMapper;
using ProductTests.Application.CommandHandler.TestCaseCommands;
using ProductTests.Domain.Model.TestCaseAggregate;

namespace ProductFocusApi.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UpdateTestStepDto, TestStep>();
        }
    }
}
