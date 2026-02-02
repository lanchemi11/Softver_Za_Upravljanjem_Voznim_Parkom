using FleetManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json.Serialization;

namespace FleetManagement.Utils
{
    public static class DataSerializer
    {
        public static void SaveServisi(List<Servis> servisi, string filePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve
            };

            string json = JsonSerializer.Serialize(servisi, options);
            File.WriteAllText(filePath, json);
        }


        public static List<Servis> LoadServisi(string filePath)
        {
            if (!File.Exists(filePath)) return new List<Servis>();
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Servis>>(json);
        }

        public static void SaveVozila(List<Vozilo> vozila, string filePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve
            };

            string json = JsonSerializer.Serialize(vozila, options);
            File.WriteAllText(filePath, json);
        }

        public static List<Vozilo> LoadVozila(string filePath)
        {
            if (!File.Exists(filePath)) return new List<Vozilo>();
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Vozilo>>(json);
        }
    }
}
