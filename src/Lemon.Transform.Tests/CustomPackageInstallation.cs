namespace Lemon.Transform.Tests
{
    public class CustomPackageInstallation : PackageInstallation
    {
        public override void OnCreate(PackageBuilder builder)
        {
            builder.Input("mongo_office_365")
                   .Action(new FakeTransformAction1())
                   .Output("json_office_365");
        }
    }
}
