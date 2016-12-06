using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace URLService
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class URLServiceTest : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }



        [WebMethod]
        public void UpdatePropertyFor(int nodeId, string propertyAlias, object value)
        {
            try
            {
                var document = new Document(nodeId);

                if (!document.HasProperty(propertyAlias))
                    return;

                document.getProperty(propertyAlias).Value = value;
                if (document.Published) //checks if document is already published 
                {
                    User user = umbraco.BusinessLogic.User.GetUser(0);
                    document.Publish(user);
                }
                else
                {
                    //just save any changes 
                    document.Save();
                }
                library.UpdateDocumentCache(document.Id);

            }
            catch (Exception e)
            {
                //Log.FatalException(String.Format("Could not set property with alias {0} for nodeid {1}", propertyAlias, nodeId), e);
            }
        }


    }
}