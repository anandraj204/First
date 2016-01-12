using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AskMJane.LeaflyScraper.Logging;
using AskMJane.LeaflyScraper.Models;
using Npgsql;
using System.Data.SqlClient;

namespace AskMJane.LeaflyScraper.WorkerNodes
{
    public class DataNode
    {
        public Logger _logger;
        public string _connString;
        public DataNode()
        {
            _logger = new Logger();
            _connString =
                @"Data Source=54.175.189.188;Initial Catalog=HARVESTGEEK_TEST;Integrated Security=False;User Id=harvestgeek;Password=harvestgeek;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True";
        }

        public async Task<bool> PopulateDatabase(LeaflyReview r)
        {
            try
            {
                using (var conn = new SqlConnection(_connString))
                {
                    conn.Open();
                    int productId = 0;
                    int categoryId = 0;
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        //cmd.CommandText = "Select * from \"Products\"";
                        cmd.CommandText = String.Format("Select \"Id\" from \"Products\" where \"Name\" = '{0}'",
                            r.StrainDisplayName);

                        var nullablProductId = cmd.ExecuteScalar();
                        if (nullablProductId != null)
                        {
                            productId = int.Parse(nullablProductId.ToString());
                        }
                    }
                    if (productId == 0)
                    {
                        using (var findCategoryCmd = new SqlCommand())
                        {
                            findCategoryCmd.Connection = conn;
                            findCategoryCmd.CommandText = String.Format("Select \"Id\" from \"ProductCategories\" where \"Name\" = '{0}'", r.Category);
                            var nullableCategoryId = findCategoryCmd.ExecuteScalar();
                            if (nullableCategoryId != null)
                            {
                                categoryId = int.Parse(nullableCategoryId.ToString());
                            }

                        }
                        if (categoryId == 0)
                        {
                            using (var insertCategoryCmd = new SqlCommand())
                            {
                                insertCategoryCmd.Connection = conn;
                                insertCategoryCmd.CommandText =
                                    String.Format(
                                        "Insert into \"ProductCategories\" (\"Name\") values ('{0}'); SELECT SCOPE_IDENTITY();",
                                        r.Category);

                                var nullableCategoryId = insertCategoryCmd.ExecuteScalar();
                                if (nullableCategoryId != null)
                                {
                                    categoryId = int.Parse(nullableCategoryId.ToString());
                                }
                            }
                        }
                        if (categoryId == 0)
                        {
                            _logger.ErrorFormat("Could not insert Category {0}", r.Category);
                            return false;
                        }
                        using (var insertProductCmd = new SqlCommand())
                        {
                            insertProductCmd.Connection = conn;
                            insertProductCmd.CommandText =
                                String.Format(
                                    "Insert into \"Products\" (\"Name\",\"Slug\",\"LeaflySlug\",\"ProductCategoryId\") values('{0}','{1}','{2}',{3}); SELECT SCOPE_IDENTITY();",
                                    r.StrainDisplayName, r.StrainDisplayName, r.LeaflySlug, categoryId);
                            var nullableProductId = insertProductCmd.ExecuteScalar();
                            if (nullableProductId != null)
                            {
                                productId = int.Parse(nullableProductId.ToString());
                            }
                        }
                        if (productId == 0)
                        {
                            _logger.ErrorFormat("Could not insert Product {0}", r.StrainDisplayName);
                            return false;
                        }
                    }
                    // Create flavors
                    List<int> flavorIds = new List<int>();
                    foreach (var f in r.Flavors)
                    {
                        int flavorid = 0;
                        using (var selectFlavorCmd = new SqlCommand())
                        {

                            selectFlavorCmd.Connection = conn;
                            selectFlavorCmd.CommandText = String.Format("Select \"Id\" from \"Flavors\"  where \"Name\" = '{0}'", f);
                            var nullableFlavorId = selectFlavorCmd.ExecuteScalar();
                            if (nullableFlavorId != null)
                            {
                                flavorid = int.Parse(nullableFlavorId.ToString());
                            }
                        }
                        if (flavorid == 0)
                        {
                            using (var insertFlavorCmd = new SqlCommand())
                            {
                                insertFlavorCmd.Connection = conn;
                                insertFlavorCmd.CommandText = String.Format("Insert into \"Flavors\" (\"Name\") values('{0}'); SELECT SCOPE_IDENTITY();", f);

                                var nullableFlavorId = insertFlavorCmd.ExecuteScalar();
                                if (nullableFlavorId != null)
                                {
                                    flavorid = int.Parse(nullableFlavorId.ToString());
                                }
                            }
                        }
                        if (flavorid > 0)
                        {
                            flavorIds.Add(flavorid);
                        }
                    }
                    // Create forms and methods
                    List<int> formIds = new List<int>();
                    foreach (var f in r.Forms)
                    {
                        int formid = 0;
                        using (var selectFormCmd = new SqlCommand())
                        {

                            selectFormCmd.Connection = conn;
                            selectFormCmd.CommandText = String.Format("Select \"Id\" from \"Properties\"  where \"Name\" = '{0}'", f);

                            var nullableFormId = selectFormCmd.ExecuteScalar();
                            if (nullableFormId != null)
                            {
                                formid = int.Parse(nullableFormId.ToString());
                            }
                        }
                        if (formid == 0)
                        {
                            using (var insertFormCmd = new SqlCommand())
                            {
                                insertFormCmd.Connection = conn;
                                insertFormCmd.CommandText = String.Format("Insert into \"Properties\" (\"Name\",\"Type\") values('{0}','{1}'); SELECT SCOPE_IDENTITY();", f, f);
                                var nullableFormId = insertFormCmd.ExecuteScalar();
                                if (nullableFormId != null)
                                {
                                    formid = int.Parse(nullableFormId.ToString());
                                }
                            }
                        }
                        if (formid > 0)
                        {
                            formIds.Add(formid);
                        }
                    }
                    // Create Effects
                    List<int> effectIds = new List<int>();
                    foreach (var f in r.Effects)
                    {
                        int effectid = 0;
                        using (var selectEffectCmd = new SqlCommand())
                        {

                            selectEffectCmd.Connection = conn;
                            selectEffectCmd.CommandText = String.Format("Select \"Id\" from \"Effects\"  where \"Name\" = '{0}'", f);


                            var nullableEffectId = selectEffectCmd.ExecuteScalar();
                            if (nullableEffectId != null)
                            {
                                effectid = int.Parse(nullableEffectId.ToString());
                            }
                        }
                        if (effectid == 0)
                        {
                            using (var insertEffectCmd = new SqlCommand())
                            {
                                insertEffectCmd.Connection = conn;
                                insertEffectCmd.CommandText = String.Format("Insert into \"Effects\" (\"Name\",\"Type\") values('{0}','{1}'); SELECT SCOPE_IDENTITY();", f, f);
                                var nullableEffectId = insertEffectCmd.ExecuteScalar();
                                if (nullableEffectId != null)
                                {
                                    effectid = int.Parse(nullableEffectId.ToString());
                                }
                            }
                        }
                        if (effectid > 0)
                        {
                            effectIds.Add(effectid);
                        }
                    }
                    // Create Review
                    int reviewid = 0;
                    using (var insertReviewCmd = new SqlCommand())
                    {
                        insertReviewCmd.Connection = conn;
                        insertReviewCmd.CommandText =
                            String.Format(
                                "Insert into \"ProductReviews\" (\"Review\", \"Rating\", \"ProductId\", \"ReviewedType\") values('{0}', {1}, {2}, {3}); SELECT SCOPE_IDENTITY();",
                                r.Review, r.Rating, productId, 0);
                        var nullableReviewId = insertReviewCmd.ExecuteScalar();
                        if (nullableReviewId != null)
                        {
                            reviewid = int.Parse(nullableReviewId.ToString());
                        }
                    }
                    // Link Review to flavors, forms, effects
                    if (reviewid == 0)
                    {
                        _logger.ErrorFormat("Could not insert Review {0}", r.Review);
                        return false;
                    }
                    foreach (var f in flavorIds)
                    {
                        using (var insertLinkCmd = new SqlCommand())
                        {
                            insertLinkCmd.Connection = conn;
                            insertLinkCmd.CommandText =
                                String.Format(
                                    "Insert into \"ProductReviewFlavors\" (\"ProductReviewId\", \"FlavorId\") values({0}, {1}); SELECT SCOPE_IDENTITY();",
                                    reviewid, f);
                            insertLinkCmd.ExecuteScalar();
                        }
                    }
                    foreach (var f in formIds)
                    {
                        using (var insertLinkCmd = new SqlCommand())
                        {
                            insertLinkCmd.Connection = conn;
                            insertLinkCmd.CommandText =
                                String.Format(
                                    "Insert into \"ProductReviewProperties\" (\"ProductReviewId\", \"PropertyId\") values({0}, {1}) ; SELECT SCOPE_IDENTITY();",
                                    reviewid, f);
                            insertLinkCmd.ExecuteScalar();
                        }
                    }

                    foreach (var f in effectIds)
                    {
                        using (var insertLinkCmd = new SqlCommand())
                        {
                            insertLinkCmd.Connection = conn;
                            insertLinkCmd.CommandText =
                                String.Format(
                                    "Insert into \"ProductReviewEffects\" (\"ProductReviewId\", \"EffectId\") values({0}, {1}) ; SELECT SCOPE_IDENTITY();",
                                    reviewid, f);
                            insertLinkCmd.ExecuteScalar();
                        }
                    }


                }

                return true;
            }
            catch (Exception e)
            {
                _logger.Error("Populate DB", e);
                return false;
            }

        }
    }
}
