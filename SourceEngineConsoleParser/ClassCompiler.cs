using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Windows.Forms;
using System.IO;

//Adapted from http://www.codeproject.com/Articles/10324/Compiling-code-during-runtime

namespace SourceEngineConsoleParser
{
    public static class ClassCompiler
    {
        static AppDomain newDomain;
        static Assembly assembly;
        public static Parser CompileExtensionMethod(String extensionName)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CSharpCodeProvider csp = new CSharpCodeProvider();
            CompilerParameters cp = new CompilerParameters();
            cp.OutputAssembly = Application.StartupPath + "\\" + extensionName + ".dll";
            cp.ReferencedAssemblies.Add("System.dll");
            cp.ReferencedAssemblies.Add("System.dll");
            cp.ReferencedAssemblies.Add("System.Data.dll");
            cp.ReferencedAssemblies.Add("System.Xml.dll");
            cp.ReferencedAssemblies.Add("mscorlib.dll");
            cp.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            cp.ReferencedAssemblies.Add("SourceEngineConsoleParser.exe");
            cp.WarningLevel = 3;

            cp.CompilerOptions = "/target:library /optimize";
            cp.GenerateExecutable = false;
            cp.GenerateInMemory = false;

            CompilerResults results = csp.CompileAssemblyFromFile(cp, Application.StartupPath + "\\" + extensionName + ".cs");
            System.Collections.Specialized.StringCollection sc = results.Output;
            if (results.Errors.Count > 0)
            {
                Program.logger.WriteLine("Unsuccessful compilation of " + extensionName + ".cs",Logger.LogLevel.Error);
                foreach (CompilerError ce in results.Errors)
                {
                    Program.logger.WriteLine(ce.ErrorNumber + ": " + ce.ErrorText,Logger.LogLevel.Error);
                }
            }
            else
            {
                Program.logger.WriteLine("Compiled " + extensionName + ".cs successfully",Logger.LogLevel.Success);
            }
            newDomain = AppDomain.CreateDomain("newDomain");
            newDomain.SetupInformation.ShadowCopyFiles = "true";
            byte[] rawAssembly = loadFile(Application.StartupPath + "\\" + extensionName + ".dll");
            assembly = newDomain.Load(rawAssembly, null);
           return (Parser)assembly.CreateInstance("SourceEngineConsoleParser." + extensionName);
            
        }

        private static byte[] loadFile(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open);
            byte[] buffer = new byte[(int)fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();
            fs = null;
            return buffer;
        }

        public static void FreeResources()
        {
            assembly = null;
            AppDomain.Unload(newDomain);
            newDomain = null;
        }
    }
}
