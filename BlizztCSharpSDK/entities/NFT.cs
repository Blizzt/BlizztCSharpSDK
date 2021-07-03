using System;
using System.Collections.Generic;
using System.Text;

namespace BlizztCSharpSDK.entities
{
    public class NFT
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string ExternalUrl { get; set; }
        public List<NFTAttribute> Attributes { get; set; }
    }

    public class NFTAttribute
    {
        public string Display_Type { get; set; }
        public string Trait_Type { get; set; }
        public dynamic Value { get; set; }
    }
}
