﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace Worker.Domain.Configuration;

public interface IJsonSerializer
{
    T Deserialize<T>(string input);
    string Serialize<T>(T input);
}

public class JsonSerializer : IJsonSerializer
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    static JsonSerializer()
    {
        Options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    }

    public T Deserialize<T>(string input)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(input, Options)!;
    }

    public string Serialize<T>(T input)
    {
        return System.Text.Json.JsonSerializer.Serialize(input, Options);
    }
}
