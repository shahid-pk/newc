using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
                file.WriteLine("using NewC.Scanner;");
                file.WriteLine();
                // write name space 
                file.WriteLine("namespace NewC.Parser");
                file.WriteLine("{");
                // write visitor interface
                DefineVisitor(file, baseName, types);
                file.WriteLine();
                // Write abstract base ast class
                file.WriteLine($"\tpublic abstract class {baseName}");
                file.WriteLine("\t{");
                file.WriteLine("\t\tpublic abstract T Accept<T>(IVisitor<T> visitor);");
                file.WriteLine("\t}");
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

            // Fields accessor                                                   
            foreach (var field in fields)
            {
                var type = field.Split(" ")[0];
                var name = field.Split(" ")[1];
                // try to make the property title case
                // which is close to c# styling's pascal case
                // for public properties
                var titleCase = new System.Globalization.CultureInfo("en-US", false).TextInfo.ToTitleCase(name.ToLower());

                file.WriteLine($"\t\tpublic {type} {titleCase} => {name};");
            }
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

            // Visitor pattern.                                      
            file.WriteLine();
            file.WriteLine($"\t\tpublic override T Accept<T>(IVisitor<T> visitor)");
            file.WriteLine("\t\t{");
            file.WriteLine($"\t\t\treturn visitor.Visit{className}{baseName}(this);");
            file.WriteLine("\t\t}");

            //end class
            file.WriteLine("\t}");
        }

        private static void DefineVisitor(StreamWriter file, string baseName, List<string> types)
        {
            file.WriteLine("\tpublic interface IVisitor<T>");
            file.WriteLine("\t{");

            foreach (var type in types)
            {
                var typeName = type.Split(":")[0].Trim();
                file.WriteLine($"\t\tT Visit{typeName}{baseName}({typeName} {baseName.ToLower()});");
            }

            file.WriteLine("\t}");
        }
    }
}
