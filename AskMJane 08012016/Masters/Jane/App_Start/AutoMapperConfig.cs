using System;
using System.Collections.Generic;
using AutoMapper;
using Jane.Core.Models;
using Jane.Data.EntityFramework.Entities;

namespace Jane.Web
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

            Mapper.CreateMap<Dispensary, DispensaryModel>();
            Mapper.CreateMap<DispensaryModel, Dispensary>();

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

            Mapper.CreateMap<User, UserModel>();
            Mapper.CreateMap<User, ThinUserModel>();
            Mapper.CreateMap<UserModel, User>();

            Mapper.CreateMap<Wallet, WalletModel>();

            Mapper.CreateMap<Transaction, TransactionModel>();

            Mapper.CreateMap<Effect, EffectModel>();

            Mapper.CreateMap<Symptom, SymptomModel>();

            Mapper.CreateMap<DateTime, DateTimeOffset>();
        }
    }
}