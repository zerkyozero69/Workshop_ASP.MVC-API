using global::System;
using global::System.Reflection;

namespace asp_workshop.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);
        string GetDocumentation(Type type);
    }
}