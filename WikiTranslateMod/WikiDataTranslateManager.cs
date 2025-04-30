using System;
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
using Eco.Simulation.Types;
using Eco.Core.Items;
using Eco.Gameplay.Skills;

namespace Eco.Mods.WikiTranslateMod
{
    internal class WikiDataTranslateManager
    {

        public static void WriteDictionaryToCSV(string filename, string data)
        {

            string path = @WikiTranslateMod.WTMFolder + $@"\" + filename + $@".csv";

            using (StreamWriter streamWriter = new StreamWriter(path, false))
            {
                streamWriter.WriteLine("\"Context\",\"English\",\"Gibberish\",\"French\",\"Spanish\",\"German\",\"Korean\",\"BrazilianPortuguese\",\"SimplifedChinese\",\"Russian\",\"Italian\",\"Portuguese\",\"Hungarian\",\"Japanese\",\"Norwegian\",\"Polish\",\"Dutch\",\"Romanian\",\"Danish\",\"Czech\",\"Swedish\",\"Ukrainian\",\"Greek\",\"Arabic\",\"Vietnamese\",\"Turkish\"");
                streamWriter.WriteLine(data);
                streamWriter.Close();
            }
        }

        public static string CleanText(string Text)
        {
            Regex regexTag = new Regex("<[^>]*>");
            Text = regexTag.Replace(Text, "");
            Regex regexFeed = new Regex("[\t\n\v\f\r]");
            Text = regexFeed.Replace(Text, "");
            Text = Text.Replace("'", "\\'");
            return Text;
        }

        public static string Localization(string Text)
        {
            string localizedString = "";
            localizedString = localizedString + "\"" + Localizer.LocalizeString(Text, SupportedLanguage.English) + "\",\"\",";

            if (Localizer.LocalizeString(Text, SupportedLanguage.French) == Localizer.LocalizeString(Text, SupportedLanguage.English)) { localizedString = localizedString + "\"\","; } else { localizedString = localizedString + "\"" + Localizer.LocalizeString(Text, SupportedLanguage.French) + "\","; }
            if (Localizer.LocalizeString(Text, SupportedLanguage.Spanish) == Localizer.LocalizeString(Text, SupportedLanguage.English)) { localizedString = localizedString + "\"\","; } else { localizedString = localizedString + "\"" + Localizer.LocalizeString(Text, SupportedLanguage.Spanish) + "\","; }
            if (Localizer.LocalizeString(Text, SupportedLanguage.German) == Localizer.LocalizeString(Text, SupportedLanguage.English)) { localizedString = localizedString + "\"\","; } else { localizedString = localizedString + "\"" + Localizer.LocalizeString(Text, SupportedLanguage.German) + "\","; }
            if (Localizer.LocalizeString(Text, SupportedLanguage.Korean) == Localizer.LocalizeString(Text, SupportedLanguage.English)) { localizedString = localizedString + "\"\","; } else { localizedString = localizedString + "\"" + Localizer.LocalizeString(Text, SupportedLanguage.Korean) + "\","; }
            if (Localizer.LocalizeString(Text, SupportedLanguage.BrazilianPortuguese) == Localizer.LocalizeString(Text, SupportedLanguage.English)) { localizedString = localizedString + "\"\","; } else { localizedString = localizedString + "\"" + Localizer.LocalizeString(Text, SupportedLanguage.BrazilianPortuguese) + "\","; }
            if (Localizer.LocalizeString(Text, SupportedLanguage.SimplifedChinese) == Localizer.LocalizeString(Text, SupportedLanguage.English)) { localizedString = localizedString + "\"\","; } else { localizedString = localizedString + "\"" + Localizer.LocalizeString(Text, SupportedLanguage.SimplifedChinese) + "\","; }
            if (Localizer.LocalizeString(Text, SupportedLanguage.Russian) == Localizer.LocalizeString(Text, SupportedLanguage.English)) { localizedString = localizedString + "\"\","; } else { localizedString = localizedString + "\"" + Localizer.LocalizeString(Text, SupportedLanguage.Russian) + "\","; }
            if (Localizer.LocalizeString(Text, SupportedLanguage.Italian) == Localizer.LocalizeString(Text, SupportedLanguage.English)) { localizedString = localizedString + "\"\","; } else { localizedString = localizedString + "\"" + Localizer.LocalizeString(Text, SupportedLanguage.Italian) + "\","; }
            if (Localizer.LocalizeString(Text, SupportedLanguage.Portuguese) == Localizer.LocalizeString(Text, SupportedLanguage.English)) { localizedString = localizedString + "\"\","; } else { localizedString = localizedString + "\"" + Localizer.LocalizeString(Text, SupportedLanguage.Portuguese) + "\","; }
            if (Localizer.LocalizeString(Text, SupportedLanguage.Hungarian) == Localizer.LocalizeString(Text, SupportedLanguage.English)) { localizedString = localizedString + "\"\","; } else { localizedString = localizedString + "\"" + Localizer.LocalizeString(Text, SupportedLanguage.Hungarian) + "\","; }
            if (Localizer.LocalizeString(Text, SupportedLanguage.Japanese) == Localizer.LocalizeString(Text, SupportedLanguage.English)) { localizedString = localizedString + "\"\","; } else { localizedString = localizedString + "\"" + Localizer.LocalizeString(Text, SupportedLanguage.Japanese) + "\","; }
            if (Localizer.LocalizeString(Text, SupportedLanguage.Norwegian) == Localizer.LocalizeString(Text, SupportedLanguage.English)) { localizedString = localizedString + "\"\","; } else { localizedString = localizedString + "\"" + Localizer.LocalizeString(Text, SupportedLanguage.Norwegian) + "\","; }
            if (Localizer.LocalizeString(Text, SupportedLanguage.Polish) == Localizer.LocalizeString(Text, SupportedLanguage.English)) { localizedString = localizedString + "\"\","; } else { localizedString = localizedString + "\"" + Localizer.LocalizeString(Text, SupportedLanguage.Polish) + "\","; }
            if (Localizer.LocalizeString(Text, SupportedLanguage.Dutch) == Localizer.LocalizeString(Text, SupportedLanguage.English)) { localizedString = localizedString + "\"\","; } else { localizedString = localizedString + "\"" + Localizer.LocalizeString(Text, SupportedLanguage.Dutch) + "\","; }
            if (Localizer.LocalizeString(Text, SupportedLanguage.Romanian) == Localizer.LocalizeString(Text, SupportedLanguage.English)) { localizedString = localizedString + "\"\","; } else { localizedString = localizedString + "\"" + Localizer.LocalizeString(Text, SupportedLanguage.Romanian) + "\","; }
            if (Localizer.LocalizeString(Text, SupportedLanguage.Danish) == Localizer.LocalizeString(Text, SupportedLanguage.English)) { localizedString = localizedString + "\"\","; } else { localizedString = localizedString + "\"" + Localizer.LocalizeString(Text, SupportedLanguage.Danish) + "\","; }
            if (Localizer.LocalizeString(Text, SupportedLanguage.Czech) == Localizer.LocalizeString(Text, SupportedLanguage.English)) { localizedString = localizedString + "\"\","; } else { localizedString = localizedString + "\"" + Localizer.LocalizeString(Text, SupportedLanguage.Czech) + "\","; }
            if (Localizer.LocalizeString(Text, SupportedLanguage.Swedish) == Localizer.LocalizeString(Text, SupportedLanguage.English)) { localizedString = localizedString + "\"\","; } else { localizedString = localizedString + "\"" + Localizer.LocalizeString(Text, SupportedLanguage.Swedish) + "\","; }
            if (Localizer.LocalizeString(Text, SupportedLanguage.Ukrainian) == Localizer.LocalizeString(Text, SupportedLanguage.English)) { localizedString = localizedString + "\"\","; } else { localizedString = localizedString + "\"" + Localizer.LocalizeString(Text, SupportedLanguage.Ukrainian) + "\","; }
            if (Localizer.LocalizeString(Text, SupportedLanguage.Greek) == Localizer.LocalizeString(Text, SupportedLanguage.English)) { localizedString = localizedString + "\"\","; } else { localizedString = localizedString + "\"" + Localizer.LocalizeString(Text, SupportedLanguage.Greek) + "\","; }
            if (Localizer.LocalizeString(Text, SupportedLanguage.Arabic) == Localizer.LocalizeString(Text, SupportedLanguage.English)) { localizedString = localizedString + "\"\","; } else { localizedString = localizedString + "\"" + Localizer.LocalizeString(Text, SupportedLanguage.Arabic) + "\","; }
            if (Localizer.LocalizeString(Text, SupportedLanguage.Vietnamese) == Localizer.LocalizeString(Text, SupportedLanguage.English)) { localizedString = localizedString + "\"\","; } else { localizedString = localizedString + "\"" + Localizer.LocalizeString(Text, SupportedLanguage.Vietnamese) + "\","; }
            if (Localizer.LocalizeString(Text, SupportedLanguage.Turkish) == Localizer.LocalizeString(Text, SupportedLanguage.English)) { localizedString = localizedString + "\"\""; } else { localizedString = localizedString + "\"" + Localizer.LocalizeString(Text, SupportedLanguage.Turkish) + "\""; }

            return localizedString;
        }

    }
}
