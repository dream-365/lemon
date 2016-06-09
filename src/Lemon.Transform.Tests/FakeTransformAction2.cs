namespace Lemon.Transform.Tests
{
    public class FakeTransformAction2 : BaseTransformAction
    {
        protected override void Build()
        {
            Copy("_id", "Id");

            Copy("title", "Title");

            Copy("createdOn", "CreatedOn");

            Copy("createBy", "CreateBy");
        }
    }
}
