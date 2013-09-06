﻿using System.Collections.Generic;
using System.Linq;

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    /// Abstract class for records with name maps.
    /// </summary>
    public abstract class NamedEntity
    {
        /// <summary>
        /// A <see cref="System.Collections.Generic.Dictionary{T,U}"/> from language codes to the name in that language.
        /// This attribute is returned by all end points.
        /// </summary>
        public Dictionary<string, string> Names { get; internal set; }

        /// <summary>
        /// The GeoName ID for the city. This attribute is returned by all endpoints
        /// </summary>
        public int GeonameId { get; internal set; }

        /// <summary>
        /// Gets or sets the languages specified by the user.
        /// </summary>
        internal List<string> Languages { get; set; }

        /// <summary>
        /// The name of the city based on the languages list passed to the
        /// <see cref="WebServiceClient"/> constructor. This
        /// attribute is returned by all endpoints.
        /// </summary>
        public string Name
        {
            get
            {
                var lang = Languages.FirstOrDefault(l => Names.ContainsKey(l));
                return lang == null ? null : Names[lang];
            }
        }
    }
}