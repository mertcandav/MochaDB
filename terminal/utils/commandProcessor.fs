namespace utils

// Command processor for commands
type commandProcessor() =
  // Get namespace of command if exists
  static member splitNamespace(cmd:string) =
    cmd.Split(' ').[0]

