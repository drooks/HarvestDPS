using System.Web.Mvc;
using Harvest.Net;

namespace HarvestDPS.Extensions
{
    //public class SiteApiController : ApiController
    //{
    //    /// <summary>
    //    /// Api Controller Helper
    //    /// </summary>
    //    /// 

    //    private HarvestRestClient _client;
    //    public HarvestRestClient Client
    //    {
    //        get
    //        {
    //            if (_client == null)
    //            {
    //                var request = new HttpRequestWrapper(HttpContext.Current.Request);
    //                _client = HarvestHelper.GetHarvestClient(request);
    //            }

    //            return _client;

    //        }
    //    }
    //}

    public  class SiteController : Controller
    {
        private HarvestRestClient _client;
        public HarvestRestClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = HarvestHelper.GetHarvestClient(Request);
                }

                return _client;

            }
        }
    }
}