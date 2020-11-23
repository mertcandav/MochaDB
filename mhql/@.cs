using System.Collections.Generic;
using System.Text.RegularExpressions;

using MochaDB.Querying;

namespace MochaDB.mhql {
  /// <summary>
  /// MHQL @ mark.
  /// </summary>
  internal class Mhql_AT {
    /// <summary>
    /// Returns tag from command.
    /// </summary>
    /// <param name="command">Command.</param>
    /// <param name="final">Command of removed tag.</param>
    public static string GetAT(string command,out string final) {
      if(command.FirstChar() != '@') {
        final = command;
        return null;
      }

      Regex rgx = new Regex(@"($)|( )|(\n)");
      int finaldex = rgx.Match(command).Index+1;

      if(finaldex==0)
        throw new MochaException("@ mark is cannot processed!");

      string atcommand = command.Substring(0,finaldex).Trim();
      final = command.Substring(finaldex).Trim();
      return atcommand;
    }

    /// <summary>
    /// Returns tags from command.
    /// </summary>
    /// <param name="command">Command.</param>
    /// <param name="final">Command of removed tags.</param>
    public static List<string> GetATS(string command,out string final) {
      List<string> tags = new List<string>();
      do {
        string tag = GetAT(command,out command);
        if(string.IsNullOrEmpty(tag))
          continue;
        tags.Add(tag);
      } while(command.FirstChar() == '@');
      final = command;
      return tags;
    }
  }
}
