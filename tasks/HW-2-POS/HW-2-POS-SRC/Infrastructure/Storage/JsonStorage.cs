using HW_2_POS.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace HW_2_POS.Infrastructure.Storage
{
    internal sealed class JsonStorage(string filePath = "pos-data.json") : IDataStorage
    {
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        public StorageDTO Load()
        {
            if (!File.Exists(filePath))
                return new StorageDTO();
            var json = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(json))
                return new StorageDTO();
            return JsonSerializer.Deserialize<StorageDTO>(json, SerializerOptions)
                ?? throw new InvalidOperationException($"Failed to deserialize database from '{filePath}'");
        }

        public void Save(StorageDTO database)
        {
            var json = JsonSerializer.Serialize(database, SerializerOptions);
            File.WriteAllText(filePath, json);
        }
    }

}
