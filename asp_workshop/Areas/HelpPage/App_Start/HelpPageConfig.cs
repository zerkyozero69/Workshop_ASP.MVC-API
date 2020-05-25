// Uncomment the following to provide samples for PageResult(Of T). Must also add the Microsoft.AspNet.WebApi.OData
// package to your project.
// '#Const Handle_PageResultOfT = 1

using global::System.Diagnostics.CodeAnalysis;
using global::System.Net.Http.Headers;
using global::System.Web.Http;
/* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
namespace asp_workshop.Areas.HelpPage
{
    /// <summary>
    /// Use this class to customize the Help Page.
    /// For example you can set a custom <see cref="System.Web.Http.Description.IDocumentationProvider"/> to supply the documentation
    /// or you can provide the samples for the requests/responses.
    /// </summary>
    public static class HelpPageConfig
    {
        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "asp_workshop.Areas.HelpPage.TextSample.#ctor(System.String)", Justification = "End users may choose to merge this string with existing localized resources.")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "bsonspec", Justification = "Part of a URI.")]
        public static void Register(HttpConfiguration config)
        {
            // ' Uncomment the following to use the documentation from XML documentation file.
            // config.SetDocumentationProvider(New XmlDocumentationProvider(HttpContext.Current.Server.MapPath("~/App_Data/XmlDocument.xml")))

            // ' Uncomment the following to use "sample string" as the sample for all actions that have string as the body parameter or return type.
            // ' Also, the string arrays will be used for IEnumerable(Of String). The sample objects will be serialized into different media type 
            // ' formats by the available formatters.
            // config.SetSampleObjects(New Dictionary(Of Type, Object) From
            // {
            // {GetType(String), "sample string"},
            // {GetType(IEnumerable(Of String)), New String() {"sample 1", "sample 2"}}
            // })

            // Extend the following to provide factories for types not handled automatically (those lacking parameterless
            // constructors) or for which you prefer to use non-default property values. Line below provides a fallback
            // since automatic handling will fail and GeneratePageResult handles only a single type.
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                                                                                                   // Extend the following to use a preset object directly as the sample for all actions that support a media
                                                                                                   // type, regardless of the body parameter or return type. The lines below avoid display of binary content.
                                                                                                   // The BsonMediaTypeFormatter (if available) is not used to serialize the TextSample object.
            config.SetSampleForMediaType(new TextSample("Binary JSON content. See http://bsonspec.org for details."), new MediaTypeHeaderValue("application/bson"));

            // ' Uncomment the following to use "[0]=foo&[1]=bar" directly as the sample for all actions that support form URL encoded format
            // ' and have IEnumerable(Of String) as the body parameter or return type.
            // config.SetSampleForType("[0]=foo&[1]=bar", New MediaTypeHeaderValue("application/x-www-form-urlencoded"), GetType(IEnumerable(Of String)))

            // ' Uncomment the following to use "1234" directly as the request sample for media type "text/plain" on the controller named "Values"
            // ' and action named "Put".
            // config.SetSampleRequest("1234", New MediaTypeHeaderValue("text/plain"), "Values", "Put")

            // ' Uncomment the following to use the image on "../images/aspNetHome.png" directly as the response sample for media type "image/png"
            // ' on the controller named "Values" and action named "Get" with parameter "id".
            // config.SetSampleResponse(New ImageSample("../images/aspNetHome.png"), New MediaTypeHeaderValue("image/png"), "Values", "Get", "id")

            // ' Uncomment the following to correct the sample request when the action expects an HttpRequestMessage with ObjectContent(Of string).
            // ' The sample will be generated as if the controller named "Values" and action named "Get" were having String as the body parameter.
            // config.SetActualRequestType(GetType(String), "Values", "Get")

            // ' Uncomment the following to correct the sample response when the action returns an HttpResponseMessage with ObjectContent(Of String).
            // ' The sample will be generated as if the controller named "Values" and action named "Post" were returning a String.
            // config.SetActualResponseType(GetType(String), "Values", "Post")
        }

        /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
    }
}