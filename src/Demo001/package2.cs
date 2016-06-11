using Lemon.Transform;

namespace Demo001
{
    public class Package2 : PackageInstallation
    {
        public override void OnCreate(PackageBuilder builder)
        {
            builder.Input("landing_threads")
                   .Action(new UserActiviesTransformAction())
                   .Output("user_activity");
        }
    }
}
