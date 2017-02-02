﻿using System.Collections.Generic;
using EA4S.Utilities;

namespace EA4S.Database.Management
{
    /// <summary>
    /// Custom JSON parser for MiniGameData
    /// </summary>
    public class MiniGameParser : DataParser<MiniGameData, MiniGameTable>
    {
        override protected MiniGameData CreateData(Dictionary<string, object> dict, DatabaseObject db)
        {
            var data = new MiniGameData();

            data.Code = ParseEnum<MiniGameCode>(data, dict["Id"]);
            data.Main = ToString(dict["Main"]);
            data.Variation = ToString(dict["Variation"]);
            data.Type = ParseEnum<MiniGameDataType>(data, dict["Type"]);
            data.Title_En = ToString(dict["Title_En"]);
            data.Title_Ar = ToString(dict["Title_Ar"]);
            data.Scene = ToString(dict["Scene"]);
            data.Available = ToString(dict["Status"]) == "active";
            data.AffectedPlaySkills = CustomParsePlaySkills(data, dict);

            return data;
        }

        protected override void RegenerateEnums(List<Dictionary<string, object>> rowdicts_list)
        {
            // TODO: should we generate also the MiniGameCode? Could be useful, but it could mess with the current inspector values. 
            // ExtractEnum(rowdicts_list, "Id", customEnumName: "MiniGameCode");
            ExtractEnum(rowdicts_list, "Type");
        }


        List<PlaySkill> CustomParsePlaySkills(MiniGameData data, Dictionary<string, object> dict)
        {
            var list = new List<PlaySkill>();

            foreach (var playSkill in GenericUtilities.SortEnums<PlaySkill>())
            {
                var key = "Skill" + playSkill;
                if (ToString(dict[key]) != "")
                {
                    list.Add(playSkill);
                }
            }

            return list;
        }
    }
}
