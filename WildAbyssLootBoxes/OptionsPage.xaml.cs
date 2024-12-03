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
            AttachScrollHandler(MinGoldEntry);
            AttachHoverHandlers(MinGoldEntry);

            MaxGoldEntry.Text = Preferences.Get("MaxGold", 0).ToString();
            AttachScrollHandler(MaxGoldEntry);
            AttachHoverHandlers(MaxGoldEntry);

            MagicItemsEntry.Text = Preferences.Get("MagicItemsPerSpin", 0).ToString();
            AttachScrollHandler(MagicItemsEntry);
            AttachHoverHandlers(MagicItemsEntry);

            CommonItemsEntry.Text = Preferences.Get("CommonItemsPerSpin", 0).ToString();
            AttachScrollHandler(CommonItemsEntry);
            AttachHoverHandlers(CommonItemsEntry);

            MagicItemsEntryMax.Text = Preferences.Get("MagicItemsMaxPerSpin", 0).ToString();
            AttachScrollHandler(MagicItemsEntryMax);
            AttachHoverHandlers(MagicItemsEntryMax);

            CommonItemsEntryMax.Text = Preferences.Get("CommonItemsMaxPerSpin", 0).ToString();
            AttachScrollHandler(CommonItemsEntryMax);
            AttachHoverHandlers(CommonItemsEntryMax);

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
            if (toggle)
            {
                // Dynamically resolve TextColor based on AppThemeBinding
                labelColor = OptionsTitle.TextColor;
            }
            else if (Application.Current.Resources.TryGetValue("DisabledTextColor", out var resource))
            {
                labelColor = (Color)resource;
            }

            MinGoldEntry.TextColor = labelColor;
            MaxGoldEntry.TextColor = labelColor;
            lblNumGoldRange.TextColor = labelColor;
            lblNumCommonItemsAnd.TextColor = labelColor;
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
            if (toggle)
            {
                // Dynamically resolve TextColor based on AppThemeBinding
                labelColor = OptionsTitle.TextColor;
            }
            else if (Application.Current.Resources.TryGetValue("DisabledTextColor", out var resource))
            {
                labelColor = (Color)resource;
            }

            lblMagicItemsRangeCheckBox.TextColor = labelColor;
            MagicItemsEntry.TextColor = labelColor;
            lblNumMagicItems.TextColor = labelColor;
            lblNumMagicItemsBetween.TextColor = labelColor;
            lblNumMagicItemsAnd.TextColor = labelColor;
            lblNumMagicItems2.TextColor = labelColor;

            lblIncludeRarities.TextColor = labelColor;
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
            if (toggle)
            {
                labelColor = OptionsTitle.TextColor;
            }
            else if (Application.Current.Resources.TryGetValue("DisabledTextColor", out var resource))
            {
                labelColor = (Color)resource;
            }

            lblCommonItemsRangeCheckBox.TextColor = labelColor;
            CommonItemsEntry.TextColor = labelColor;
            lblNumCommonItems.TextColor = labelColor;
            lblNumCommonItemsBetween.TextColor = labelColor;
            lblNumCommonItemsAnd.TextColor = labelColor;
            lblNumCommonItems2.TextColor = labelColor;
            Preferences.Set("CommonItemsEnabled", toggle);
        }

        private void OnMagicItemsRangeCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            MagicItemsEntryMax.IsVisible = e.Value;
            lblNumMagicItemsAnd.IsVisible = e.Value;
            lblNumMagicItemsBetween.IsVisible = e.Value;

            ValidateField(MagicItemsEntryMax);
            ValidateField(MagicItemsEntry);

            Preferences.Set("MagicItemsUseRange", e.Value);
        }

        private void OnCommonItemsRangeCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            CommonItemsEntryMax.IsVisible = e.Value;
            lblNumCommonItemsAnd.IsVisible = e.Value;
            lblNumCommonItemsBetween.IsVisible = e.Value;

            ValidateField(CommonItemsEntryMax);
            ValidateField(CommonItemsEntry);

            Preferences.Set("CommonItemsUseRange", e.Value);
        }

        public static int? GetPositiveInteger(string input)
        {
            if (int.TryParse(input, out int result) && result > 0)
            {
                return result;
            }
            return null;
        }

        private void OnEntryTextUnfocused(object sender, FocusEventArgs e)
        {
            if(sender is Entry)
            {
                ValidateField((Entry)sender);
            }
        }

        private void ValidateField(Entry entry)
        {
            int value;
            if (int.TryParse(entry.Text, out value))
            {
                if (entry == MinGoldEntry)
                {
                    if (Preferences.Get("MaxGold", 1) < 1)
                    {
                        Preferences.Set("MaxGold", 1);
                        MaxGoldEntry.Text = "1";
                    }
                    if (Preferences.Get("MaxGold", 1) > 999999)
                    {
                        Preferences.Set("MaxGold", 999999);
                        MaxGoldEntry.Text = "999999";
                    }
                    if (value >= Preferences.Get("MaxGold", 1))
                    {
                        value = Preferences.Get("MaxGold", 1) - 1;
                        MinGoldEntry.Text = value.ToString();
                    }
                    if (value < 0 )
                    {
                        value = 0;
                        MinGoldEntry.Text = "0";
                    }
                    Preferences.Set("MinGold", value);
                }

                else if (entry == MaxGoldEntry)
                {
                    if (value < Preferences.Get("MinGold", 0))
                    {
                        value = Preferences.Get("MinGold", 0) + 1;
                        MaxGoldEntry.Text = value.ToString();
                    }
                    if (value > 999999)
                    {
                        value = 999999;
                        MaxGoldEntry.Text = "999999";
                    }
                    if (value < 0)
                    {
                        value = 0;
                        MaxGoldEntry.Text = "0";
                    }
                    Preferences.Set("MaxGold", value);
                }
                else if (entry == MagicItemsEntry && Preferences.Get("MagicItemsEnabled", false))
                {
                    if (Preferences.Get("MagicItemsMaxPerSpin", 1) < 1)
                    {
                        Preferences.Set("MagicItemsMaxPerSpin", 1);
                        MagicItemsEntryMax.Text = "1";
                    }
                    if (Preferences.Get("MagicItemsMaxPerSpin", 1) > 99)
                    {
                        Preferences.Set("MagicItemsMaxPerSpin", 99);
                        MagicItemsEntryMax.Text = "99";
                    }
                    if (value >= Preferences.Get("MagicItemsMaxPerSpin", 1) && MagicItemsRangeCheckBox.IsChecked)
                    {
                        value = Preferences.Get("MagicItemsMaxPerSpin", 1) - 1;
                        MagicItemsEntry.Text = value.ToString();
                    }
                    if (value > 99)
                    {
                        value = 99;
                        MagicItemsEntry.Text = "99";
                    }
                    if (value < 0)
                    {
                        value = 0;
                        MagicItemsEntry.Text = "0";
                    }
                    Preferences.Set("MagicItemsPerSpin", value);
                }
                else if (entry == MagicItemsEntryMax && Preferences.Get("MagicItemsEnabled", false) && Preferences.Get("MagicItemsUseRange", false))
                {
                    if (value < Preferences.Get("MagicItemsPerSpin", 0))
                    {
                        value = Preferences.Get("MagicItemsPerSpin", 0) + 1;
                        MagicItemsEntryMax.Text = value.ToString();
                    }
                    if (value > 99)
                    {
                        value = 99;
                        MagicItemsEntryMax.Text = "99";
                    }
                    if (value < 0)
                    {
                        value = 0;
                        MagicItemsEntryMax.Text = "0";
                    }
                    Preferences.Set("MagicItemsMaxPerSpin", value);
                }
                else if (entry == CommonItemsEntry && Preferences.Get("CommonItemsEnabled", false))
                {
                    if (Preferences.Get("CommonItemsMaxPerSpin", 1) < 1)
                    {
                        Preferences.Set("CommonItemsMaxPerSpin", 1);
                        CommonItemsEntryMax.Text = "1";
                    }
                    if (Preferences.Get("CommonItemsMaxPerSpin", 1) > 99)
                    {
                        Preferences.Set("CommonItemsMaxPerSpin", 99);
                        CommonItemsEntryMax.Text = "99";
                    }
                    if (value >= Preferences.Get("CommonItemsMaxPerSpin", 1) && CommonItemsRangeCheckBox.IsChecked)
                    {
                        value = Preferences.Get("CommonItemsMaxPerSpin", 1) - 1;
                        CommonItemsEntry.Text = value.ToString();
                    }
                    if (value > 99)
                    {
                        value = 99;
                        CommonItemsEntry.Text = "99";
                    }
                    if (value < 0)
                    {
                        value = 0;
                        CommonItemsEntry.Text = "0";
                    }
                    Preferences.Set("CommonItemsPerSpin", value);
                }
                else if (entry == CommonItemsEntryMax && Preferences.Get("CommonItemsEnabled", false) && Preferences.Get("CommonItemsUseRange", false))
                {
                    if (value < Preferences.Get("CommonItemsPerSpin", 0))
                    {
                        value = Preferences.Get("CommonItemsPerSpin", 0) + 1;
                        CommonItemsEntryMax.Text = value.ToString();
                    }
                    if (value > 99)
                    {
                        Preferences.Set("CommonItemsMaxPerSpin", 99);
                        CommonItemsEntryMax.Text = "99";
                    }
                    if (value < 0)
                    {
                        value = 99;
                        CommonItemsEntryMax.Text = "0";
                    }
                    Preferences.Set("CommonItemsMaxPerSpin", value);
                }
            } else
            {
                entry.Text = "0";
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
                    "GoldRangeToolTip" => "Set the range for gold in loot boxes. Minimum 1(or disabled), and maximum 999,999. A random amount of gold is selected from the selected range. Party level slightly weights the randomness factor. Check 'Enable Gold Range' to enable or disable the inclusion of gold in loot boxes.",
                    "MagicItemsToolTip" => "Configure the magic items included in loot boxes. Minimum 1(or disabled), and maximum 99. Select 'Use Range' to include a random number of magic items from the selected range. Deselect 'Use Range' to include a fixed number. Select or deselect the specific item rarities to include/ exclude. Check the 'Enable Magic Items' to enable or disable the inclusion of magic items in the loot boxes",
                    "CommonItemsToolTip" => "Configure the non-magical items included in loot boxes. Minimum 1(or disabled), and maximum 99. Select 'Use Range' to include a random number of non-magical items from the selected range. Deselect 'Use Range' to include a fixed number. Check the 'Enable Non-Magical Items' to enable or disable the inclusion of non-magical items in the loot boxes",
                    "LootTableToolTip" => "Select a json loot table to replace or append to the default loot table.",
                    _ => "No information available."
                };

                await DisplayAlert("Information", message, "OK");
            }
        }

        private void AttachScrollHandler(Entry entry)
        {
#if WINDOWS
            entry.HandlerChanged += (s, e) =>
            {
                var handler = entry.Handler.PlatformView as Microsoft.UI.Xaml.Controls.TextBox;
                if (handler != null)
                {
                    handler.PointerWheelChanged += (s, args) =>
                    {
                        int currentValue = int.TryParse(entry.Text, out int value) ? value : 0;

                        if (args.GetCurrentPoint(handler).Properties.MouseWheelDelta > 0)
                        {
                            currentValue++;
                        }
                        else
                        {
                            currentValue--;
                        }

                        entry.Text = currentValue.ToString();
                    };
                }
            };
#endif
        }

        private void AttachHoverHandlers(Entry entry)
        {
#if WINDOWS
            entry.HandlerChanged += (s, e) =>
            {
                var handler = entry.Handler.PlatformView as Microsoft.UI.Xaml.Controls.TextBox;
                if (handler != null)
                {
                    handler.PointerWheelChanged += (sender, args) =>
                    {
                        args.Handled = true;
                    };
                }
            };
#endif
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
