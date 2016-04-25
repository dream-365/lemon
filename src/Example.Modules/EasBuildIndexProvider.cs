using Lemon.Core.Discover;

namespace eas.modules
{
    public class EasBuildIndexProvider : IBuildIndexProvider
    {
        public IBuildIndex Get(string name)
        {
            if(name == "msdn")
            {
                return new MSDNIndexBuilder();
            } else if ( name == "powerbi")
            {
                return new PowerBIIndexBuilder();
            } else if (name == "uservoice" )
            {
                return new UserVoiceIndexBuilder();
            }

            throw new System.NotSupportedException();
        }
    }
}
