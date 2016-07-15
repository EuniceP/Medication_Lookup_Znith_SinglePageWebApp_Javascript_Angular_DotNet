using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ComponentModel;
using System.Text;
using System.IO;

namespace Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService" in both code and config file together.
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        [Description("Get Pharmaceuticals Groups")]
        [WebGet(RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "GET_PHARMA_GROUP"
        )]
        Stream GET_PHARMA_GROUP();

        [OperationContract]
        [Description("Get Pharmaceuticals Classes")]
        [WebGet(RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "GET_PHARMA_CLASS?Group_Code={Group_Code}"
        )]
        Stream GET_PHARMA_CLASS(string Group_Code);

        [OperationContract]
        [Description("Get Pharmaceuticals Groups/Classes")]
        [WebGet(RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "GET_PHARMA_GROUPCLASS"
        )]
        Stream GET_PHARMA_GROUPCLASS();

        [OperationContract]
        [Description("Get Pharmaceuticals Price History")]
        [WebGet(RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "GET_PHARMA_PRICE_HISTORY?NDC={NDC}"
        )]
        Stream GET_PHARMA_PRICE_HISTORY(string NDC);

        [OperationContract]
        [Description("Get Pharmaceuticals Ingredient")]
        [WebGet(RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "GET_PHARMA_INGREDIENT?NDC={NDC}"
        )]
        Stream GET_PHARMA_INGREDIENT(string NDC);

        [OperationContract]
        [Description("Get Pharmaceuticals Generic Names")]
        [WebGet(RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "GET_PHARMA_GENERICNAMES?Group_Code={Group_Code}&Class_Code={Class_Code}&NDC={NDC}&GPI={GPI}&Product={Product}&Manufacturer={Manufacturer}&GroupClass_Name={GroupClass_Name}"
        )]
        Stream GET_PHARMA_GENERICNAMES(string Group_Code, string Class_Code, string NDC, string GPI, string Product, string Manufacturer, string GroupClass_Name);

        [OperationContract]
        [Description("Get Pharmaceuticals Manufacturer")]
        [WebGet(RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "GET_PHARMA_MANUFACTURER"
        )]
        Stream GET_PHARMA_MANUFACTURER();

        [OperationContract]
        [Description("Get Pharmaceuticals")]
        [WebGet(RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "GET_PHARMA_RESULT?Group_Code={Group_Code}&Class_Code={Class_Code}&NDC={NDC}&GPI={GPI}&Product={Product}&Manufacturer={Manufacturer}&GroupClass_Name={GroupClass_Name}"
        )]
        Stream GET_PHARMA_RESULT(string Group_Code, string Class_Code, string NDC, string GPI, string Product, string Manufacturer, string GroupClass_Name);

    }
}
