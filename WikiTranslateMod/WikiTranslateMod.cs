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
using Eco.Shared.Logging;

namespace Eco.Mods.WikiTranslateMod
{
	[LocDisplayName(nameof(WikiTranslateMod))]
    public class WikiTranslateMod : IModKitPlugin, IServerPlugin, IInitializablePlugin, ICommandablePlugin
    {
        public static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public const string WTMFolder = "WikiTranslate";
       
        public void Initialize(TimedTask timer)
		{
			
		}

        public string GetStatus() => string.Empty;

        public string GetCategory() => Localizer.DoStr("Mods");
        public override string ToString() => Localizer.DoStr("WTM");

        public void GetCommands(Dictionary<string, Action> nameToFunction)
        {
            nameToFunction.Add(Localizer.DoStr("Export Wiki Translate"), this.ExportWikiTranslate);
        }

		void ExportWikiTranslate()
		{

            Directory.CreateDirectory(WTMFolder);

            try { WikiData.ExportTranslate(); } catch (Exception e) { Log.WriteWarningLineLoc($"Export Translate error: {e.Message}"); };

        }
    }
}