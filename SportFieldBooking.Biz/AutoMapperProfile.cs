// 1st
using AutoMapper;
using SportFieldBooking;

namespace SportFieldBooking.Biz
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Create mappings from business model to data model and vice versa
            CreateMap<Data.Model.User, Biz.Model.User.New>().ReverseMap();
            CreateMap<Data.Model.User, Biz.Model.User.View>().ReverseMap();
            CreateMap<Data.Model.User, Biz.Model.User.List>().ReverseMap();
            CreateMap<Data.Model.User, Biz.Model.User.Edit>().ReverseMap();
            CreateMap<Data.Model.User, Biz.Model.User.LoginRequest>().ReverseMap();
            CreateMap<Data.Model.User, Biz.Model.User.RegisterRequest>().ReverseMap();
            CreateMap<Data.Model.User, Biz.Model.User.LoginResponse>().ReverseMap();
            CreateMap<Data.Model.User, Biz.Model.User.CurrentUser>().ReverseMap();

            CreateMap<Data.Model.SportField, Biz.Model.SportField.New>().ReverseMap();
            CreateMap<Data.Model.SportField, Biz.Model.SportField.View>().ReverseMap();
            CreateMap<Data.Model.SportField, Biz.Model.SportField.List>().ReverseMap();
            CreateMap<Data.Model.SportField, Biz.Model.SportField.Edit>().ReverseMap();

            CreateMap<Data.Model.Booking, Biz.Model.Booking.New>().ReverseMap();
            CreateMap<Data.Model.Booking, Biz.Model.Booking.View>().ReverseMap();
            CreateMap<Data.Model.Booking, Biz.Model.Booking.List>().ReverseMap();

            CreateMap<Data.Model.BookingStatus, Biz.Model.BookingStatus.New>().ReverseMap();
            CreateMap<Data.Model.BookingStatus, Biz.Model.BookingStatus.View>().ReverseMap();
            CreateMap<Data.Model.BookingStatus, Biz.Model.BookingStatus.List>().ReverseMap();
            CreateMap<Data.Model.BookingStatus, Biz.Model.BookingStatus.Edit>().ReverseMap();
        }
    }
}
