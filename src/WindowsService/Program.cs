using System;
using System.IO;
using System.Reflection;

namespace WindowsService {
    class Program {
        static void Main(string[] args) {
            if (args.Length != 1) {
                Console.WriteLine("Invalid arguments");
                Console.WriteLine(" wk-windows-service <APP-DIR>");
                return;
            }

            var dir = args[0];
            var exeName = new DirectoryInfo(dir).Name + ".exe";
            var asm = Assembly.GetEntryAssembly(); ;
            var names = asm.GetManifestResourceNames();
            foreach (var item in names) {
                var stream = asm.GetManifestResourceStream(item);
                var fileName = item.Replace("WindowsService.Pack.", string.Empty);
                var path = Path.Combine(dir, fileName);

                var reader = new StreamReader(stream);
                var content = reader.ReadToEnd().Replace("{exe}", exeName);
                File.WriteAllText(path, content);
            }
        }
    }
}
