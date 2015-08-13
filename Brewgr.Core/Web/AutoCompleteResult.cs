using System;
using System.Runtime.Serialization;

namespace ctorx.Core.Web
{
    /// <summary>
    /// Represents an AutoComplete result as used by JQuery UI
    /// </summary>
    [DataContract]
    public class AutoCompleteResult
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }

        [DataMember(Name = "label")]
        public string Label { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }

        /// <summary>
        /// Creates a new AutoComplete Result
        /// </summary>
        public AutoCompleteResult(string id, string label, string value)
        {
            this.ID = id;
            this.Label = label;
            this.Value = value;
        }
    }
}