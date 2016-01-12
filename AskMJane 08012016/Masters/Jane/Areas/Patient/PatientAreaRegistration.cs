using System.Web.Mvc;

namespace Jane.Web.Areas.Patient
{
    public class PatientAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Patient";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {

            context.MapRoute("patient_applicationv2", "patient/applyv2", new { controller = "Patient", action = "ApplyV2" });
            context.MapRoute("patient_application", "patient/apply", new { controller = "Patient", action = "Apply" });
            context.MapRoute(
                "Patient_default",
                "Patient/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}