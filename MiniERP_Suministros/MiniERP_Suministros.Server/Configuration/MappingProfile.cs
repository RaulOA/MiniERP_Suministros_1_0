using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MiniERP_Suministros.Core.Models.Account;
using MiniERP_Suministros.Core.Models.Shop;
using MiniERP_Suministros.Core.Services.Account;
using MiniERP_Suministros.Server.ViewModels.Account;
using MiniERP_Suministros.Server.ViewModels.Shop;

namespace MiniERP_Suministros.Server.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserVM>()
                   .ForMember(d => d.Roles, map => map.Ignore());
            CreateMap<UserVM, ApplicationUser>()
                .ForMember(d => d.Roles, map => map.Ignore())
                .ForMember(d => d.Id, map => map.Condition(src => src.Id != null));

            CreateMap<ApplicationUser, UserEditVM>()
                .ForMember(d => d.Roles, map => map.Ignore());
            CreateMap<UserEditVM, ApplicationUser>()
                .ForMember(d => d.Roles, map => map.Ignore())
                .ForMember(d => d.Id, map => map.Condition(src => src.Id != null));

            CreateMap<ApplicationUser, UserPatchVM>()
                .ReverseMap();

            CreateMap<ApplicationRole, RoleVM>()
                .ForMember(d => d.Permissions, map => map.MapFrom(s => s.Claims))
                .ForMember(d => d.UsersCount, map => map.MapFrom(s => s.Users != null ? s.Users.Count : 0))
                .ReverseMap();
            CreateMap<RoleVM, ApplicationRole>()
                .ForMember(d => d.Id, map => map.Condition(src => src.Id != null));

            CreateMap<IdentityRoleClaim<string>, ClaimVM>()
                .ForMember(d => d.Type, map => map.MapFrom(s => s.ClaimType))
                .ForMember(d => d.Value, map => map.MapFrom(s => s.ClaimValue))
                .ReverseMap();

            CreateMap<ApplicationPermission, PermissionVM>()
                .ReverseMap();

            CreateMap<IdentityRoleClaim<string>, PermissionVM>()
                .ConvertUsing(s => ((PermissionVM)ApplicationPermissions.GetPermissionByValue(s.ClaimValue))!);

            CreateMap<Customer, CustomerVM>()
                .ReverseMap();

            CreateMap<Product, ProductVM>()
                .ForMember(d => d.ProductCategoryId, m => m.MapFrom(s => s.ProductCategoryId))
                .ForMember(d => d.ParentId, m => m.MapFrom(s => s.ParentId))
                .ForMember(d => d.ProductCategoryName, m => m.MapFrom(s => s.ProductCategory != null ? s.ProductCategory.Name : null))
                .ForMember(d => d.ParentName, m => m.MapFrom(s => s.Parent != null ? s.Parent.Name : null))
                .ReverseMap()
                .ForMember(d => d.ProductCategory, m => m.Ignore()) // se asigna por servicio al validar
                .ForMember(d => d.Parent, m => m.Ignore());

            CreateMap<OrderDetail, OrderItemVM>()
                .ForMember(d => d.ProductName, m => m.MapFrom(s => s.Product.Name));

            CreateMap<Order, OrderVM>()
                .ForMember(d => d.CustomerId, m => m.MapFrom(s => s.CustomerId))
                .ForMember(d => d.CustomerName, m => m.MapFrom(s => s.Customer.Name))
                .ForMember(d => d.CashierName, m => m.MapFrom(s => s.Cashier != null ? s.Cashier.FullName : null))
                .ForMember(d => d.Items, m => m.MapFrom(s => s.OrderDetails));

            CreateMap<ProductCategory, ProductCategoryVM>()
                .ReverseMap();
        }
    }
}