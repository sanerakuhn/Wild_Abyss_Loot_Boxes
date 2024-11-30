using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Wild_Abyss_Loot_Boxes
{
    public partial class LootTablePage : ContentPage
    {
        private List<MagicItem> _allItems;
        public ObservableCollection<MagicItem> FilteredItems { get; set; }

        private readonly Dictionary<string, int> RarityOrder = new()
        {
            { "non-magical", 0 },
            { "common", 1 },
            { "uncommon", 2 },
            { "rare", 3 },
            { "very rare", 4 },
            { "legendary", 5 },
            { "artifact", 6 },
            { "varies", 7 }
        };

        public LootTablePage()
        {
            InitializeComponent();

            _allItems = LootTableLoader.LoadLootTable();
            FilteredItems = new ObservableCollection<MagicItem>(_allItems);
            SortFilteredItemsByRarity();
            BindingContext = this;

            MessagingCenter.Subscribe<EditItemPage, MagicItem>(this, "ItemUpdated", (sender, updatedItem) =>
            {
                Console.WriteLine($"Received update for item: {updatedItem.Name}");
                UpdateItemAndRefresh(updatedItem);
            });
        }

        private void OnRarityFilterChanged(object sender, CheckedChangedEventArgs e)
        {
            ApplyRarityFilter();
        }

        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = e.NewTextValue?.ToLower() ?? string.Empty;

            var filteredResults = _allItems.Where(item =>
                item.Name.ToLower().Contains(searchText) ||
                (item.Variants != null && item.Variants.Any(v => v.Name.ToLower().Contains(searchText))))
                .ToList();

            FilteredItems.Clear();
            foreach (var item in filteredResults)
            {
                FilteredItems.Add(item);
            }
        }

        private void ApplyRarityFilter()
        {
            var selectedRarities = new List<string>();
            if (NonMagicCheckBox.IsChecked) selectedRarities.Add("non-magical");
            if (CommonCheckBox.IsChecked) selectedRarities.Add("common");
            if (UncommonCheckBox.IsChecked) selectedRarities.Add("uncommon");
            if (RareCheckBox.IsChecked) selectedRarities.Add("rare");
            if (VeryRareCheckBox.IsChecked) selectedRarities.Add("very rare");
            if (LegendaryCheckBox.IsChecked) selectedRarities.Add("legendary");
            if (ArtifactCheckBox.IsChecked) selectedRarities.Add("artifact");

            FilteredItems.Clear();

            foreach (var item in _allItems)
            {
                bool matchesItemRarity = selectedRarities.Contains(item.Rarity.ToLower());
                bool matchesVariantRarity = item.Variants?.Any(v => selectedRarities.Contains(v.Rarity.ToLower())) ?? false;

                if (matchesItemRarity || matchesVariantRarity)
                {
                    FilteredItems.Add(item);
                }
            }
        }

        private void ToggleItemDetails(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.Content is StackLayout stackLayout)
            {
                var details = stackLayout.FindByName<StackLayout>("ItemDetails");
                if (details != null)
                {
                    details.IsVisible = !details.IsVisible;
                }
            }
        }

        private async void DeleteSelectedItems(object sender, EventArgs e)
        {
            var selectedItems = _allItems.Where(item => item.IsSelected).ToList();
            if (!selectedItems.Any())
            {
                await DisplayAlert("No Selection", "Please select items to delete.", "OK");
                return;
            }

            bool isConfirmed = await DisplayAlert(
                "Confirm Deletion",
                $"Are you sure you want to delete {selectedItems.Count} item(s)? This action cannot be undone.",
                "Delete",
                "Cancel"
            );

            if (!isConfirmed)
            {
                return;
            }

            foreach (var item in selectedItems)
            {
                _allItems.Remove(item);
                FilteredItems.Remove(item);
            }

            var filePath = Path.Combine(FileSystem.Current.AppDataDirectory, "loot_table.json");
            File.WriteAllText(filePath, JsonConvert.SerializeObject(_allItems, Formatting.Indented));

            await DisplayAlert("Success", $"{selectedItems.Count} item(s) have been deleted.", "OK");
        }

        private void UpdateItemAndRefresh(MagicItem updatedItem)
        {
            _allItems = LootTableLoader.LoadLootTable();

            var refreshedItems = new ObservableCollection<MagicItem>(_allItems);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                FilteredItems = refreshedItems;

                LootTableCollectionView.ItemsSource = null;
                SortFilteredItemsByRarity();
                LootTableCollectionView.ItemsSource = FilteredItems;

                ApplyRarityFilter();
            });

        }

        private void SortFilteredItemsByRarity()
        {
            var sortedItems = FilteredItems
                .OrderBy(item => RarityOrder.TryGetValue(item.Rarity.ToLower(), out var order) ? order : int.MaxValue)
                .ToList();

            FilteredItems.Clear();
            foreach (var item in sortedItems)
            {
                FilteredItems.Add(item);
            }
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            Shell.SetBackButtonBehavior(this, new BackButtonBehavior
            {
                IsVisible = false
            });
        }

        private async void OnEditButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is MagicItem itemToEdit)
            {
                await Navigation.PushAsync(new EditItemPage(itemToEdit));
            }
        }

        private async void AddEditItem(object sender, EventArgs e)
        {
            MagicItem itemToEdit = new MagicItem
            {
                Name = "New Item",
                Quantity = 1,
                Rarity = "non-magical",
                Variants = null
            };

            await Navigation.PushAsync(new EditItemPage(itemToEdit));
        }

        private async void NavigateToMain(object sender, EventArgs e)
        {
            MessagingCenter.Unsubscribe<EditItemPage, MagicItem>(this, "ItemUpdated");

            await Navigation.PopAsync(true);
        }
    }
}
