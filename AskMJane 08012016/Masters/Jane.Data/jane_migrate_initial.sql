--
-- PostgreSQL database dump
--

-- Dumped from database version 9.4.4
-- Dumped by pg_dump version 9.4.4
-- Started on 2015-09-17 07:25:26

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

SET search_path = public, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 181 (class 1259 OID 18693)
-- Name: Addresses; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "Addresses" (
    "Id" integer NOT NULL,
    "Name" text,
    "Address1" text NOT NULL,
    "Address2" text,
    "City" text NOT NULL,
    "State" text NOT NULL,
    "Zip" text NOT NULL,
    "Country" text NOT NULL,
    "PhoneNumber" text,
    "IsDeleted" boolean DEFAULT false NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "LatLong" point,
    "Coordinates" geometry(Point,4326),
    "Latitude" double precision,
    "Longitude" double precision,
    "FormattedAddress" text
);


ALTER TABLE "Addresses" OWNER TO postgres;

--
-- TOC entry 180 (class 1259 OID 18691)
-- Name: Addresses_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "Addresses_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "Addresses_Id_seq" OWNER TO postgres;

--
-- TOC entry 3960 (class 0 OID 0)
-- Dependencies: 180
-- Name: Addresses_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "Addresses_Id_seq" OWNED BY "Addresses"."Id";


--
-- TOC entry 188 (class 1259 OID 18746)
-- Name: Dispensaries; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "Dispensaries" (
    "Id" integer NOT NULL,
    "Name" text NOT NULL,
    "PhoneNumber" text NOT NULL,
    "EmailAddress" text NOT NULL,
    "HoursAndInfo" text NOT NULL,
    "Description" text NOT NULL,
    "PhotoUrl" text,
    "HasDelivery" boolean DEFAULT false NOT NULL,
    "HasPickup" boolean DEFAULT false NOT NULL,
    "HasScheduledDelivery" boolean DEFAULT false NOT NULL,
    "HasStorefront" boolean DEFAULT false NOT NULL,
    "IsCaregiver" boolean DEFAULT false NOT NULL,
    "IsPrivate" boolean DEFAULT false NOT NULL,
    "Slug" text NOT NULL,
    "LeaflySlug" text,
    "AddressId" integer NOT NULL,
    "Photos" json,
    "HoursOfOperation" json,
    "DeliveryZones" json,
    "IsDeleted" boolean DEFAULT false NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "Coordinates" geometry(Point,4326),
    "ScheduledDeliveryZipCodes" text[],
    "ApprovalZipCodes" text,
    "DeliveryZipCodes" text,
    "Guid" text NOT NULL,
    "OnfleetMerchantId" text
);


ALTER TABLE "Dispensaries" OWNER TO postgres;

--
-- TOC entry 187 (class 1259 OID 18744)
-- Name: Dispensaries_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "Dispensaries_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "Dispensaries_Id_seq" OWNER TO postgres;

--
-- TOC entry 3961 (class 0 OID 0)
-- Dependencies: 187
-- Name: Dispensaries_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "Dispensaries_Id_seq" OWNED BY "Dispensaries"."Id";


--
-- TOC entry 205 (class 1259 OID 19258)
-- Name: DispensaryInvites; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "DispensaryInvites" (
    "Id" integer NOT NULL,
    "DispensaryInviteCode" text NOT NULL,
    "Name" text,
    "Email" text,
    "Phone" text,
    "DispensaryId" integer NOT NULL,
    "UserId" integer,
    "IsDeleted" boolean DEFAULT false NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE "DispensaryInvites" OWNER TO postgres;

--
-- TOC entry 204 (class 1259 OID 19256)
-- Name: DispensaryInvites_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "DispensaryInvites_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "DispensaryInvites_Id_seq" OWNER TO postgres;

--
-- TOC entry 3962 (class 0 OID 0)
-- Dependencies: 204
-- Name: DispensaryInvites_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "DispensaryInvites_Id_seq" OWNED BY "DispensaryInvites"."Id";


--
-- TOC entry 194 (class 1259 OID 18803)
-- Name: DispensaryProductVariantOrders; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "DispensaryProductVariantOrders" (
    "Id" integer NOT NULL,
    "IsPricedByWeight" boolean DEFAULT false NOT NULL,
    "Weight" numeric(18,2) NOT NULL,
    "Quantity" numeric(18,2) NOT NULL,
    "UnitPrice" numeric(18,2) NOT NULL,
    "TotalPrice" numeric(18,2) NOT NULL,
    "OrderId" integer NOT NULL,
    "DispensaryProductVariantId" integer NOT NULL,
    "IsDeleted" boolean DEFAULT false NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE "DispensaryProductVariantOrders" OWNER TO postgres;

--
-- TOC entry 193 (class 1259 OID 18801)
-- Name: DispensaryProductVariantOrders_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "DispensaryProductVariantOrders_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "DispensaryProductVariantOrders_Id_seq" OWNER TO postgres;

--
-- TOC entry 3963 (class 0 OID 0)
-- Dependencies: 193
-- Name: DispensaryProductVariantOrders_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "DispensaryProductVariantOrders_Id_seq" OWNED BY "DispensaryProductVariantOrders"."Id";


--
-- TOC entry 192 (class 1259 OID 18788)
-- Name: DispensaryProductVariants; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "DispensaryProductVariants" (
    "Id" integer NOT NULL,
    "Name" text,
    "IsPricedByWeight" boolean DEFAULT false NOT NULL,
    "VariantQuantity" numeric(18,2),
    "Price" numeric(18,2),
    "DispensaryProductId" integer NOT NULL,
    "IsDeleted" boolean DEFAULT false NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "Slug" text,
    "DisplayOrder" integer,
    "VariantPricing" jsonb NOT NULL,
    "Photos" jsonb,
    "VariantAttributes" jsonb,
    "LeaflySlug" text,
    "IsMasterVariant" boolean DEFAULT false,
    "Guid" text NOT NULL
);


ALTER TABLE "DispensaryProductVariants" OWNER TO postgres;

--
-- TOC entry 191 (class 1259 OID 18786)
-- Name: DispensaryProductVariants_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "DispensaryProductVariants_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "DispensaryProductVariants_Id_seq" OWNER TO postgres;

--
-- TOC entry 3964 (class 0 OID 0)
-- Dependencies: 191
-- Name: DispensaryProductVariants_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "DispensaryProductVariants_Id_seq" OWNED BY "DispensaryProductVariants"."Id";


--
-- TOC entry 190 (class 1259 OID 18770)
-- Name: DispensaryProducts; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "DispensaryProducts" (
    "Id" integer NOT NULL,
    "Name" text NOT NULL,
    "Description" text NOT NULL,
    "IsAvailable" boolean DEFAULT true NOT NULL,
    "IsDiscounted" boolean DEFAULT false NOT NULL,
    "IsPopular" boolean DEFAULT false NOT NULL,
    "ProductId" integer NOT NULL,
    "DispensaryId" integer,
    "Slug" text NOT NULL,
    "LeaflySlug" text,
    "IsDeleted" boolean DEFAULT false NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "Photos" json,
    "ProductAttributes" json
);


ALTER TABLE "DispensaryProducts" OWNER TO postgres;

--
-- TOC entry 189 (class 1259 OID 18768)
-- Name: DispensaryProducts_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "DispensaryProducts_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "DispensaryProducts_Id_seq" OWNER TO postgres;

--
-- TOC entry 3965 (class 0 OID 0)
-- Dependencies: 189
-- Name: DispensaryProducts_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "DispensaryProducts_Id_seq" OWNED BY "DispensaryProducts"."Id";


--
-- TOC entry 203 (class 1259 OID 19212)
-- Name: DispensaryStaff; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "DispensaryStaff" (
    "Id" integer NOT NULL,
    "UserId" integer NOT NULL,
    "DispensaryId" integer NOT NULL,
    "JobRole" text,
    "Phone" text,
    "Email" text,
    "HiredDate" timestamp with time zone,
    "IsDeleted" boolean DEFAULT false NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE "DispensaryStaff" OWNER TO postgres;

--
-- TOC entry 202 (class 1259 OID 19210)
-- Name: DispensaryStaff_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "DispensaryStaff_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "DispensaryStaff_Id_seq" OWNER TO postgres;

--
-- TOC entry 3966 (class 0 OID 0)
-- Dependencies: 202
-- Name: DispensaryStaff_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "DispensaryStaff_Id_seq" OWNED BY "DispensaryStaff"."Id";


--
-- TOC entry 211 (class 1259 OID 19338)
-- Name: Effects; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "Effects" (
    "Id" integer NOT NULL,
    "Type" text NOT NULL,
    "Name" text NOT NULL,
    "Description" text
);


ALTER TABLE "Effects" OWNER TO postgres;

--
-- TOC entry 210 (class 1259 OID 19336)
-- Name: Effects_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "Effects_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "Effects_Id_seq" OWNER TO postgres;

--
-- TOC entry 3967 (class 0 OID 0)
-- Dependencies: 210
-- Name: Effects_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "Effects_Id_seq" OWNED BY "Effects"."Id";


--
-- TOC entry 215 (class 1259 OID 19376)
-- Name: Flavors; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "Flavors" (
    "Id" integer NOT NULL,
    "Name" text NOT NULL
);


ALTER TABLE "Flavors" OWNER TO postgres;

--
-- TOC entry 214 (class 1259 OID 19374)
-- Name: Flavors_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "Flavors_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "Flavors_Id_seq" OWNER TO postgres;

--
-- TOC entry 3968 (class 0 OID 0)
-- Dependencies: 214
-- Name: Flavors_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "Flavors_Id_seq" OWNED BY "Flavors"."Id";


--
-- TOC entry 207 (class 1259 OID 19303)
-- Name: Invites; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "Invites" (
    "Id" integer NOT NULL,
    "InviteCode" text,
    "InviterId" integer,
    "InviteeId" integer,
    "InviterCredit" numeric(18,2),
    "InviteeCredit" numeric(18,2),
    "IsDeleted" boolean DEFAULT false NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "InviteeEmail" text,
    "InviteePhone" text,
    "InviteeName" text
);


ALTER TABLE "Invites" OWNER TO postgres;

--
-- TOC entry 206 (class 1259 OID 19301)
-- Name: Invites_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "Invites_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "Invites_Id_seq" OWNER TO postgres;

--
-- TOC entry 3969 (class 0 OID 0)
-- Dependencies: 206
-- Name: Invites_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "Invites_Id_seq" OWNED BY "Invites"."Id";


--
-- TOC entry 221 (class 1259 OID 19474)
-- Name: LabTestTypes; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "LabTestTypes" (
    "Id" integer NOT NULL,
    "LabTestName" text NOT NULL,
    "LabTestMedian" numeric(10,4),
    "LabTestRangeLow" numeric(10,4),
    "LabTestRangeHigh" numeric(10,4),
    "LabTestDescription" text NOT NULL
);


ALTER TABLE "LabTestTypes" OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 19472)
-- Name: LabTestTypes_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "LabTestTypes_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "LabTestTypes_Id_seq" OWNER TO postgres;

--
-- TOC entry 3970 (class 0 OID 0)
-- Dependencies: 220
-- Name: LabTestTypes_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "LabTestTypes_Id_seq" OWNED BY "LabTestTypes"."Id";


--
-- TOC entry 291 (class 1259 OID 23280)
-- Name: OAuthClients; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "OAuthClients" (
    "Id" integer NOT NULL,
    "Name" text,
    "ClientSecretHash" text,
    "AllowedGrant" integer,
    "CreatedAt" timestamp with time zone,
    "UpdatedAt" timestamp with time zone
);


ALTER TABLE "OAuthClients" OWNER TO postgres;

--
-- TOC entry 290 (class 1259 OID 23278)
-- Name: OAuthClients_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "OAuthClients_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "OAuthClients_Id_seq" OWNER TO postgres;

--
-- TOC entry 3971 (class 0 OID 0)
-- Dependencies: 290
-- Name: OAuthClients_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "OAuthClients_Id_seq" OWNED BY "OAuthClients"."Id";


--
-- TOC entry 196 (class 1259 OID 18818)
-- Name: Orders; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "Orders" (
    "Id" integer NOT NULL,
    "PaymentType" integer NOT NULL,
    "DeliveryType" integer NOT NULL,
    "IsCheckedOut" boolean DEFAULT false NOT NULL,
    "IsConfirmed" boolean DEFAULT false NOT NULL,
    "IsReceived" boolean DEFAULT false NOT NULL,
    "OrderReferenceId" text NOT NULL,
    "OnfleetDestinationId" text,
    "OnfleetTaskId" text,
    "TotalPrice" numeric(18,2),
    "BillingAddressId" integer,
    "DeliveryAddressId" integer,
    "UserId" integer,
    "DeliveryCharge" numeric(18,2),
    "IsDeleted" boolean DEFAULT false NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "CheckedOutAt" timestamp with time zone,
    "DispensaryId" integer,
    "OnfleetTrackingURL" text,
    "OrderStatus" integer DEFAULT 0 NOT NULL
);


ALTER TABLE "Orders" OWNER TO postgres;

--
-- TOC entry 195 (class 1259 OID 18816)
-- Name: Orders_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "Orders_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "Orders_Id_seq" OWNER TO postgres;

--
-- TOC entry 3972 (class 0 OID 0)
-- Dependencies: 195
-- Name: Orders_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "Orders_Id_seq" OWNED BY "Orders"."Id";


--
-- TOC entry 186 (class 1259 OID 18730)
-- Name: PatientInfos; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "PatientInfos" (
    "Id" integer NOT NULL,
    "MedicalCardNumber" text,
    "MedicalCardExpirationDate" timestamp with time zone,
    "IsValidMedicalCard" boolean DEFAULT false NOT NULL,
    "MedicalCardValidationDate" timestamp with time zone,
    "RecommendationImageUrl" text,
    "DriversLicenseNumber" text,
    "DriversLicenseImageUrl" text,
    "IsApproved" boolean DEFAULT false NOT NULL,
    "PreferredWeight" numeric(18,2) NOT NULL,
    "ApprovalStatus" integer DEFAULT 1 NOT NULL
);


ALTER TABLE "PatientInfos" OWNER TO postgres;

--
-- TOC entry 185 (class 1259 OID 18728)
-- Name: PatientInfos_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "PatientInfos_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "PatientInfos_Id_seq" OWNER TO postgres;

--
-- TOC entry 3973 (class 0 OID 0)
-- Dependencies: 185
-- Name: PatientInfos_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "PatientInfos_Id_seq" OWNED BY "PatientInfos"."Id";


--
-- TOC entry 200 (class 1259 OID 18852)
-- Name: ProductCategories; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "ProductCategories" (
    "Id" integer NOT NULL,
    "Name" text DEFAULT ''::text NOT NULL,
    "Color" text DEFAULT ''::text NOT NULL,
    "PhotoUrl" text DEFAULT ''::text NOT NULL
);


ALTER TABLE "ProductCategories" OWNER TO postgres;

--
-- TOC entry 199 (class 1259 OID 18850)
-- Name: ProductCategories_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "ProductCategories_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "ProductCategories_Id_seq" OWNER TO postgres;

--
-- TOC entry 3974 (class 0 OID 0)
-- Dependencies: 199
-- Name: ProductCategories_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "ProductCategories_Id_seq" OWNED BY "ProductCategories"."Id";


--
-- TOC entry 217 (class 1259 OID 19387)
-- Name: ProductReviewEffects; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "ProductReviewEffects" (
    "Id" integer NOT NULL,
    "ProductReviewId" integer NOT NULL,
    "EffectId" integer NOT NULL,
    "IsDeleted" boolean DEFAULT false NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE "ProductReviewEffects" OWNER TO postgres;

--
-- TOC entry 216 (class 1259 OID 19385)
-- Name: ProductReviewEffects_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "ProductReviewEffects_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "ProductReviewEffects_Id_seq" OWNER TO postgres;

--
-- TOC entry 3975 (class 0 OID 0)
-- Dependencies: 216
-- Name: ProductReviewEffects_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "ProductReviewEffects_Id_seq" OWNED BY "ProductReviewEffects"."Id";


--
-- TOC entry 219 (class 1259 OID 19405)
-- Name: ProductReviewFlavors; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "ProductReviewFlavors" (
    "Id" integer NOT NULL,
    "ProductReviewId" integer NOT NULL,
    "FlavorId" integer NOT NULL,
    "IsDeleted" boolean DEFAULT false NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE "ProductReviewFlavors" OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 19403)
-- Name: ProductReviewFlavors_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "ProductReviewFlavors_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "ProductReviewFlavors_Id_seq" OWNER TO postgres;

--
-- TOC entry 3976 (class 0 OID 0)
-- Dependencies: 218
-- Name: ProductReviewFlavors_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "ProductReviewFlavors_Id_seq" OWNED BY "ProductReviewFlavors"."Id";


--
-- TOC entry 301 (class 1259 OID 36087)
-- Name: ProductReviewProperties; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "ProductReviewProperties" (
    "Id" integer NOT NULL,
    "ProductReviewId" integer NOT NULL,
    "PropertyId" integer NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "IsDeleted" boolean DEFAULT false NOT NULL
);


ALTER TABLE "ProductReviewProperties" OWNER TO postgres;

--
-- TOC entry 300 (class 1259 OID 36085)
-- Name: ProductReviewProperties_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "ProductReviewProperties_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "ProductReviewProperties_Id_seq" OWNER TO postgres;

--
-- TOC entry 3977 (class 0 OID 0)
-- Dependencies: 300
-- Name: ProductReviewProperties_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "ProductReviewProperties_Id_seq" OWNED BY "ProductReviewProperties"."Id";


--
-- TOC entry 213 (class 1259 OID 19349)
-- Name: ProductReviews; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "ProductReviews" (
    "Id" integer NOT NULL,
    "UserId" integer,
    "Review" text NOT NULL,
    "Rating" numeric(3,2) NOT NULL,
    "PurchsedFromDispensaryId" integer,
    "ProductId" integer,
    "DispensaryProductId" integer,
    "ReviewedType" integer NOT NULL,
    "IsDeleted" boolean DEFAULT false NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE "ProductReviews" OWNER TO postgres;

--
-- TOC entry 212 (class 1259 OID 19347)
-- Name: ProductReviews_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "ProductReviews_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "ProductReviews_Id_seq" OWNER TO postgres;

--
-- TOC entry 3978 (class 0 OID 0)
-- Dependencies: 212
-- Name: ProductReviews_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "ProductReviews_Id_seq" OWNED BY "ProductReviews"."Id";


--
-- TOC entry 198 (class 1259 OID 18837)
-- Name: Products; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "Products" (
    "Id" integer NOT NULL,
    "Name" text DEFAULT ''::text NOT NULL,
    "Description" text DEFAULT ''::text NOT NULL,
    "Slug" text DEFAULT ''::text NOT NULL,
    "LeaflySlug" text,
    "ProductCategoryId" integer DEFAULT 0 NOT NULL,
    "Photos" json,
    "IsDeleted" boolean DEFAULT false NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "Attributes" json
);


ALTER TABLE "Products" OWNER TO postgres;

--
-- TOC entry 197 (class 1259 OID 18835)
-- Name: Products_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "Products_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "Products_Id_seq" OWNER TO postgres;

--
-- TOC entry 3979 (class 0 OID 0)
-- Dependencies: 197
-- Name: Products_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "Products_Id_seq" OWNED BY "Products"."Id";


--
-- TOC entry 209 (class 1259 OID 19327)
-- Name: Properties; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "Properties" (
    "Id" integer NOT NULL,
    "Type" text NOT NULL,
    "Name" text NOT NULL,
    "Description" text
);


ALTER TABLE "Properties" OWNER TO postgres;

--
-- TOC entry 208 (class 1259 OID 19325)
-- Name: Properties_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "Properties_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "Properties_Id_seq" OWNER TO postgres;

--
-- TOC entry 3980 (class 0 OID 0)
-- Dependencies: 208
-- Name: Properties_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "Properties_Id_seq" OWNED BY "Properties"."Id";


--
-- TOC entry 176 (class 1259 OID 18656)
-- Name: Roles; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "Roles" (
    "Id" integer NOT NULL,
    "Name" character varying(256) DEFAULT ''::character varying NOT NULL
);


ALTER TABLE "Roles" OWNER TO postgres;

--
-- TOC entry 175 (class 1259 OID 18654)
-- Name: Roles_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "Roles_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "Roles_Id_seq" OWNER TO postgres;

--
-- TOC entry 3981 (class 0 OID 0)
-- Dependencies: 175
-- Name: Roles_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "Roles_Id_seq" OWNED BY "Roles"."Id";


--
-- TOC entry 293 (class 1259 OID 35875)
-- Name: Sessions; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "Sessions" (
    "Id" integer NOT NULL,
    "Token" text NOT NULL,
    "UpdatedAt" timestamp with time zone,
    "CreatedAt" timestamp with time zone,
    "LastSeen" timestamp with time zone
);


ALTER TABLE "Sessions" OWNER TO postgres;

--
-- TOC entry 292 (class 1259 OID 35873)
-- Name: Sessions_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "Sessions_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "Sessions_Id_seq" OWNER TO postgres;

--
-- TOC entry 3982 (class 0 OID 0)
-- Dependencies: 292
-- Name: Sessions_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "Sessions_Id_seq" OWNED BY "Sessions"."Id";


--
-- TOC entry 298 (class 1259 OID 35931)
-- Name: Transactions; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "Transactions" (
    "Id" integer NOT NULL,
    "TransactionType" integer NOT NULL,
    "TransactionAmount" numeric(18,2) NOT NULL,
    "TransactionCharge" numeric(18,2) DEFAULT 0 NOT NULL,
    "TransactedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "WalletId" integer NOT NULL,
    "SecondInvolvedWalletId" integer,
    "TransactionStatus" integer NOT NULL,
    "OrderId" integer
);


ALTER TABLE "Transactions" OWNER TO postgres;

--
-- TOC entry 299 (class 1259 OID 35934)
-- Name: Transactions_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "Transactions_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "Transactions_Id_seq" OWNER TO postgres;

--
-- TOC entry 3983 (class 0 OID 0)
-- Dependencies: 299
-- Name: Transactions_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "Transactions_Id_seq" OWNED BY "Transactions"."Id";


--
-- TOC entry 183 (class 1259 OID 18710)
-- Name: UserClaims; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "UserClaims" (
    "Id" integer NOT NULL,
    "UserId" integer DEFAULT 0 NOT NULL,
    "ClaimType" text,
    "ClaimValue" text
);


ALTER TABLE "UserClaims" OWNER TO postgres;

--
-- TOC entry 182 (class 1259 OID 18708)
-- Name: UserClaims_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "UserClaims_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "UserClaims_Id_seq" OWNER TO postgres;

--
-- TOC entry 3984 (class 0 OID 0)
-- Dependencies: 182
-- Name: UserClaims_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "UserClaims_Id_seq" OWNED BY "UserClaims"."Id";


--
-- TOC entry 184 (class 1259 OID 18720)
-- Name: UserLogins; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "UserLogins" (
    "LoginProvider" character varying(128) DEFAULT ''::character varying NOT NULL,
    "ProviderKey" character varying(128) DEFAULT ''::character varying NOT NULL,
    "UserId" integer DEFAULT 0 NOT NULL
);


ALTER TABLE "UserLogins" OWNER TO postgres;

--
-- TOC entry 177 (class 1259 OID 18663)
-- Name: UserRoles; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "UserRoles" (
    "UserId" integer DEFAULT 0 NOT NULL,
    "RoleId" integer DEFAULT 0 NOT NULL
);


ALTER TABLE "UserRoles" OWNER TO postgres;

--
-- TOC entry 295 (class 1259 OID 35888)
-- Name: UserSessions; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "UserSessions" (
    "Id" integer NOT NULL,
    "UserId" integer,
    "SessionId" integer
);


ALTER TABLE "UserSessions" OWNER TO postgres;

--
-- TOC entry 294 (class 1259 OID 35886)
-- Name: UserSessions_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "UserSessions_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "UserSessions_Id_seq" OWNER TO postgres;

--
-- TOC entry 3985 (class 0 OID 0)
-- Dependencies: 294
-- Name: UserSessions_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "UserSessions_Id_seq" OWNED BY "UserSessions"."Id";


--
-- TOC entry 179 (class 1259 OID 18672)
-- Name: Users; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "Users" (
    "Id" integer NOT NULL,
    "FirstName" text,
    "LastName" text,
    "Zipcode" text,
    "Birthday" timestamp without time zone,
    "OnfleetRecipientId" text,
    "CurrentIp" text,
    "LastIp" text,
    "SignInCount" integer,
    "PatientInfoId" integer,
    "AddressId" integer,
    "DeliveryAddressId" integer,
    "BillingAddressId" integer,
    "Email" character varying(256),
    "EmailConfirmed" boolean DEFAULT false NOT NULL,
    "PasswordHash" text,
    "SecurityStamp" text,
    "PhoneNumber" text,
    "PhoneNumberConfirmed" boolean DEFAULT false NOT NULL,
    "TwoFactorEnabled" boolean DEFAULT false NOT NULL,
    "LockoutEndDateUtc" timestamp without time zone,
    "LockoutEnabled" boolean DEFAULT false NOT NULL,
    "AccessFailedCount" integer DEFAULT 0 NOT NULL,
    "UserName" character varying(256),
    "Guid" text NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "LastLogin" timestamp with time zone DEFAULT now() NOT NULL,
    "WalletId" integer
);


ALTER TABLE "Users" OWNER TO postgres;

--
-- TOC entry 178 (class 1259 OID 18670)
-- Name: Users_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "Users_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "Users_Id_seq" OWNER TO postgres;

--
-- TOC entry 3986 (class 0 OID 0)
-- Dependencies: 178
-- Name: Users_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "Users_Id_seq" OWNED BY "Users"."Id";


--
-- TOC entry 297 (class 1259 OID 35914)
-- Name: Wallets; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "Wallets" (
    "Id" integer NOT NULL,
    "Balance" numeric(18,2) DEFAULT 0 NOT NULL,
    "Credit" numeric(18,2) DEFAULT 0 NOT NULL,
    "IsDeleted" boolean DEFAULT false NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now(),
    "UpdatedAt" timestamp with time zone DEFAULT now()
);


ALTER TABLE "Wallets" OWNER TO postgres;

--
-- TOC entry 296 (class 1259 OID 35912)
-- Name: Wallets_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "Wallets_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "Wallets_Id_seq" OWNER TO postgres;

--
-- TOC entry 3987 (class 0 OID 0)
-- Dependencies: 296
-- Name: Wallets_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "Wallets_Id_seq" OWNED BY "Wallets"."Id";


--
-- TOC entry 3592 (class 2604 OID 18696)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Addresses" ALTER COLUMN "Id" SET DEFAULT nextval('"Addresses_Id_seq"'::regclass);


--
-- TOC entry 3605 (class 2604 OID 18749)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Dispensaries" ALTER COLUMN "Id" SET DEFAULT nextval('"Dispensaries_Id_seq"'::regclass);


--
-- TOC entry 3657 (class 2604 OID 19261)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "DispensaryInvites" ALTER COLUMN "Id" SET DEFAULT nextval('"DispensaryInvites_Id_seq"'::regclass);


--
-- TOC entry 3631 (class 2604 OID 18806)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "DispensaryProductVariantOrders" ALTER COLUMN "Id" SET DEFAULT nextval('"DispensaryProductVariantOrders_Id_seq"'::regclass);


--
-- TOC entry 3623 (class 2604 OID 18791)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "DispensaryProductVariants" ALTER COLUMN "Id" SET DEFAULT nextval('"DispensaryProductVariants_Id_seq"'::regclass);


--
-- TOC entry 3615 (class 2604 OID 18773)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "DispensaryProducts" ALTER COLUMN "Id" SET DEFAULT nextval('"DispensaryProducts_Id_seq"'::regclass);


--
-- TOC entry 3654 (class 2604 OID 19215)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "DispensaryStaff" ALTER COLUMN "Id" SET DEFAULT nextval('"DispensaryStaff_Id_seq"'::regclass);


--
-- TOC entry 3666 (class 2604 OID 19341)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Effects" ALTER COLUMN "Id" SET DEFAULT nextval('"Effects_Id_seq"'::regclass);


--
-- TOC entry 3671 (class 2604 OID 19379)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Flavors" ALTER COLUMN "Id" SET DEFAULT nextval('"Flavors_Id_seq"'::regclass);


--
-- TOC entry 3661 (class 2604 OID 19306)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Invites" ALTER COLUMN "Id" SET DEFAULT nextval('"Invites_Id_seq"'::regclass);


--
-- TOC entry 3680 (class 2604 OID 19477)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "LabTestTypes" ALTER COLUMN "Id" SET DEFAULT nextval('"LabTestTypes_Id_seq"'::regclass);


--
-- TOC entry 3681 (class 2604 OID 23283)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "OAuthClients" ALTER COLUMN "Id" SET DEFAULT nextval('"OAuthClients_Id_seq"'::regclass);


--
-- TOC entry 3636 (class 2604 OID 18821)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Orders" ALTER COLUMN "Id" SET DEFAULT nextval('"Orders_Id_seq"'::regclass);


--
-- TOC entry 3601 (class 2604 OID 18733)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "PatientInfos" ALTER COLUMN "Id" SET DEFAULT nextval('"PatientInfos_Id_seq"'::regclass);


--
-- TOC entry 3649 (class 2604 OID 18855)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "ProductCategories" ALTER COLUMN "Id" SET DEFAULT nextval('"ProductCategories_Id_seq"'::regclass);


--
-- TOC entry 3672 (class 2604 OID 19390)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "ProductReviewEffects" ALTER COLUMN "Id" SET DEFAULT nextval('"ProductReviewEffects_Id_seq"'::regclass);


--
-- TOC entry 3676 (class 2604 OID 19408)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "ProductReviewFlavors" ALTER COLUMN "Id" SET DEFAULT nextval('"ProductReviewFlavors_Id_seq"'::regclass);


--
-- TOC entry 3693 (class 2604 OID 36090)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "ProductReviewProperties" ALTER COLUMN "Id" SET DEFAULT nextval('"ProductReviewProperties_Id_seq"'::regclass);


--
-- TOC entry 3667 (class 2604 OID 19352)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "ProductReviews" ALTER COLUMN "Id" SET DEFAULT nextval('"ProductReviews_Id_seq"'::regclass);


--
-- TOC entry 3644 (class 2604 OID 18840)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Products" ALTER COLUMN "Id" SET DEFAULT nextval('"Products_Id_seq"'::regclass);


--
-- TOC entry 3665 (class 2604 OID 19330)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Properties" ALTER COLUMN "Id" SET DEFAULT nextval('"Properties_Id_seq"'::regclass);


--
-- TOC entry 3579 (class 2604 OID 18659)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Roles" ALTER COLUMN "Id" SET DEFAULT nextval('"Roles_Id_seq"'::regclass);


--
-- TOC entry 3682 (class 2604 OID 35878)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Sessions" ALTER COLUMN "Id" SET DEFAULT nextval('"Sessions_Id_seq"'::regclass);


--
-- TOC entry 3690 (class 2604 OID 35936)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Transactions" ALTER COLUMN "Id" SET DEFAULT nextval('"Transactions_Id_seq"'::regclass);


--
-- TOC entry 3596 (class 2604 OID 18713)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "UserClaims" ALTER COLUMN "Id" SET DEFAULT nextval('"UserClaims_Id_seq"'::regclass);


--
-- TOC entry 3683 (class 2604 OID 35891)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "UserSessions" ALTER COLUMN "Id" SET DEFAULT nextval('"UserSessions_Id_seq"'::regclass);


--
-- TOC entry 3583 (class 2604 OID 18675)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Users" ALTER COLUMN "Id" SET DEFAULT nextval('"Users_Id_seq"'::regclass);


--
-- TOC entry 3684 (class 2604 OID 35917)
-- Name: Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Wallets" ALTER COLUMN "Id" SET DEFAULT nextval('"Wallets_Id_seq"'::regclass);


--
-- TOC entry 3757 (class 2606 OID 19266)
-- Name: DispensaryInvite_pk; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "DispensaryInvites"
    ADD CONSTRAINT "DispensaryInvite_pk" PRIMARY KEY ("Id");


--
-- TOC entry 3754 (class 2606 OID 19217)
-- Name: DispensaryStaff_PK; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "DispensaryStaff"
    ADD CONSTRAINT "DispensaryStaff_PK" PRIMARY KEY ("Id");


--
-- TOC entry 3767 (class 2606 OID 19346)
-- Name: Effects_pk; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Effects"
    ADD CONSTRAINT "Effects_pk" PRIMARY KEY ("Id");


--
-- TOC entry 3774 (class 2606 OID 19384)
-- Name: Flavors_pk; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Flavors"
    ADD CONSTRAINT "Flavors_pk" PRIMARY KEY ("Id");


--
-- TOC entry 3763 (class 2606 OID 19314)
-- Name: Invites_pk; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Invites"
    ADD CONSTRAINT "Invites_pk" PRIMARY KEY ("Id");


--
-- TOC entry 3784 (class 2606 OID 19482)
-- Name: LabTestTypes_pk; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "LabTestTypes"
    ADD CONSTRAINT "LabTestTypes_pk" PRIMARY KEY ("Id");


--
-- TOC entry 3786 (class 2606 OID 23288)
-- Name: OAuthClients_pk; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "OAuthClients"
    ADD CONSTRAINT "OAuthClients_pk" PRIMARY KEY ("Id");


--
-- TOC entry 3718 (class 2606 OID 18707)
-- Name: PK_dbo.Addresses; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Addresses"
    ADD CONSTRAINT "PK_dbo.Addresses" PRIMARY KEY ("Id");


--
-- TOC entry 3730 (class 2606 OID 18767)
-- Name: PK_dbo.Dispensaries; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Dispensaries"
    ADD CONSTRAINT "PK_dbo.Dispensaries" PRIMARY KEY ("Id");


--
-- TOC entry 3741 (class 2606 OID 18815)
-- Name: PK_dbo.DispensaryProductVariantOrders; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "DispensaryProductVariantOrders"
    ADD CONSTRAINT "PK_dbo.DispensaryProductVariantOrders" PRIMARY KEY ("Id");


--
-- TOC entry 3737 (class 2606 OID 18800)
-- Name: PK_dbo.DispensaryProductVariants; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "DispensaryProductVariants"
    ADD CONSTRAINT "PK_dbo.DispensaryProductVariants" PRIMARY KEY ("Id");


--
-- TOC entry 3734 (class 2606 OID 18785)
-- Name: PK_dbo.DispensaryProducts; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "DispensaryProducts"
    ADD CONSTRAINT "PK_dbo.DispensaryProducts" PRIMARY KEY ("Id");


--
-- TOC entry 3746 (class 2606 OID 18834)
-- Name: PK_dbo.Orders; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Orders"
    ADD CONSTRAINT "PK_dbo.Orders" PRIMARY KEY ("Id");


--
-- TOC entry 3726 (class 2606 OID 18743)
-- Name: PK_dbo.PatientInfos; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "PatientInfos"
    ADD CONSTRAINT "PK_dbo.PatientInfos" PRIMARY KEY ("Id");


--
-- TOC entry 3751 (class 2606 OID 18863)
-- Name: PK_dbo.ProductCategories; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "ProductCategories"
    ADD CONSTRAINT "PK_dbo.ProductCategories" PRIMARY KEY ("Id");


--
-- TOC entry 3748 (class 2606 OID 18849)
-- Name: PK_dbo.Products; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Products"
    ADD CONSTRAINT "PK_dbo.Products" PRIMARY KEY ("Id");


--
-- TOC entry 3698 (class 2606 OID 18662)
-- Name: PK_dbo.Roles; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Roles"
    ADD CONSTRAINT "PK_dbo.Roles" PRIMARY KEY ("Id");


--
-- TOC entry 3720 (class 2606 OID 18719)
-- Name: PK_dbo.UserClaims; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "UserClaims"
    ADD CONSTRAINT "PK_dbo.UserClaims" PRIMARY KEY ("Id");


--
-- TOC entry 3723 (class 2606 OID 18727)
-- Name: PK_dbo.UserLogins; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "UserLogins"
    ADD CONSTRAINT "PK_dbo.UserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey", "UserId");


--
-- TOC entry 3701 (class 2606 OID 18669)
-- Name: PK_dbo.UserRoles; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "UserRoles"
    ADD CONSTRAINT "PK_dbo.UserRoles" PRIMARY KEY ("UserId", "RoleId");


--
-- TOC entry 3705 (class 2606 OID 18690)
-- Name: PK_dbo.Users; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Users"
    ADD CONSTRAINT "PK_dbo.Users" PRIMARY KEY ("Id");


--
-- TOC entry 3778 (class 2606 OID 19392)
-- Name: ProductReviewEffects_pk; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "ProductReviewEffects"
    ADD CONSTRAINT "ProductReviewEffects_pk" PRIMARY KEY ("Id");


--
-- TOC entry 3782 (class 2606 OID 19413)
-- Name: ProductReviewFlavors_pk; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "ProductReviewFlavors"
    ADD CONSTRAINT "ProductReviewFlavors_pk" PRIMARY KEY ("Id");


--
-- TOC entry 3799 (class 2606 OID 36092)
-- Name: ProductReviewProperties_pk; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "ProductReviewProperties"
    ADD CONSTRAINT "ProductReviewProperties_pk" PRIMARY KEY ("Id");


--
-- TOC entry 3765 (class 2606 OID 19335)
-- Name: Properties_PK; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Properties"
    ADD CONSTRAINT "Properties_PK" PRIMARY KEY ("Id");


--
-- TOC entry 3772 (class 2606 OID 19357)
-- Name: Review_pk; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "ProductReviews"
    ADD CONSTRAINT "Review_pk" PRIMARY KEY ("Id");


--
-- TOC entry 3788 (class 2606 OID 35885)
-- Name: Session_Token_Unique; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Sessions"
    ADD CONSTRAINT "Session_Token_Unique" UNIQUE ("Token");


--
-- TOC entry 3790 (class 2606 OID 35883)
-- Name: Session_pk; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Sessions"
    ADD CONSTRAINT "Session_pk" PRIMARY KEY ("Id");


--
-- TOC entry 3796 (class 2606 OID 35949)
-- Name: Transaction_pk; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Transactions"
    ADD CONSTRAINT "Transaction_pk" PRIMARY KEY ("Id");


--
-- TOC entry 3792 (class 2606 OID 35893)
-- Name: UserSession_pk; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "UserSessions"
    ADD CONSTRAINT "UserSession_pk" PRIMARY KEY ("Id");


--
-- TOC entry 3711 (class 2606 OID 27782)
-- Name: Users_Unique_Email; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Users"
    ADD CONSTRAINT "Users_Unique_Email" UNIQUE ("Email");


--
-- TOC entry 3713 (class 2606 OID 27780)
-- Name: Users_Unique_Guid; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Users"
    ADD CONSTRAINT "Users_Unique_Guid" UNIQUE ("Guid");


--
-- TOC entry 3794 (class 2606 OID 35924)
-- Name: Wallet_pk; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Wallets"
    ADD CONSTRAINT "Wallet_pk" PRIMARY KEY ("Id");


--
-- TOC entry 3716 (class 1259 OID 23263)
-- Name: Addresses_Coordinates_IX; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "Addresses_Coordinates_IX" ON "Addresses" USING gist ("Coordinates");

ALTER TABLE "Addresses" CLUSTER ON "Addresses_Coordinates_IX";


--
-- TOC entry 3727 (class 1259 OID 23264)
-- Name: Dispensaries_Coordinates_IX; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "Dispensaries_Coordinates_IX" ON "Dispensaries" USING gist ("Coordinates");

ALTER TABLE "Dispensaries" CLUSTER ON "Dispensaries_Coordinates_IX";


--
-- TOC entry 3728 (class 1259 OID 18874)
-- Name: Dispensaries_IX_AddressId; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "Dispensaries_IX_AddressId" ON "Dispensaries" USING btree ("AddressId");


--
-- TOC entry 3758 (class 1259 OID 23265)
-- Name: DispensaryInvites_Dispensary_IX; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "DispensaryInvites_Dispensary_IX" ON "DispensaryInvites" USING btree ("DispensaryId");


--
-- TOC entry 3759 (class 1259 OID 23266)
-- Name: DispensaryInvites_User_IX; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "DispensaryInvites_User_IX" ON "DispensaryInvites" USING btree ("UserId");


--
-- TOC entry 3738 (class 1259 OID 18879)
-- Name: DispensaryProductVariantOrders_IX_DispensaryProductVariant_Id; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "DispensaryProductVariantOrders_IX_DispensaryProductVariant_Id" ON "DispensaryProductVariantOrders" USING btree ("DispensaryProductVariantId");


--
-- TOC entry 3739 (class 1259 OID 18878)
-- Name: DispensaryProductVariantOrders_IX_OrderId; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "DispensaryProductVariantOrders_IX_OrderId" ON "DispensaryProductVariantOrders" USING btree ("OrderId");


--
-- TOC entry 3735 (class 1259 OID 18877)
-- Name: DispensaryProductVariants_IX_DispensaryProductId; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "DispensaryProductVariants_IX_DispensaryProductId" ON "DispensaryProductVariants" USING btree ("DispensaryProductId");


--
-- TOC entry 3731 (class 1259 OID 18876)
-- Name: DispensaryProducts_IX_Dispensary_Id; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "DispensaryProducts_IX_Dispensary_Id" ON "DispensaryProducts" USING btree ("DispensaryId");


--
-- TOC entry 3732 (class 1259 OID 18875)
-- Name: DispensaryProducts_IX_ProductId; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "DispensaryProducts_IX_ProductId" ON "DispensaryProducts" USING btree ("ProductId");


--
-- TOC entry 3752 (class 1259 OID 23268)
-- Name: DispensaryStaff_Dispensary_IX; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "DispensaryStaff_Dispensary_IX" ON "DispensaryStaff" USING btree ("DispensaryId");


--
-- TOC entry 3755 (class 1259 OID 23267)
-- Name: DispensaryStaff_User_IX; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "DispensaryStaff_User_IX" ON "DispensaryStaff" USING btree ("UserId");


--
-- TOC entry 3760 (class 1259 OID 23270)
-- Name: Invites_Invitee_IX; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "Invites_Invitee_IX" ON "Invites" USING btree ("InviteeId");


--
-- TOC entry 3761 (class 1259 OID 23269)
-- Name: Invites_Inviter_IX; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "Invites_Inviter_IX" ON "Invites" USING btree ("InviterId");


--
-- TOC entry 3742 (class 1259 OID 18880)
-- Name: Orders_IX_BillingAddressId; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "Orders_IX_BillingAddressId" ON "Orders" USING btree ("BillingAddressId");


--
-- TOC entry 3743 (class 1259 OID 18881)
-- Name: Orders_IX_DeliveryAddressId; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "Orders_IX_DeliveryAddressId" ON "Orders" USING btree ("DeliveryAddressId");


--
-- TOC entry 3744 (class 1259 OID 18882)
-- Name: Orders_IX_UserId; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "Orders_IX_UserId" ON "Orders" USING btree ("UserId");


--
-- TOC entry 3775 (class 1259 OID 23272)
-- Name: ProductReviewEffects_Effects_IX; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "ProductReviewEffects_Effects_IX" ON "ProductReviewEffects" USING btree ("EffectId");


--
-- TOC entry 3776 (class 1259 OID 23271)
-- Name: ProductReviewEffects_ProductReview_IX; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "ProductReviewEffects_ProductReview_IX" ON "ProductReviewEffects" USING btree ("ProductReviewId");


--
-- TOC entry 3779 (class 1259 OID 23273)
-- Name: ProductReviewFlavors_Flavors_IX; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "ProductReviewFlavors_Flavors_IX" ON "ProductReviewFlavors" USING btree ("FlavorId");


--
-- TOC entry 3780 (class 1259 OID 23274)
-- Name: ProductReviewFlavors_ProductReview_IX; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "ProductReviewFlavors_ProductReview_IX" ON "ProductReviewFlavors" USING btree ("ProductReviewId");


--
-- TOC entry 3768 (class 1259 OID 23276)
-- Name: ProductReviews_DispensaryProduct_IX; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "ProductReviews_DispensaryProduct_IX" ON "ProductReviews" USING btree ("DispensaryProductId");


--
-- TOC entry 3769 (class 1259 OID 23277)
-- Name: ProductReviews_Dispensary_IX; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "ProductReviews_Dispensary_IX" ON "ProductReviews" USING btree ("PurchsedFromDispensaryId");


--
-- TOC entry 3770 (class 1259 OID 23275)
-- Name: ProductReviews_Product_IX; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "ProductReviews_Product_IX" ON "ProductReviews" USING btree ("ProductId");


--
-- TOC entry 3749 (class 1259 OID 18883)
-- Name: Products_IX_ProductCategoryId; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "Products_IX_ProductCategoryId" ON "Products" USING btree ("ProductCategoryId");


--
-- TOC entry 3699 (class 1259 OID 18864)
-- Name: Roles_RoleNameIndex; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE UNIQUE INDEX "Roles_RoleNameIndex" ON "Roles" USING btree ("Name");


--
-- TOC entry 3721 (class 1259 OID 18872)
-- Name: UserClaims_IX_UserId; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "UserClaims_IX_UserId" ON "UserClaims" USING btree ("UserId");


--
-- TOC entry 3724 (class 1259 OID 18873)
-- Name: UserLogins_IX_UserId; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "UserLogins_IX_UserId" ON "UserLogins" USING btree ("UserId");


--
-- TOC entry 3702 (class 1259 OID 18866)
-- Name: UserRoles_IX_RoleId; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "UserRoles_IX_RoleId" ON "UserRoles" USING btree ("RoleId");


--
-- TOC entry 3703 (class 1259 OID 18865)
-- Name: UserRoles_IX_UserId; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "UserRoles_IX_UserId" ON "UserRoles" USING btree ("UserId");


--
-- TOC entry 3706 (class 1259 OID 18868)
-- Name: Users_IX_AddressId; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "Users_IX_AddressId" ON "Users" USING btree ("AddressId");


--
-- TOC entry 3707 (class 1259 OID 18870)
-- Name: Users_IX_BillingAddressId; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "Users_IX_BillingAddressId" ON "Users" USING btree ("BillingAddressId");


--
-- TOC entry 3708 (class 1259 OID 18869)
-- Name: Users_IX_DeliveryAddressId; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "Users_IX_DeliveryAddressId" ON "Users" USING btree ("DeliveryAddressId");


--
-- TOC entry 3709 (class 1259 OID 18867)
-- Name: Users_IX_PatientInfoId; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "Users_IX_PatientInfoId" ON "Users" USING btree ("PatientInfoId");


--
-- TOC entry 3714 (class 1259 OID 18871)
-- Name: Users_UserNameIndex; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE UNIQUE INDEX "Users_UserNameIndex" ON "Users" USING btree ("UserName");


--
-- TOC entry 3797 (class 1259 OID 35965)
-- Name: fki_Transaction_Order_FK; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "fki_Transaction_Order_FK" ON "Transactions" USING btree ("OrderId");


--
-- TOC entry 3715 (class 1259 OID 35930)
-- Name: fki_User_wallet_fk; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX "fki_User_wallet_fk" ON "Users" USING btree ("WalletId");


--
-- TOC entry 3821 (class 2606 OID 19272)
-- Name: DispensaryInvite_Dispensary_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "DispensaryInvites"
    ADD CONSTRAINT "DispensaryInvite_Dispensary_fk" FOREIGN KEY ("DispensaryId") REFERENCES "Dispensaries"("Id");


--
-- TOC entry 3822 (class 2606 OID 19267)
-- Name: DispensaryInvite_User_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "DispensaryInvites"
    ADD CONSTRAINT "DispensaryInvite_User_fk" FOREIGN KEY ("UserId") REFERENCES "Users"("Id");


--
-- TOC entry 3820 (class 2606 OID 19218)
-- Name: DispensaryStaff_Dispensary_FK; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "DispensaryStaff"
    ADD CONSTRAINT "DispensaryStaff_Dispensary_FK" FOREIGN KEY ("DispensaryId") REFERENCES "Dispensaries"("Id");


--
-- TOC entry 3819 (class 2606 OID 19223)
-- Name: DispensaryStaff_User_FK; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "DispensaryStaff"
    ADD CONSTRAINT "DispensaryStaff_User_FK" FOREIGN KEY ("UserId") REFERENCES "Users"("Id");


--
-- TOC entry 3809 (class 2606 OID 18988)
-- Name: FK_dbo.Dispensaries_dbo.Addresses_AddressId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Dispensaries"
    ADD CONSTRAINT "FK_dbo.Dispensaries_dbo.Addresses_AddressId" FOREIGN KEY ("AddressId") REFERENCES "Addresses"("Id");


--
-- TOC entry 3814 (class 2606 OID 18944)
-- Name: FK_dbo.DispensaryProductVariantOrders_dbo.DispensaryProductVari; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "DispensaryProductVariantOrders"
    ADD CONSTRAINT "FK_dbo.DispensaryProductVariantOrders_dbo.DispensaryProductVari" FOREIGN KEY ("DispensaryProductVariantId") REFERENCES "DispensaryProductVariants"("Id");


--
-- TOC entry 3813 (class 2606 OID 19107)
-- Name: FK_dbo.DispensaryProductVariantOrders_dbo.Orders_OrderId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "DispensaryProductVariantOrders"
    ADD CONSTRAINT "FK_dbo.DispensaryProductVariantOrders_dbo.Orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Orders"("Id");


--
-- TOC entry 3812 (class 2606 OID 18999)
-- Name: FK_dbo.DispensaryProductVariants_dbo.DispensaryProducts_Dispens; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "DispensaryProductVariants"
    ADD CONSTRAINT "FK_dbo.DispensaryProductVariants_dbo.DispensaryProducts_Dispens" FOREIGN KEY ("DispensaryProductId") REFERENCES "DispensaryProducts"("Id");


--
-- TOC entry 3811 (class 2606 OID 18929)
-- Name: FK_dbo.DispensaryProducts_dbo.Dispensaries_Dispensary_Id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "DispensaryProducts"
    ADD CONSTRAINT "FK_dbo.DispensaryProducts_dbo.Dispensaries_Dispensary_Id" FOREIGN KEY ("DispensaryId") REFERENCES "Dispensaries"("Id");


--
-- TOC entry 3810 (class 2606 OID 18994)
-- Name: FK_dbo.DispensaryProducts_dbo.Products_ProductId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "DispensaryProducts"
    ADD CONSTRAINT "FK_dbo.DispensaryProducts_dbo.Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products"("Id");


--
-- TOC entry 3817 (class 2606 OID 18954)
-- Name: FK_dbo.Orders_dbo.Addresses_BillingAddressId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Orders"
    ADD CONSTRAINT "FK_dbo.Orders_dbo.Addresses_BillingAddressId" FOREIGN KEY ("BillingAddressId") REFERENCES "Addresses"("Id");


--
-- TOC entry 3816 (class 2606 OID 18959)
-- Name: FK_dbo.Orders_dbo.Addresses_DeliveryAddressId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Orders"
    ADD CONSTRAINT "FK_dbo.Orders_dbo.Addresses_DeliveryAddressId" FOREIGN KEY ("DeliveryAddressId") REFERENCES "Addresses"("Id");


--
-- TOC entry 3815 (class 2606 OID 18964)
-- Name: FK_dbo.Orders_dbo.Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Orders"
    ADD CONSTRAINT "FK_dbo.Orders_dbo.Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE;


--
-- TOC entry 3818 (class 2606 OID 19484)
-- Name: FK_dbo.Products_dbo.ProductCategories_ProductCategoryId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Products"
    ADD CONSTRAINT "FK_dbo.Products_dbo.ProductCategories_ProductCategoryId" FOREIGN KEY ("ProductCategoryId") REFERENCES "ProductCategories"("Id");


--
-- TOC entry 3807 (class 2606 OID 18914)
-- Name: FK_dbo.UserClaims_dbo.Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "UserClaims"
    ADD CONSTRAINT "FK_dbo.UserClaims_dbo.Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE;


--
-- TOC entry 3808 (class 2606 OID 18919)
-- Name: FK_dbo.UserLogins_dbo.Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "UserLogins"
    ADD CONSTRAINT "FK_dbo.UserLogins_dbo.Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE;


--
-- TOC entry 3801 (class 2606 OID 18884)
-- Name: FK_dbo.UserRoles_dbo.Roles_RoleId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "UserRoles"
    ADD CONSTRAINT "FK_dbo.UserRoles_dbo.Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "Roles"("Id") ON DELETE CASCADE;


--
-- TOC entry 3800 (class 2606 OID 18889)
-- Name: FK_dbo.UserRoles_dbo.Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "UserRoles"
    ADD CONSTRAINT "FK_dbo.UserRoles_dbo.Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE;


--
-- TOC entry 3806 (class 2606 OID 18894)
-- Name: FK_dbo.Users_dbo.Addresses_AddressId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Users"
    ADD CONSTRAINT "FK_dbo.Users_dbo.Addresses_AddressId" FOREIGN KEY ("AddressId") REFERENCES "Addresses"("Id");


--
-- TOC entry 3805 (class 2606 OID 18899)
-- Name: FK_dbo.Users_dbo.Addresses_BillingAddressId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Users"
    ADD CONSTRAINT "FK_dbo.Users_dbo.Addresses_BillingAddressId" FOREIGN KEY ("BillingAddressId") REFERENCES "Addresses"("Id");


--
-- TOC entry 3804 (class 2606 OID 18904)
-- Name: FK_dbo.Users_dbo.Addresses_DeliveryAddressId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Users"
    ADD CONSTRAINT "FK_dbo.Users_dbo.Addresses_DeliveryAddressId" FOREIGN KEY ("DeliveryAddressId") REFERENCES "Addresses"("Id");


--
-- TOC entry 3803 (class 2606 OID 18909)
-- Name: FK_dbo.Users_dbo.PatientInfos_PatientInfoId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Users"
    ADD CONSTRAINT "FK_dbo.Users_dbo.PatientInfos_PatientInfoId" FOREIGN KEY ("PatientInfoId") REFERENCES "PatientInfos"("Id");


--
-- TOC entry 3824 (class 2606 OID 19315)
-- Name: Invites_Invitee_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Invites"
    ADD CONSTRAINT "Invites_Invitee_fk" FOREIGN KEY ("InviteeId") REFERENCES "Users"("Id");


--
-- TOC entry 3823 (class 2606 OID 19320)
-- Name: Invites_Inviter_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Invites"
    ADD CONSTRAINT "Invites_Inviter_fk" FOREIGN KEY ("InviterId") REFERENCES "Users"("Id");


--
-- TOC entry 3829 (class 2606 OID 19398)
-- Name: ProductReviewEffect_Effect_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "ProductReviewEffects"
    ADD CONSTRAINT "ProductReviewEffect_Effect_fk" FOREIGN KEY ("EffectId") REFERENCES "Effects"("Id");


--
-- TOC entry 3830 (class 2606 OID 19393)
-- Name: ProductReviewEffects_review_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "ProductReviewEffects"
    ADD CONSTRAINT "ProductReviewEffects_review_fk" FOREIGN KEY ("ProductReviewId") REFERENCES "ProductReviews"("Id");


--
-- TOC entry 3831 (class 2606 OID 19419)
-- Name: ProductReviewFlavors_Flavor_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "ProductReviewFlavors"
    ADD CONSTRAINT "ProductReviewFlavors_Flavor_fk" FOREIGN KEY ("FlavorId") REFERENCES "Flavors"("Id");


--
-- TOC entry 3832 (class 2606 OID 19414)
-- Name: ProductReviewFlavors_ProductReview_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "ProductReviewFlavors"
    ADD CONSTRAINT "ProductReviewFlavors_ProductReview_fk" FOREIGN KEY ("ProductReviewId") REFERENCES "ProductReviews"("Id");


--
-- TOC entry 3839 (class 2606 OID 36093)
-- Name: ProductReviewProperties_Property_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "ProductReviewProperties"
    ADD CONSTRAINT "ProductReviewProperties_Property_fk" FOREIGN KEY ("PropertyId") REFERENCES "Properties"("Id");


--
-- TOC entry 3838 (class 2606 OID 36098)
-- Name: ProductReviewProperties_productreview_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "ProductReviewProperties"
    ADD CONSTRAINT "ProductReviewProperties_productreview_fk" FOREIGN KEY ("ProductReviewId") REFERENCES "ProductReviews"("Id");


--
-- TOC entry 3827 (class 2606 OID 19363)
-- Name: Review_DispensaryProduct_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "ProductReviews"
    ADD CONSTRAINT "Review_DispensaryProduct_fk" FOREIGN KEY ("DispensaryProductId") REFERENCES "DispensaryProducts"("Id");


--
-- TOC entry 3826 (class 2606 OID 19368)
-- Name: Review_Product_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "ProductReviews"
    ADD CONSTRAINT "Review_Product_fk" FOREIGN KEY ("ProductId") REFERENCES "Products"("Id");


--
-- TOC entry 3828 (class 2606 OID 19358)
-- Name: Review_PurchasedFromDispensary_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "ProductReviews"
    ADD CONSTRAINT "Review_PurchasedFromDispensary_fk" FOREIGN KEY ("PurchsedFromDispensaryId") REFERENCES "Dispensaries"("Id");


--
-- TOC entry 3825 (class 2606 OID 36118)
-- Name: Review_User_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "ProductReviews"
    ADD CONSTRAINT "Review_User_fk" FOREIGN KEY ("UserId") REFERENCES "Users"("Id");


--
-- TOC entry 3835 (class 2606 OID 35960)
-- Name: Transaction_Order_FK; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Transactions"
    ADD CONSTRAINT "Transaction_Order_FK" FOREIGN KEY ("OrderId") REFERENCES "Orders"("Id");


--
-- TOC entry 3836 (class 2606 OID 35955)
-- Name: Transaction_SecondWallet_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Transactions"
    ADD CONSTRAINT "Transaction_SecondWallet_fk" FOREIGN KEY ("SecondInvolvedWalletId") REFERENCES "Wallets"("Id");


--
-- TOC entry 3837 (class 2606 OID 35950)
-- Name: Transaction_wallet_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Transactions"
    ADD CONSTRAINT "Transaction_wallet_fk" FOREIGN KEY ("WalletId") REFERENCES "Wallets"("Id");


--
-- TOC entry 3833 (class 2606 OID 35899)
-- Name: UserSession_Session_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "UserSessions"
    ADD CONSTRAINT "UserSession_Session_fk" FOREIGN KEY ("SessionId") REFERENCES "Sessions"("Id") ON DELETE CASCADE;


--
-- TOC entry 3834 (class 2606 OID 35894)
-- Name: UserSession_User_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "UserSessions"
    ADD CONSTRAINT "UserSession_User_fk" FOREIGN KEY ("UserId") REFERENCES "Users"("Id");


--
-- TOC entry 3802 (class 2606 OID 35925)
-- Name: User_wallet_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Users"
    ADD CONSTRAINT "User_wallet_fk" FOREIGN KEY ("WalletId") REFERENCES "Wallets"("Id");


-- Completed on 2015-09-17 07:25:27

--
-- PostgreSQL database dump complete
--

