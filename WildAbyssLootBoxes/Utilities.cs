using Newtonsoft.Json;

namespace Wild_Abyss_Loot_Boxes
{
    public static class LootTableLoader
    {
        public static List<MagicItem> LoadLootTable()
        {
            var filePath = Path.Combine(FileSystem.Current.AppDataDirectory, "loot_table.json");

            if (!File.Exists(filePath))
            {
                using var stream = FileSystem.Current.OpenAppPackageFileAsync("loot_table.json").Result;
                using var reader = new StreamReader(stream);
                var content = reader.ReadToEnd();

                File.WriteAllText(filePath, content);
            }

            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<MagicItem>>(json) ?? new List<MagicItem>();
        }
    }
}
