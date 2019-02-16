using System;
using System.Collections.Generic;
using System.IO;

namespace NewC.Tools
{
    class Program
    {
        static int Main(string[] args)
        {
            if(args.Length != 1)
            {
                Console.WriteLine("usage: generate_ast <output directory>");
                return 1;
            }
            var outputDir = args[0];
            DefineAst(outputDir, "Expr", new List<string> {
                "Binary   : Expr left,Token op,Expr right",
                "Grouping : Expr expression",
                "Literal  : object value",
                "Unary    : Token op,Expr right"
            });
            return 0;
        }

        private static void DefineAst(string outputDir, string baseName, List<string> types)
        {
            var path = $"{outputDir}/{baseName}.cs";
            using (var file = new StreamWriter($"{path}", true))
            {
                file.WriteLine("//This file is auto generated, do not edit by hand.");
                file.WriteLine("//This file includes AST types for newc");
                file.WriteLine();
                // use lexer namespace
                file.WriteLine("using NewC.Lexer;");
                file.WriteLine();
                // write name space 
                file.WriteLine("namespace NewC.Scanner");
                file.WriteLine("{");
                // Write abstract base ast class
                file.WriteLine($"\tpublic abstract class {baseName}");
                file.WriteLine("\t{}");
                file.WriteLine();
                // Write the derived AST classes.                                     
                foreach (var type in types)
                {
                    var className = type.Split(":")[0].Trim();
                    var fieldList = type.Split(":")[1].Trim();
                    DefineType(file, baseName, className, fieldList);
                    file.WriteLine();
                }
                file.WriteLine("}");
            }
        }

        private static void DefineType(StreamWriter file, string baseName, string className, string fieldList)
        {
            // Store parameters in fields.                               
            var fields = fieldList.Split(",");

            //class name
            file.WriteLine($"\tpublic class {className} : {baseName}");
            file.WriteLine("\t{");

            // Fields.                                                   
            foreach (var field in fields)
            {
                file.WriteLine($"\t\tprivate {field};");
            }
            // empty line
            file.WriteLine();

            // Constructor.                                              
            file.WriteLine($"\t\tpublic {className}({fieldList})");
            file.WriteLine("\t\t{");

            foreach (var field in fields)
            {
                var name = field.Split(" ")[1];
                file.WriteLine($"\t\t\tthis.{name} = {name};");
            }
            // end constructor
            file.WriteLine("\t\t}");
            //end class
            file.WriteLine("\t}");
        }
    }
}
