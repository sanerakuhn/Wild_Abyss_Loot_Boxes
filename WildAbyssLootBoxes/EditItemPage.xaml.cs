using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Wild_Abyss_Loot_Boxes
{
    public partial class EditItemPage : ContentPage
    {
        public MagicItem EditableItem { get; set; }
        public ObservableCollection<string> Rarities { get; set; } = new ObservableCollection<string>
            {
                "non-magical",
                "common",
                "uncommon",
                "rare",
                "very rare",
                "legendary",
                "artifact"
            };

        public EditItemPage(MagicItem item)
        {
            InitializeComponent();


            EditableItem = new MagicItem
            {
                Name = item.Name,
                Quantity = item.Quantity,
                Rarity = item.Variants != null && item.Variants.Any() ? "varies" : item.Rarity,
                Variants = item.Variants != null
                    ? new ObservableCollection<Variant>(item.Variants)
                    : new ObservableCollection<Variant>()
            };

            txtRarityVaries.IsEnabled = false;
            txtQtyVaries.IsEnabled = false;

            EnforceVisibilityRules();
            BindingContext = this;
        }

        private void EnforceVisibilityRules()
        {
            if (EditableItem.Variants.Any())
            {
                EditableItem.Rarity = "varies";
                EditableItem.Quantity = "varies";
                pkrRarities.IsVisible = false;
                txtRarityVaries.IsVisible = true;
                txtQty.IsVisible = false;
                txtQtyVaries.IsVisible = true;
            }
            else
            {
                pkrRarities.IsVisible = true;
                txtRarityVaries.IsVisible = false;
                txtQty.IsVisible = true;
                txtQtyVaries.IsVisible = false;
            }
            OnPropertyChanged(nameof(EditableItem));
            OnPropertyChanged(nameof(Rarities));
        }

        private Variant InitializeVariant(Variant variant)
        {
            if (!Rarities.Contains(variant.Rarity))
            {
                variant.Rarity = "non-magical";
            }
            return variant;
        }

        private void AddVariant(object sender, EventArgs e)
        {
            EditableItem.Variants.Add(new Variant { Name = "New Variant", Rarity = "non-magical", Quantity = 1 });
            EnforceVariesRules();
        }

        private void RemoveVariant(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Variant variant)
            {
                EditableItem.Variants.Remove(variant);
                EnforceVariesRules();
            }
        }

        private void EnforceVariesRules()
        {
            if (EditableItem.Variants.Any())
            {
                EditableItem.Rarity = "varies";
                EditableItem.Quantity = "varies";
            }
            else
            {
                EditableItem.Rarity = "non-magical";
                EditableItem.Quantity = 1;
            }
            if (EditableItem.Variants != null && EditableItem.Variants.Any())
            {
                txtRarityVaries.IsVisible = true;
                txtQtyVaries.IsVisible = true;
                pkrRarities.IsVisible = false;
                txtQty.IsVisible = false;
            }
            else
            {
                txtRarityVaries.IsVisible = false;
                txtQtyVaries.IsVisible = false;
                pkrRarities.IsVisible = true;
                txtQty.IsVisible = true;
            }
        }

        private async void SaveItem(object sender, EventArgs e)
        {
            try
            {
                string filePath = Path.Combine(FileSystem.Current.AppDataDirectory, "loot_table.json");
                List<MagicItem> allItems = new List<MagicItem>();

                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    allItems = JsonConvert.DeserializeObject<List<MagicItem>>(json) ?? new List<MagicItem>();
                }

                var existingItem = allItems.FirstOrDefault(i => i.Name == EditableItem.Name);
                if (existingItem != null)
                {
                    allItems.Remove(existingItem);
                }
                allItems.Add(EditableItem);

                string updatedJson = JsonConvert.SerializeObject(allItems, Formatting.Indented);
                File.WriteAllText(filePath, updatedJson);

                MessagingCenter.Send(this, "ItemUpdated", EditableItem);

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save the item. {ex.Message}", "OK");
            }
        }
    }
}
