var options = new JsonSerializerOptions(JsonSerializerDefaults.___)
{
    WriteIndented = true
};
string json = JsonSerializer.Serialize(incident, options);
