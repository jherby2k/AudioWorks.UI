/* Copyright © 2019 Jeremy Herbison

This file is part of AudioWorks.

AudioWorks is free software: you can redistribute it and/or modify it under the terms of the GNU Affero General Public
License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later
version.

AudioWorks is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more
details.

You should have received a copy of the GNU Affero General Public License along with AudioWorks. If not, see
<https://www.gnu.org/licenses/>. */

using System;
using System.Globalization;
using AudioWorks.Common;
using Newtonsoft.Json;

namespace AudioWorks.UI.Services
{
    sealed class SettingDictionaryConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) =>
            throw new NotImplementedException();

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new SettingDictionary();

            reader.Read();
            while (reader.TokenType == JsonToken.PropertyName)
            {
                var propertyName = (string) reader.Value;

                reader.Read();
                var value = reader.TokenType == JsonToken.Integer
                    ? Convert.ToInt32(reader.Value, NumberFormatInfo.InvariantInfo)
                    : serializer.Deserialize(reader);
                result.Add(propertyName, value);

                reader.Read();
            }

            return result;
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(SettingDictionary);

        public override bool CanWrite => false;
    }
}