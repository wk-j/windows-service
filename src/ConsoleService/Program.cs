using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConsoleService {
    class Program {
        static async Task Main(string[] args) {
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
                var fileName = item.Replace("ConsoleService.Pack.", string.Empty);
                var path = Path.Combine(dir, fileName);

                var reader = new StreamReader(stream);
                var content = reader.ReadToEnd().Replace("{exe}", exeName);
                File.WriteAllText(path, content);
            }
        }
    }
}
