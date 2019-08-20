using AutoMapper;
using StreetLegal.Models;
using StreetLegal.Models.CarModels;
using StreetLegal.ViewModels.AdminViewModels;
using StreetLegal.ViewModels.HomeViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreetLegal.Extentions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateCarVM, Car>();
            CreateMap<Car, CreateCarVM>();

            CreateMap<CreateEngineVM, Engine>();
            CreateMap<Engine, CreateEngineVM>();

            CreateMap<CreateTyresVM, Tyres>();
            CreateMap<Tyres, CreateTyresVM>();

            CreateMap<CreatePartVM, Part>();
            CreateMap<Part, CreatePartVM>();
        }
    }
}
