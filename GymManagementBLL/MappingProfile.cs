using AutoMapper;
using GymManagementBLL.ViewModels;
using GymManagementDAL.Entities;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL
{

    // MappingProfile: This class inherits from AutoMapper.Profile
    // It is responsible for defining the rules for mapping data from source entities to destination ViewModels
    // Here we configure mapping from Session to SessionViewModel
    // - CreateMap<Source, Destination>: tells AutoMapper what the source and destination types are
    // - ForMember: specifies a particular property in the destination and where its value should come from
    //      - MapFrom(src => src.Category.CategoryName): get CategoryName from the related Category entity
    //      - MapFrom(src => src.Trainer.Name): get TrainerName from the related Trainer entity
    // - Ignore: tells AutoMapper to ignore a specific property in the destination because it will be calculated manually
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapSession();
        }

        private void MapSession()
        {
            // Make configure on Mapping Session.
            CreateMap<Session, SessionViewModel>() // Session is Source, SessionViewModel is Destination
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName)) // Map CategoryName from Category entity
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name)) // Map TrainerName from Trainer entity
                .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore()); // Ignore AvailableSlots, will be calculated manually

            CreateMap<CreateSessionViewModel, Session>(); // Mapping from CreateSessionViewModel to Session entity

            CreateMap<Session, SessionToUpdateViewModel>().ReverseMap(); // Update from SessionToUpdateViewModel Or Or vice versa


        }
    }
}
