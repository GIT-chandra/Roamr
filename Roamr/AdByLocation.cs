using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roamr
{
    public class Doc
    {
        public string order_paymentFor { get; set; }
        public string attribute_Ad_Type { get; set; }
        public string order_paymentStatus { get; set; }
        public string cityName { get; set; }
        public string attribute_You_are { get; set; }
        public string order_createdTimestamp { get; set; }
        public string order_updatedTimestamp { get; set; }
        public string order_orderUpdatedReason { get; set; }
        public string categoryName { get; set; }
        public string attribute_presence { get; set; }
        public string order_parentpackid { get; set; }
        public string order_invoiceId { get; set; }
        public string attribute_Price { get; set; }
        public string metaCategoryName { get; set; }
        public string attribute_Brand_name { get; set; }
        public string verified_mobile { get; set; }
        public string attribute_Condition { get; set; }
        public string status { get; set; }
        public string adExpireTime { get; set; }
        public string ad_locality { get; set; }
        public string order_productStatus { get; set; }
        public string content { get; set; }
        public string ad_quality_score { get; set; }
        public string order_packid { get; set; }
        public object images { get; set; }
        public string order_orderId { get; set; }
        public string cas { get; set; }
        public string order_remark { get; set; }
        public string image_count { get; set; }
        public string order_table_id { get; set; }
        public string order_amount { get; set; }
        public string title { get; set; }
        public string order_modeofpayment { get; set; }
        public string order_attempts { get; set; }
        public string geo_pin { get; set; }
        public string recentlyViewedTime { get; set; }
        public string createdTime { get; set; }
        public string order_paymentType { get; set; }
        public string stateName { get; set; }
        public long attribute_last_online { get; set; }
        public int ad_view_count { get; set; }
        public string url { get; set; }
    }

    public class AdsByLocationData
    {
        public bool success { get; set; }
        public int total { get; set; }
        public int timeTaken { get; set; }
        public int responseCode { get; set; }
        public List<Doc> docs { get; set; }
    }

    public class MetaData
    {
        public string requestId { get; set; }
    }

    public class AdsByLocationResponse
    {
        public AdsByLocationData AdsByLocationData { get; set; }
        public MetaData MetaData { get; set; }
    }

    public class AdByLocation
    {
        public AdsByLocationResponse AdsByLocationResponse { get; set; }
    }
}
