﻿using MaxMind.Db;
using Newtonsoft.Json;
using System;
using System.Net;

namespace MaxMind.GeoIP2
{
    class NetworkConverter : JsonConverter<Network>
    {
        public override void WriteJson(JsonWriter writer, Network value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override Network ReadJson(JsonReader reader, Type objectType, Network existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // It'd probably be nice if we added an appropriate constructor
            // to Network.
            var parts = ((string)reader.Value).Split('/');
            if (parts.Length != 2 || !int.TryParse(parts[1], out var prefixLength))
            {
                throw new JsonSerializationException("Network not in CIDR format: " + reader.Value);
            }

            return new Network(IPAddress.Parse(parts[0]), prefixLength);
        }
    }
}
