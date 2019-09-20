using System.Collections.Generic;

namespace Magalu.Challenge.Data.External.Models
{
    public class GetProductPageModel
    {
        public MetadataModel Meta { get; set; }

        public IList<GetProductModel> Products { get; set; }
    }
}
