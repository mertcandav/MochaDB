namespace rec utils

// Command processor for commands
type commandProcessor() =
  // Get namespace of command if exists
  static member splitNamespace(cmd:string) =
    cmd.Split(' ').[0]

  // Remove namespace from command
  static member removeNamespace(cmd:string) =
    cmd.Substring(commandProcessor.splitNamespace(cmd).Length).TrimStart()

