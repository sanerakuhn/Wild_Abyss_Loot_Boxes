using Newtonsoft.Json;

namespace Wild_Abyss_Loot_Boxes
{
    public class MagicItem
    {
        public string Name { get; set; }
        public string Rarity { get; set; }
        public object Quantity { get; set; }
        public List<Variant> Variants { get; set; }

        public int GetQuantity(string variantName = null)
        {
            if (Quantity is string && Quantity.ToString() == "varies" && Variants != null && variantName != null)
            {
                var variant = Variants.FirstOrDefault(v => v.Name == variantName);
                return variant?.Quantity ?? 0;
            }

            if (Quantity is int qty)
            {
                return qty;
            }

            return 1;
        }
    }

    public class Variant
    {
        public string Name { get; set; }
        public string Rarity { get; set; }
        public int Quantity { get; set; }
    }

    public partial class MainPage : ContentPage
    {
        private List<MagicItem> _items;
        private int _itemCount = 1;
        private int _partyLevel = 1;

        public int ItemCount
        {
            get => _itemCount;
            set
            {
                _itemCount = value;
                OnPropertyChanged(nameof(ItemCount));
            }
        }

        public int PartyLevel
        {
            get => _partyLevel;
            set
            {
                _partyLevel = Math.Clamp(value, 1, 20);
                OnPropertyChanged(nameof(PartyLevel));
            }
        }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            LoadItems();
            InitializePreferences();

            OptionsPage.MagicItemsUpdated += UpdateMagicItems;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            OptionsPage.MagicItemsUpdated += UpdateMagicItems;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            OptionsPage.MagicItemsUpdated -= UpdateMagicItems;
        }

        private void UpdateMagicItems(List<MagicItem> updatedItems, bool append)
        {
            if (append)
            {
                _items.AddRange(updatedItems);
            }
            else
            {
                _items = updatedItems;
            }

            DisplayAlert("Update", "Magic Items list has been updated!", "OK");
        }

        private void LoadItems()
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
            _items = JsonConvert.DeserializeObject<List<MagicItem>>(json);
        }

        private void InitializePreferences()
        {
            if (Preferences.Get("MinGold", 0) == 0)
            {
                Preferences.Set("MinGold", 10);
            }
            if (Preferences.Get("MaxGold", 0) == 0)
            {
                Preferences.Set("MaxGold", 1000);
            }
            if (Preferences.Get("MagicItemsPerSpin", 0) == 0)
            {
                Preferences.Set("MagicItemsPerSpin", 1);
            }
            if (Preferences.Get("CommonItemsPerSpin", 0) == 0)
            {
                Preferences.Set("CommonItemsPerSpin", 1);
            }
            if (Preferences.Get("MagicItemsMaxPerSpin", 0) == 0)
            {
                Preferences.Set("MagicItemsMaxPerSpin", 1);
            }
            if (Preferences.Get("CommonItemsMaxPerSpin", 0) == 0)
            {
                Preferences.Set("CommonItemsMaxPerSpin", 1);
            }
            Preferences.Set("GoldEnabled", true);
            Preferences.Set("MagicItemsEnabled", true);
            Preferences.Set("CommonItemsEnabled", true);
            Preferences.Set("MagicItemsUseRange", false);
            Preferences.Set("CommonItemsUseRange", false);
        }

        private void IncrementCounter(object sender, EventArgs e) => ItemCount++;
        private void DecrementCounter(object sender, EventArgs e) => ItemCount = Math.Max(1, ItemCount - 1);

        private void IncrementPartyLevel(object sender, EventArgs e) => PartyLevel++;
        private void DecrementPartyLevel(object sender, EventArgs e) => PartyLevel--;

        private void SpinItems(object sender, EventArgs e)
        {
            if (_items == null || !_items.Any())
            {
                DisplayAlert("Error", "Item dataset is not loaded. Please check the file.", "OK");
                return;
            }

            var random = new Random();
            var results = new List<FormattedString>();

            bool magicItemsEnabled = Preferences.Get("MagicItemsEnabled", false);
            bool commonItemsEnabled = Preferences.Get("CommonItemsEnabled", false);
            bool goldEnabled = Preferences.Get("GoldEnabled", false);
            bool magicItemsUseRange = Preferences.Get("MagicItemsUseRange", false);
            bool commonItemsUseRange = Preferences.Get("CommonItemsUseRange", false);

            int magicItemsMin = Preferences.Get("MagicItemsPerSpin", 1);
            int magicItemsMax = Preferences.Get("MagicItemsMaxPerSpin", 1);

            int commonItemsMin = Preferences.Get("CommonItemsPerSpin", 1);
            int commonItemsMax = Preferences.Get("CommonItemsMaxPerSpin", 1);

            for (int i = 0; i < ItemCount; i++)
            {
                var formattedResult = new FormattedString();
                var itemGroups = new Dictionary<string, (int Quantity, string DisplayText, string Rarity)>();

                if (magicItemsEnabled)
                {
                    int magicItemsCount = magicItemsUseRange
                        ? random.Next(magicItemsMin, magicItemsMax + 1)
                        : magicItemsMin;

                    for (int j = 0; j < magicItemsCount; j++)
                    {
                        string rarity = GetRarity(random.NextDouble());
                        AddOrUpdateItemGroup(itemGroups, random, rarity);
                    }
                }

                if (commonItemsEnabled)
                {
                    int commonItemsCount = commonItemsUseRange
                        ? random.Next(commonItemsMin, commonItemsMax + 1)
                        : commonItemsMin;

                    for (int j = 0; j < commonItemsCount; j++)
                    {
                        AddOrUpdateItemGroup(itemGroups, random, "mundane");
                    }
                }

                if (goldEnabled)
                {
                    int gold = GenerateRandomValue(_partyLevel);
                    string goldKey = "Gold";
                    if (itemGroups.ContainsKey(goldKey))
                    {
                        itemGroups[goldKey] = (
                            itemGroups[goldKey].Quantity + gold,
                            $"{itemGroups[goldKey].Quantity + gold} GP",
                            "legendary"
                        );
                    }
                    else
                    {
                        itemGroups[goldKey] = (gold, $"{gold} GP", "legendary");
                    }
                }

                foreach (var group in itemGroups)
                {
                    formattedResult.Spans.Add(new Span
                    {
                        Text = group.Value.DisplayText,
                        TextColor = GetRarityColor(group.Value.Rarity),
                        FontAttributes = FontAttributes.Bold
                    });
                    formattedResult.Spans.Add(new Span { Text = "\n" });
                }

                if (formattedResult.Spans.Count > 0)
                {
                    formattedResult.Spans.RemoveAt(formattedResult.Spans.Count - 1);
                }

                results.Add(formattedResult);
            }

            ResultsListView.ItemsSource = results;
        }

        private void AddOrUpdateItemGroup(Dictionary<string, (int Quantity, string DisplayText, string Rarity)> itemGroups, Random random, string rarity)
        {
            var itemsOfRarity = _items.Where(item =>
                item.Rarity == rarity ||
                (item.Rarity == "varies" && item.Variants != null && item.Variants.Any(v => v.Rarity == rarity))
            ).ToList();

            if (!itemsOfRarity.Any())
                return;

            var selectedItem = itemsOfRarity[random.Next(itemsOfRarity.Count)];
            Variant selectedVariant = null;

            if (selectedItem.Rarity == "varies" && selectedItem.Variants != null)
            {
                var variantsOfRarity = selectedItem.Variants.Where(v => v.Rarity == rarity).ToList();
                if (variantsOfRarity.Any())
                {
                    selectedVariant = variantsOfRarity[random.Next(variantsOfRarity.Count)];
                }
            }

            string itemKey = selectedVariant != null
                ? $"{selectedItem.Name} ({selectedVariant.Name})"
                : selectedItem.Name;

            int quantity = selectedVariant != null
                ? selectedVariant.Quantity
                : selectedItem.GetQuantity();

            if (itemGroups.ContainsKey(itemKey))
            {
                itemGroups[itemKey] = (
                    itemGroups[itemKey].Quantity + quantity,
                    $"{itemGroups[itemKey].Quantity + quantity}x {itemKey}",
                    rarity
                );
            }
            else
            {
                itemGroups[itemKey] = (quantity, $"{quantity}x {itemKey}", rarity);
            }
        }

        private int GenerateRandomValue(int level)
        {
            var random = new Random();
            double randomFactor = random.NextDouble();

            int minGold = Preferences.Get("MinGold", 10);
            int maxGold = Preferences.Get("MaxGold", 1000);

            level = Math.Clamp(level, 1, 20);

            double alpha = 1 + (10 - level) / 100.0;

            double weightedRandom = Math.Pow(randomFactor, alpha);

            double randomValue = minGold + (maxGold - minGold) * weightedRandom;

            return (int)Math.Round(randomValue);
        }

        private string GetRarity(double value)
        {
            var rarityOdds = new Dictionary<string, double>
            {
        { "common", Math.Max(40 - (PartyLevel - 1) * 2, 10) },
        { "uncommon", Math.Max(35 - (PartyLevel - 1) * 1.5, 15) },
        { "rare", 20 + (PartyLevel - 1) * 1.25 },
        { "very rare", 4 + (PartyLevel - 1) * 0.75 },
        { "legendary", 1 + (PartyLevel - 1) * 0.5 },
        { "artifact", 0.2 + (PartyLevel - 1) * 0.25 }
    };

            if (!Preferences.Get("IncludeCommon", true)) rarityOdds.Remove("common");
            if (!Preferences.Get("IncludeUncommon", true)) rarityOdds.Remove("uncommon");
            if (!Preferences.Get("IncludeRare", true)) rarityOdds.Remove("rare");
            if (!Preferences.Get("IncludeVeryRare", true)) rarityOdds.Remove("very rare");
            if (!Preferences.Get("IncludeLegendary", true)) rarityOdds.Remove("legendary");
            if (!Preferences.Get("IncludeArtifact", true)) rarityOdds.Remove("artifact");

            double totalOdds = rarityOdds.Values.Sum();
            var normalizedOdds = rarityOdds.ToDictionary(k => k.Key, v => v.Value / totalOdds);

            double cumulative = 0;
            foreach (var rarity in normalizedOdds)
            {
                cumulative += rarity.Value;
                if (value < cumulative) return rarity.Key;
            }

            return "common";
        }


        public static Color GetRarityColor(string rarity)
        {
            return rarity.ToLower() switch
            {
                "mundane" => Color.FromArgb("#1E1E1E"),
                "common" => Color.FromArgb("#D3D3D3"),
                "uncommon" => Color.FromArgb("#7CFC00"),
                "rare" => Color.FromArgb("#00BFFF"),
                "very rare" => Color.FromArgb("#DA70D6"),
                "legendary" => Color.FromArgb("#FFA500"),
                "artifact" => Color.FromArgb("#FF6347"),
                _ => Colors.Gray
            };
        }

        private async void NavigateToOptions(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OptionsPage());
        }
    }
}