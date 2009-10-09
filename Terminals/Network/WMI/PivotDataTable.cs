using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace Terminals.Network.WMI
{
    class PivotDataTable
    {
        public static Dictionary<string, string> ConvertToNameValue(DataTable dataValues, int index)
        {
            Dictionary<string, string> nv = new Dictionary<string, string>();

            DataRow row = dataValues.Rows[index];
            //columns become the names
            foreach(System.Data.DataColumn col in dataValues.Columns)
            {
                nv.Add(col.ColumnName, row[col].ToString());
            }
            return nv;
        }
        private static void AddPropertyAndField(CodeTypeDeclaration classDec, Type DataType, string DataTypeString, string Name)
        {
            CodeMemberField field = null;
            if(DataType != null)
                field = new CodeMemberField(DataType, Name + "_field");
            else
                field = new CodeMemberField(DataTypeString, Name + "_field");

            classDec.Members.Add(field);
            CodeMemberProperty prop = new CodeMemberProperty();

            if(DataType != null)
                prop.Type = new CodeTypeReference(DataType);
            else
                prop.Type = new CodeTypeReference(DataTypeString);

            prop.Name = Name;
            prop.HasGet = true;
            prop.HasSet = false;
            prop.Attributes = MemberAttributes.Public;
            prop.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field.Name)));
            classDec.Members.Add(prop);
        }
        public static System.Reflection.Assembly CreateAssemblyFromDataTable(DataTable DataValues)
        {
            System.Random rnd = new Random();
            if(DataValues.TableName == null || DataValues.TableName == "") 
                DataValues.TableName = rnd.Next().ToString();

            CodeTypeDeclaration classDec = new CodeTypeDeclaration(DataValues.TableName);
            classDec.IsClass = true;


            CodeConstructor constructor = new CodeConstructor();
            constructor.Attributes = MemberAttributes.Public;
            classDec.Members.Add(classDec);


            foreach(System.Data.DataColumn col in DataValues.Columns)
            {
                AddPropertyAndField(classDec, col.DataType, "", col.ColumnName);
            }

            AddPropertyAndField(classDec, null, "System.Collections.Generic.List<" + DataValues.TableName + ">", "ListOf" + DataValues.TableName);

            using(CSharpCodeProvider provider = new CSharpCodeProvider())
            {
                ICodeGenerator generator = provider.CreateGenerator();
                CodeNamespace ns = new CodeNamespace("Terminals.Generated");
                ns.Types.Add((CodeTypeDeclaration)classDec);
                CodeGeneratorOptions options = new CodeGeneratorOptions();
                //options.BlankLinesBetweenMembers = true;
                string filename = System.IO.Path.GetTempFileName();
                using(System.IO.StreamWriter sw = new System.IO.StreamWriter(filename, false))
                {
                    generator.GenerateCodeFromNamespace(ns, sw, options);

                    ICodeCompiler icc = provider.CreateCompiler();

                    CompilerParameters compileParams = new CompilerParameters();
                    compileParams.GenerateExecutable = false;
                    compileParams.GenerateInMemory = true;

                    return icc.CompileAssemblyFromSource(compileParams, System.IO.File.ReadAllText(filename)).CompiledAssembly;
                }
            }
        }
        public static object CreateTypeFromDataTable(DataTable DataValues)
        {
            System.Reflection.Assembly asm = CreateAssemblyFromDataTable(DataValues);
            object instance = asm.CreateInstance(DataValues.TableName);
            return null;
        }
    }
}