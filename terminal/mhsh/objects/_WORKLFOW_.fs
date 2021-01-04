namespace mhsh.objects

open System.Collections.Generic

/// <summary>
/// Workflow instance.
/// </summary>
type _WORKFLOW_() =
  /// <summary>
  /// Works of workflow.
  /// </summary>
  member val works:List<string> = new List<string>() with get, set
