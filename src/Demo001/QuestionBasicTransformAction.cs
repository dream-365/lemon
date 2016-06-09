using Lemon.Transform;

namespace Demo001
{
    public class QuestionBasicTransformAction : BaseTransformAction
    {
        protected override void Build()
        {
            Copy("_id", "questionId");

            Copy("authorId", "createdBy");
        }
    }
}
