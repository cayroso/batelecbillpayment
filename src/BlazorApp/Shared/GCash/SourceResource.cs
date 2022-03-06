using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.GCash
{
    public class SourceResource
    {
        public SourceResourceInfo Data { get; set; }

        public class SourceResourceInfo
        {
            public string Id { get; set; }
            public AttributesInfo Attributes { get; set; }

            public class AttributesInfo
            {
                public double Amount { get; set; }
                public string Description { get; set; }
                public string Status { get; set; }
                public RedirectInfo Redirect { get; set; }
            }

            public class RedirectInfo
            {
                public string Checkout_Url { get; set; }
                public string Success { get; set; }
                public string Failed { get; set; }
            }

        }
    }


}
