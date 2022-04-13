namespace WebRazor.ViewModels.GCash
{
    public class WebHookEvent
    {
        public WebHookEventData? Data { get; set; }
    }

    public class WebHookEventData
    {
        public string? Id { get; set; }
        public string? Type { get; set; }

        public WebHookAttribute? Attributes { get; set; }
    }

    public class WebHookAttribute
    {
        public string? Type { get; set; }
        public bool? LiveMode { get; set; }
        public WebHookAttributeData? Data { get; set; }
    }

    public class WebHookAttributeData
    {
        /// <summary>
        /// Source Resouce Id
        /// </summary>
        public string? Id { get; set; }
        public string? Type { get; set; }
        public WebHookAttributeDataAttribute? Attributes { get; set; }
    }

    public class WebHookAttributeDataAttribute
    {
        public string? Access_Url { get; set; }
        public double Amount { get; set; }
        public string? Balance_Transaction_Id { get; set; }
        public string? Description { get; set; }
        public bool? Disputed { get; set; }
        public string? External_Reference_Number { get; set; }
        public double? Fee { get; set; }
        public bool LiveMode { get; set; }
        public double? Net_Amount { get; set; }
        public string? Payout { get; set; }
        public string? Statement_Descriptor { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public double? Tax_Amount { get; set; }

        //public DateTime Available_At { get; set; }
        //public DateTime Created_At { get; set; }
        //public DateTime Paid_At { get; set; }
        //public DateTime Updated_At { get; set; }

        //public WebHookAttributeDataAttributeSource Source { get; set; };
    }

    //public class WebHookAttributeDataAttributeSource
    //{
    //    public string Id { get; set; }
    //    public string Type { get; set; }
    //}

}
