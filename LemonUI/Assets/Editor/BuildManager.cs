using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildRule
{
    public string inPut;
    public string outPut;
    public List<string> pattens = new List<string>();
}

public class BuildManager
{
    static List<BuildRule> _rules = new List<BuildRule>();
    public static List<BuildRule> GetBuildRules(bool isReloadRule = false)
    {
        if (!isReloadRule && _rules != null && _rules.Count > 0)
            return _rules;

        _rules.Clear();

        var ruleFile = Application.dataPath + "/../buildrules.txt";
        var str = File.ReadAllText(ruleFile);
        var rules = str.Split('\n');
        foreach (var item in rules)
        {
            BuildRule rule = new BuildRule();
            var param = item.Split('#');
            if (param.Length < 3)
                continue;

            rule.inPut = param[0].Trim();
            rule.outPut = param[2].Trim();

            var pattens = param[1].Split(',');
            foreach (var patten in pattens)
            {
                rule.pattens.Add(patten.Trim());
            }

            _rules.Add(rule);
        }

        return _rules;
    }

    public static string[] GetAllFiles(string path, List<string> pattens)
    {
        DirectoryInfo folder = new DirectoryInfo(path);
        List<string> ret = new List<string>();
        foreach (var file in folder.GetFiles())
        {
            if (pattens.Contains(file.Extension))
            {
                ret.Add(file.Name);
            }
        }

        return ret.ToArray();
    }
}
