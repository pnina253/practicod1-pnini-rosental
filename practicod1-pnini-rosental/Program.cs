using practicod1_pnini_rosental.Commands;
using practicod1_pnini_rosental.Services;
using System.CommandLine;

var rootCommand = new RootCommand("CLI tool");


var createRspCommand = CreateRspCommand.Create();



rootCommand.Add(BundleCommand.Create());
rootCommand.Add(createRspCommand);


rootCommand.Parse(args).Invoke();



