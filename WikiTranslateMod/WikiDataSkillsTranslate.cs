﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Eco.Core.Controller;
using Eco.Core.Plugins;
using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Gameplay.Blocks;
using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Players;
using Eco.Gameplay.Systems.Messaging.Chat.Commands;
using Eco.Gameplay.Systems.Messaging.Chat;
using Eco.Shared.Icons;
using Eco.Shared.Localization;
using Eco.Shared.Networking;
using Eco.Shared.Utils;
using Eco.Gameplay.Systems;
using Eco.Shared;
using Eco.Shared.IoC;
using Eco.Gameplay.Skills;
using Eco.Core.Items;
using Eco.Mods.WikiTranslateMod;
using System.Xml.Linq;

namespace Eco.Mods.EcoWikiDataExporter
{
	public partial class WikiData
    {
       
        public static void ExportSkillsTranslate()
        {
            string SkillsData = "";

            foreach (var skill in Skill.AllSkills)
            {
                SkillsData = SkillsData + "\"" + skill.Name + " Title\"," + WikiDataTranslateManager.Localization(skill.DisplayName) + "\n";
                SkillsData = SkillsData + "\"" + skill.Name + " Description\"," + WikiDataTranslateManager.Localization(WikiDataTranslateManager.CleanText(skill.GetDescription)) + "\n";
            }

            WikiDataTranslateManager.WriteDictionaryToCSV("SkillsTranslate", SkillsData);
        }


    }
}
