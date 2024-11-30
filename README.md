A loot box generation application for tabletop RPGs like Dungeons & Dragons 5e. Customize gold, magic items, and non-magic items, and configure loot tables with ease.

Features
--------

-   Randomized loot box generation.
-   Configurable gold, magic items, and non-magic items.
-   Adjustable party levels to balance rarity distribution.
-   Support for custom loot tables in JSON format.
-   A default loot table containing most D&D 5e items is compiled in the executable, so the application will work out of the box.
-   Built in GUI loot table editor.

Getting Started
---------------

### Pre-Compiled Executable
- WildAbyssLootBoxes.7z contains both a pre-compiled executable and a sample loot table.
- The sample loot table is a copy of the default - compiled loot table. The application will work out of the box.
- The sample loot table is only to demonstrate the formatting in the event that you want to curate your own loot table and update it through the application options page.
- Loot tables can also be edited from within the application through the GUI editor. 

### Prerequisites for Compiling

-   .NET MAUI environment set up.
-   Visual Studio 2022 or later.

### Installation

1.  Clone the repository:

    bash

    Copy code

    `https://github.com/sanerakuhn/Wild_Abyss_Loot_Boxes.git`

2.  Open the project in Visual Studio.
3.  Select your desired platform (Android, iOS, Windows).
4.  Build and run the application using `F5`.

For publishing, ```<PublishSingleFile>true</PublishSingleFile>``` and ```<WindowsPackageType>None</WindowsPackageType>``` need to be uncommented 
		and then run dotnet publish -f net8.0-windows10.0.19041.0 -r win10-x64 -c Release to generate a self contained executable.
		The executable should be located in \bin\Release\net8.0-windows10.0.19041.0\win10-x64\publish

Usage
-----

1.  Open the application.
2.  Configure loot boxes and party level on the Main Page.
3.  Click **Open** to generate loot items.
4.  Navigate to the **Options Page** from the toolbar to adjust settings:
    -   Enable or disable gold, magic items, or non-magic items.
    -   Define ranges or fixed quantities.
    -   Add or replace custom loot tables in JSON format.
5.  View generated loot in the results list.

Release build
-------------

A compressed archive with a pre-compiled executable and sample loot table has been provided in the root directory of this project. The application features a built-in default loot table. you can override or replace it from options with a loot table formatted like the sample.

Configuration
-------------

### Default Preferences

-   **Gold Range**: 10 (min), 1000 (max)
-   **Magic Items**: Enabled, fixed count of 1
-   **Non-Magic Items**: Enabled, fixed count of 1
-   **Rarities**:
    -   Common, Uncommon, Rare, Very Rare, Legendary, Artifact (all included by default)

### Loot Table Format
```
Custom loot tables must follow this JSON structure:

[  {    "Name": "Item Name",    "Rarity": "Item Rarity",    "Quantity": "Fixed or 'varies'",    "Variants": [      {        "Name": "Variant Name",        "Rarity": "Variant Rarity",        "Quantity": "Variant Quantity"      }    ]
  }
]
```
### Sample `loot_table.json`

```
[  {    "Name": "Wooden Shield",    "Rarity": "common",    "Quantity": 1  },  {    "Name": "Ring of Protection",    "Rarity": "uncommon",    "Quantity": 1  },  {    "Name": "Staff of Power",    "Rarity": "legendary",    "Quantity": "varies",    "Variants": [      {        "Name": "Fire Staff",        "Rarity": "legendary",        "Quantity": 1      },      {        "Name": "Ice Staff",        "Rarity": "legendary",        "Quantity": 1      },      {        "Name": "Earth Staff",        "Rarity": "legendary",        "Quantity": 1      }    ]
  }
]
```

License
-------

Wild Abyss Loot Boxes is licensed under the Apache License 2.0. For more details, see the `LICENSE` file in the repository.

Contributions
-------------

Contributions are welcome! To contribute:

1.  Fork the repository.
2.  Create a feature branch.
3.  Submit a pull request with a detailed description of your changes.

* * * * *
