namespace Lemon.Transform
{
    public abstract class PackageInstallation
    {
        private PackageBuilder _builder;

        public PackageInstallation()
        {
            _builder = new PackageBuilder();
        }

        public abstract void OnCreate(PackageBuilder builder);

        internal TransformPackage Create()
        {
            OnCreate(_builder);

            return _builder.Build();
        }
    }
}
