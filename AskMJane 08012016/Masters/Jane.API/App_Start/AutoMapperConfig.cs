using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Jane.Core.Models;
using Jane.Data.EntityFramework.Entities;

namespace Jane.API
{
    public class AutoMapperConfig
    {

        public static void Configure()
        {
            Mapper.CreateMap<Address, AddressModel>();
            Mapper.CreateMap<AddressModel, Address>();

            Mapper.CreateMap<string[], string>().ConvertUsing(new StringArrayToStringConverter());
            Mapper.CreateMap<string, string[]>().ConvertUsing(new StringToStringArrayConverter());

            Mapper.CreateMap<List<HoursOfOperation>, string>().ConvertUsing(new HoursToStringConverter());
            Mapper.CreateMap<string, List<HoursOfOperation>>().ConvertUsing(new StringToHoursConverter());

            Mapper.CreateMap<Dispensary, DispensaryModel>()
                .ForMember(d => d.ApprovalZipCodes,
                    z => z.MapFrom(s => String.Join(", ", s.ApprovalZipCodes.Select(c => c.Code))))
                .ForMember(d => d.DeliveryZipCodes,
                    z => z.MapFrom(s => String.Join(", ", s.DeliveryZipCodes.Select(c => c.Code))));

            Mapper.CreateMap<DispensaryModel, Dispensary>()
                .ForMember(x => x.ApprovalZipCodes, opt => opt.Ignore())
                .ForMember(x => x.DeliveryZipCodes, opt => opt.Ignore());
            Mapper.CreateMap<Dispensary, ThinDispensaryModel>();

            Mapper.CreateMap<DispensaryProduct, DispensaryProductModel>();
            Mapper.CreateMap<DispensaryProduct, ThinDispensaryProductModel>();
            Mapper.CreateMap<DispensaryProductModel, DispensaryProduct>();

            Mapper.CreateMap<DispensaryProductVariant, DispensaryProductVariantModel>();
            Mapper.CreateMap<DispensaryProductVariantModel, DispensaryProductVariant>();

            Mapper.CreateMap<DispensaryProductVariantOrder, DispensaryProductVariantOrderModel>();

            Mapper.CreateMap<Order, OrderModel>();
            Mapper.CreateMap<OrderModel, Order>();

            Mapper.CreateMap<PatientInfo, PatientInfoModel>();
            Mapper.CreateMap<PatientInfoModel, PatientInfo>();

            Mapper.CreateMap<Product, ProductModel>();
            Mapper.CreateMap<ProductModel, Product>();

            Mapper.CreateMap<ProductCategory, ProductCategoryModel>();

            Mapper.CreateMap<List<VariantPricing>, string>().ConvertUsing(new VariantPricingConverter());
            Mapper.CreateMap<string, List<VariantPricing>>().ConvertUsing(new StringToVariantPricingConverter());

            Mapper.CreateMap<FileModel, File>();
            Mapper.CreateMap<File, FileModel>();

            Mapper.CreateMap<Role, RoleModel>();

            Mapper.CreateMap<User, UserModel>();
            Mapper.CreateMap<User, UserWithRolesModel>();
            Mapper.CreateMap<User, ThinUserModel>();
            Mapper.CreateMap<UserModel, User>();

            Mapper.CreateMap<Wallet, WalletModel>();

            Mapper.CreateMap<Transaction, TransactionModel>();

            Mapper.CreateMap<Effect, EffectModel>();
            Mapper.CreateMap<EffectModel, Effect>();

            Mapper.CreateMap<Symptom, SymptomModel>();
            Mapper.CreateMap<SymptomModel, Symptom>();

            Mapper.CreateMap<DateTime, DateTimeOffset>();
        }
    }
}