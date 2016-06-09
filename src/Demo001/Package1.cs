using Lemon.Transform;

namespace Demo001
{
    public class Package1 : PackageInstallation
    {
        public override void OnCreate(PackageBuilder builder)
        {
            builder.Input("office_365_threads")
                   .Action(new QuestionBasicTransformAction())
                   .Output("output_test");
        }
    }
}
