using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CardGameCorner.Models
{
    public class ListBoxViewModel
    {
        // Property to store expansions
        public List<ExpansionModal> Expansions { get; set; }

        // Property to store listboxes
        public List<Listbox> Listboxes { get; set; }
    }

    public class Option
    {
        [JsonConverter(typeof(StringOrIntConverter))]
        public string Value { get; set; } // Now always deserialized as a string
        public string Name { get; set; }
    }
    public class Listbox
    {
        //  public List<ExpansionModal> Exapnsion { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string Filter { get; set; }
        public List<Option> Options { get; set; }
    }
    public class LanguageModal
    {
        public string Id { get; set; }
        public string Language { get; set; }
    }
    
    public class ConditionModal
    {
        public string Id { get; set; }
        public string Condition { get; set; }
    }
    public class ExpansionModal
    {
        public int Id { get; set; }
        public string Espansione { get; set; }
        public string ImageUrl { get; set; }
        public string SeoUrl { get; set; }
        public bool Special { get; set; }
        public bool Modern { get; set; }
        public bool Regular { get; set; }
        public bool Standard { get; set; }
        public bool Pioneer { get; set; }
        public int SortOrder { get; set; }
        public bool Enabled { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime DataUscita { get; set; }
    }
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Assuming the date format in the JSON is in this format: "2023-01-22T00:00:00"
            return DateTime.ParseExact(reader.GetString(), "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss"));
        }
    }


    public class StringOrIntConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return reader.GetString();
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32().ToString(); // Convert number to string
            }
            throw new JsonException($"Unexpected token parsing {typeToConvert}. Expected String or Number, got {reader.TokenType}.");
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }



}
