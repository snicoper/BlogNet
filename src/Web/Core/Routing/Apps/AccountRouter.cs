using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Web.Core.Routing.Apps
{
    public class AccountRouter
    {
        public static void Initialize(IRouteBuilder routes)
        {
            routes.MapRoute(
                name: "account_profile",
                template: "account/profile/",
                defaults: new { controller = "Account", action = "Profile" }
            );

            routes.MapRoute(
                name: "account_login",
                template: "account/login/",
                defaults: new { controller = "Account", action = "Login" }
            );

            routes.MapRoute(
                name: "account_logout",
                template: "account/logout/",
                defaults: new { controller = "Account", action = "Logout" }
            );

            routes.MapRoute(
                name: "account_register",
                template: "account/register/",
                defaults: new { controller = "Account", action = "Register" }
            );

            routes.MapRoute(
                name: "account_confirm_email_send",
                template: "account/confirm-email-send/{userId}/",
                defaults: new { controller = "Account", action = "RegisterConfirmation" }
            );

            routes.MapRoute(
                name: "account_confirm_email",
                template: "account/confirm-email/{userId}/",
                defaults: new { controller = "Account", action = "ConfirmEmail" }
            );

            routes.MapRoute(
                name: "account_edit_password",
                template: "account/edit-password/",
                defaults: new { controller = "Account", action = "EditPassword" }
            );

            routes.MapRoute(
                name: "account_recovery_password",
                template: "account/recovery-password/",
                defaults: new { controller = "Account", action = "RecoveryPassword" }
            );

            routes.MapRoute(
                name: "account_reset_recovery_password",
                template: "account/reset-recovery-password/{userId}/",
                defaults: new { controller = "Account", action = "ResetPassword" }
            );

            routes.MapRoute(
                name: "account_edit_email",
                template: "account/edit-email/",
                defaults: new { controller = "Account", action = "EditEmail" }
            );

            routes.MapRoute(
                name: "account_confirm_change_email",
                template: "account/confirm-edit-email/{userId}/",
                defaults: new { controller = "Account", action = "ConfirmEditEmail" }
            );

            routes.MapRoute(
                name: "account_remove_temporal_email",
                template: "account/remove-temporal-email/",
                defaults: new { controller = "Account", action = "RemoveTemporalEmail" }
            );

            routes.MapRoute(
                name: "account_edit_photo",
                template: "account/edit-photo/",
                defaults: new { controller = "Account", action = "EditPhoto" }
            );
        }
    }
}
