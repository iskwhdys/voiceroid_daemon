// See https://aka.ms/new-console-template for more information
using AozoraBunko2Voiceroid2;

Console.WriteLine(args[0]);


TextConverter c = new TextConverter(args[0], args[0] + ".2.txt", args[0] + ".wdic");
c.Convert();