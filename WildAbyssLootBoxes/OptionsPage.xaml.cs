using Newtonsoft.Json;

namespace Wild_Abyss_Loot_Boxes
{
    public partial class OptionsPage : ContentPage
    {
        public OptionsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            MinGoldEntry.Text = Preferences.Get("MinGold", 0).ToString();
            MaxGoldEntry.Text = Preferences.Get("MaxGold", 0).ToString();
            MagicItemsEntry.Text = Preferences.Get("MagicItemsPerSpin", 0).ToString();
            CommonItemsEntry.Text = Preferences.Get("CommonItemsPerSpin", 0).ToString();
            MagicItemsEntryMax.Text = Preferences.Get("MagicItemsMaxPerSpin", 0).ToString();
            CommonItemsEntryMax.Text = Preferences.Get("CommonItemsMaxPerSpin", 0).ToString();

            CommonRarityCheckBox.IsChecked = Preferences.Get("IncludeCommon", true);
            UncommonRarityCheckBox.IsChecked = Preferences.Get("IncludeUncommon", true);
            RareRarityCheckBox.IsChecked = Preferences.Get("IncludeRare", true);
            VeryRareRarityCheckBox.IsChecked = Preferences.Get("IncludeVeryRare", true);
            LegendaryRarityCheckBox.IsChecked = Preferences.Get("IncludeLegendary", true);
            ArtifactRarityCheckBox.IsChecked = Preferences.Get("IncludeArtifact", true);

            if (Preferences.Get("GoldEnabled", true) == false)
            {
                GoldCheckBox.IsChecked = false;
            }
            if (Preferences.Get("MagicItemsEnabled", true) == false)
            {
                MagicItemsCheckBox.IsChecked = false;
            }
            if (Preferences.Get("CommonItemsEnabled", true) == false)
            {
                CommonItemsCheckBox.IsChecked = false;
            }
            if (Preferences.Get("MagicItemsUseRange", true) == false)
            {
                MagicItemsRangeCheckBox.IsChecked = false;
            }
            if (Preferences.Get("CommonItemsUseRange", true) == false)
            {
                CommonItemsRangeCheckBox.IsChecked = false;
            }
        }


        private void OnGoldCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            bool isChecked = e.Value;
            ToggleGold(isChecked);
        }

        private void ToggleGold(bool toggle)
        {
            MinGoldEntry.IsEnabled = toggle;
            MaxGoldEntry.IsEnabled = toggle;

            Color labelColor = Colors.Gray;
            if (Application.Current.Resources.TryGetValue(toggle ? "TextColor" : "DisabledTextColor", out var resource))
            {
                labelColor = (Color)resource;
            }

            MinGoldEntry.TextColor = labelColor;
            MaxGoldEntry.TextColor = labelColor;
            lblNumGoldRange.TextColor = labelColor;
            lblNumGoldRange2.TextColor = labelColor;

            Preferences.Set("GoldEnabled", toggle);
        }

        private void OnMagicItemsCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            bool isChecked = e.Value;
            ToggleMagicItems(isChecked);
        }

        private void ToggleMagicItems(bool toggle)
        {
            MagicItemsEntry.IsEnabled = toggle;
            MagicItemsEntryMax.IsEnabled = toggle;
            MagicItemsRangeCheckBox.IsEnabled = toggle;

            Color labelColor = Colors.Gray;
            if (Application.Current.Resources.TryGetValue(toggle ? "TextColor" : "DisabledTextColor", out var resource))
            {
                labelColor = (Color)resource;
            }

            lblMagicItemsRangeCheckBox.TextColor = labelColor;
            MagicItemsEntry.TextColor = labelColor;
            lblNumMagicItems.TextColor = labelColor;
            lblNumMagicItemsAnd.TextColor = labelColor;
            lblNumMagicItems2.TextColor = labelColor;

            lblCommonRarityCheckBox.TextColor = labelColor;
            lblUncommonRarityCheckBox.TextColor = labelColor;
            lblRareRarityCheckBox.TextColor = labelColor;
            lblVeryRareRarityCheckBox.TextColor = labelColor;
            lblLegendaryRarityCheckBox.TextColor = labelColor;
            lblArtifactRarityCheckBox.TextColor = labelColor;


            Preferences.Set("MagicItemsEnabled", toggle);
        }
        private void OnRarityCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender == CommonRarityCheckBox)
                Preferences.Set("IncludeCommon", e.Value);
            else if (sender == UncommonRarityCheckBox)
                Preferences.Set("IncludeUncommon", e.Value);
            else if (sender == RareRarityCheckBox)
                Preferences.Set("IncludeRare", e.Value);
            else if (sender == VeryRareRarityCheckBox)
                Preferences.Set("IncludeVeryRare", e.Value);
            else if (sender == LegendaryRarityCheckBox)
                Preferences.Set("IncludeLegendary", e.Value);
            else if (sender == ArtifactRarityCheckBox)
                Preferences.Set("IncludeArtifact", e.Value);
        }

        private void OnCommonItemsCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            bool isChecked = e.Value;
            ToggleCommonItems(isChecked);
        }

        private void ToggleCommonItems(bool toggle)
        {
            CommonItemsEntry.IsEnabled = toggle;
            CommonItemsEntryMax.IsEnabled = toggle;
            CommonItemsRangeCheckBox.IsEnabled = toggle;

            Color labelColor = Colors.Gray;
            if (Application.Current.Resources.TryGetValue(toggle ? "TextColor" : "DisabledTextColor", out var resource))
            {
                labelColor = (Color)resource;
            }

            lblCommonItemsRangeCheckBox.TextColor = labelColor;
            CommonItemsEntry.TextColor = labelColor;
            lblNumCommonItems.TextColor = labelColor;
            lblNumCommonItemsAnd.TextColor = labelColor;
            lblNumCommonItems2.TextColor = labelColor;
            Preferences.Set("CommonItemsEnabled", toggle);
        }

        private void OnMagicItemsRangeCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            MagicItemsEntryMax.IsVisible = e.Value;
            lblNumMagicItemsAnd.IsVisible = e.Value;
            lblNumMagicItemsBetween.IsVisible = e.Value;

            Preferences.Set("MagicItemsUseRange", e.Value);
        }

        private void OnCommonItemsRangeCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            CommonItemsEntryMax.IsVisible = e.Value;
            lblNumCommonItemsAnd.IsVisible = e.Value;
            lblNumCommonItemsBetween.IsVisible = e.Value;

            Preferences.Set("CommonItemsUseRange", e.Value);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry)
            {
                int value;
                if (int.TryParse(entry.Text, out value))
                {
                    if (entry == MinGoldEntry)
                        Preferences.Set("MinGold", value);
                    else if (entry == MaxGoldEntry)
                        Preferences.Set("MaxGold", value);
                    else if (entry == MagicItemsEntry)
                        Preferences.Set("MagicItemsPerSpin", value);
                    else if (entry == MagicItemsEntryMax)
                        Preferences.Set("MagicItemsMaxPerSpin", value);
                    else if (entry == CommonItemsEntry)
                        Preferences.Set("CommonItemsPerSpin", value);
                    else if (entry == CommonItemsEntryMax)
                        Preferences.Set("CommonItemsMaxPerSpin", value);
                }
            }
        }

        private async void OnMagicItemsFilePickerClicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { ".json" } },
                    { DevicePlatform.iOS, new[] { ".json" } },
                    { DevicePlatform.WinUI, new[] { ".json" } }
                }),
                PickerTitle = "Select a JSON File for Magic Items"
            });

            if (result != null)
            {
                try
                {
                    var fileContent = await File.ReadAllTextAsync(result.FullPath);
                    var magicItems = JsonConvert.DeserializeObject<List<MagicItem>>(fileContent);
                    if (magicItems == null)
                    {
                        await DisplayAlert("Error", "Invalid JSON format.", "OK");
                        return;
                    }

                    var filePath = Path.Combine(FileSystem.Current.AppDataDirectory, "loot_table.json");

                    if (MagicItemsReplaceRadioButton.IsChecked)
                    {
                        File.WriteAllText(filePath, fileContent);
                    }
                    else if (MagicItemsAddRadioButton.IsChecked)
                    {
                        List<MagicItem> existingItems = new List<MagicItem>();

                        if (File.Exists(filePath))
                        {
                            var existingContent = File.ReadAllText(filePath);
                            existingItems = JsonConvert.DeserializeObject<List<MagicItem>>(existingContent) ?? new List<MagicItem>();
                        }

                        existingItems.AddRange(magicItems);
                        File.WriteAllText(filePath, JsonConvert.SerializeObject(existingItems, Formatting.Indented));
                    }

                    await DisplayAlert("Success", "Loot table has been updated successfully!", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to process the file: {ex.Message}", "OK");
                }
            }
        }

        private async void ShowToolTip(object sender, EventArgs e)
        {
            if (sender is ImageButton button && button.CommandParameter is string parameter)
            {
                string message = parameter switch
                {
                    "GoldRangeToolTip" => "Set the range for gold in loot boxes. A random amount of gold is selected from the selected range. Party level slightly weights the randomness factor. Check 'Enable Gold Range' to enable or disable the inclusion of gold in loot boxes.",
                    "MagicItemsToolTip" => "Configure the magic items included in loot boxes. Select 'Use Range' to include a random number of magic items from the selected range. Deselect 'Use Range' to include a fixed number. Select or deselect the specific item rarities to include/ exclude. Check the 'Enable Magic Items' to enable or disable the inclusion of magic items in the loot boxes",
                    "CommonItemsToolTip" => "Configure the non-magical items included in loot boxes. Select 'Use Range' to include a random number of non-magical items from the selected range. Deselect 'Use Range' to include a fixed number. Check the 'Enable Non-Magical Items' to enable or disable the inclusion of non-magical items in the loot boxes",
                    "LootTableToolTip" => "Select a json loot table to replace or append to the default loot table.",
                    _ => "No information available."
                };

                await DisplayAlert("Information", message, "OK");
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

        private async void NavigateToMain(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);
        }
    }
}
