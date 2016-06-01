using System;

namespace refactor_me.Models
{
    using Newtonsoft.Json;

    public abstract class GenericObject
    {
         public Guid Id { get; set; }

         public string Name { get; set; }

         public string Description { get; set; }

         [JsonIgnore]
        // set accessor needs here for setting value,
        // it is always good to have get;set; accessors in terms of creating more flexible code  
        public bool IsNew { get; set; }     
    }
}