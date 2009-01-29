using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Umbraco.InteractionLayer.CodeBuilder.Properties;
namespace Umbraco.InteractionLayer.CodeBuilder
{
    public enum Language
    {
        VB, CSharp
    }

    public class CodeCreator
    {
        public Language GenerationLanaguage { get; set; }
        public int[] DocTypesToGen { get; set; }
        public string Namespace { get; set; }

        public CodeCreator() { }

        private static CodeAssignStatement IsDirtyAssignment()
        {
            return new CodeAssignStatement(
                            new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "IsDirty"),
                            new CodePrimitiveExpression(true)
                        );
        }

        public void GetDocTypes()
        {
#if v4
            var umbDocTypes = umbraco.cms.businesslogic.web.DocumentType.GetAllAsList().Where(dt => DocTypesToGen.Contains(dt.Id));
#endif
#if v3
            var umbDocTypes = umbraco.cms.businesslogic.web.DocumentType.GetAll.Where(dt => DocTypesToGen.Contains(dt.Id));
#endif
            List<DocType> docTypes = new List<DocType>();
            foreach (var item in umbDocTypes)
            {
                DocType dt = new DocType
                {
                    Id = item.Id,
                    Name = item.Text,
                    OriginalName = item.Text,
                    DocTypeProperties = item.PropertyTypes.Select(pt => new Property
                    {
                        DataType = pt.GetDotNetType(),
                        Name = pt.Name,
                        Mandatory = pt.Mandatory,
                        Description = pt.Description,
                        Alias = pt.Alias,
                        ValidationRegex = pt.ValidationRegExp
                    }),
                    Alias = item.Alias.Replace(" ", "").Replace("-","_"),
                    Description = item.Description,
                    ChildContentTypes = item.AllowedChildContentTypeIDs
                };

                docTypes.Add(dt);
            }

            var d = new DirectoryInfo(Settings.Default.GeneratePath);
            if (!d.Exists)
            {
                d.Create();
            }

            foreach (var docType in docTypes)
            {
                string genName = docType.Alias;

                CodeCompileUnit currUnit = new CodeCompileUnit();

                CodeNamespace ns = GenerateNamespace(this.Namespace);
                currUnit.Namespaces.Add(ns);
                
                //create class
                CodeTypeDeclaration currClass = new CodeTypeDeclaration(genName);
                //create the custom attribute
                CodeAttributeDeclarationCollection classAttributes = new CodeAttributeDeclarationCollection(
                    new CodeAttributeDeclaration[] {
                        new CodeAttributeDeclaration("UmbracoDocTypeInfo",
                            new CodeAttributeArgument("Alias", new CodePrimitiveExpression(docType.Alias)),
                            new CodeAttributeArgument("Id", new CodePrimitiveExpression(docType.Id))),
                        new CodeAttributeDeclaration(new CodeTypeReference(typeof(DataContractAttribute), CodeTypeReferenceOptions.GlobalReference) )
                    });
                //add the address to the class
                currClass.CustomAttributes.AddRange(classAttributes);
                currClass.IsClass = true;
                //add the summary decoration
                currClass.Comments.AddRange(GenerateSummary(docType.Description));
                //set up the type
                currClass.TypeAttributes = System.Reflection.TypeAttributes.Public;
                currClass.BaseTypes.Add(new CodeTypeReference("DocTypeBase")); //base class
                currClass.IsPartial = true;

                currClass.Members.AddRange(GenerateConstructors());

                #region Doc Type Properties
                foreach (var docTypeProperty in docType.DocTypeProperties)
                {
                    CodeMemberField valueField = new CodeMemberField();
                    valueField.Attributes = MemberAttributes.Private;
                    valueField.Name = "_" + docTypeProperty.Alias;
                    valueField.Type = new CodeTypeReference(docTypeProperty.DataType);
                    currClass.Members.Add(valueField);

                    //store the Umbraco data in an attribute.
                    CodeMemberProperty p = new CodeMemberProperty();
                    p.CustomAttributes.AddRange(GenerateDocTypePropertyAttributes(docTypeProperty));

                    p.Name = docTypeProperty.Alias;
                    p.Type = new CodeTypeReference(docTypeProperty.DataType);
                    p.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                    p.HasGet = true;
                    p.HasSet = true;
                    p.GetStatements.Add(new CodeMethodReturnStatement(
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(), valueField.Name))
                        );

                    #region Set statement
                    //have a conditional statment so we can use the INotifyChanging and INotifyChanged events
                    CodeExpression left = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), docTypeProperty.Alias);
                    CodeExpression right = new CodePropertySetValueReferenceExpression();

                    CodeExpression cond = GenerateInequalityConditionalStatement(left, right);

                    //Build the statements to execute when we are changing the property value
                    //The order is:
                    // - RaisePropertyChanging event
                    // - Assign the property
                    // - Set IsDirty
                    // - RaisePropertyChanged event
                    var trues = new CodeStatement[] {
                        new CodeExpressionStatement(new CodeMethodInvokeExpression(
                            new CodeThisReferenceExpression(), 
                            "RaisePropertyChanging"
                            )
                        ),
                        new CodeAssignStatement(
                            new CodeFieldReferenceExpression(
                                new CodeThisReferenceExpression(), valueField.Name),
                            new CodePropertySetValueReferenceExpression()
                        ),
                        IsDirtyAssignment(),
                        new CodeExpressionStatement(
                            new CodeMethodInvokeExpression(
                                new CodeThisReferenceExpression(), 
                                "RaisePropertyChanged",
                                new CodePrimitiveExpression(docTypeProperty.Alias)
                            )
                        )
                    };

                    CodeConditionStatement condition = new CodeConditionStatement(cond, trues);
                    //enforce the validation from Umbraco. It's there for a reason ;)
                    if (!string.IsNullOrEmpty(docTypeProperty.ValidationRegex))
                    {
                        p.SetStatements.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(
                                        null,
                                        "ValidateProperty",
                                        new CodePrimitiveExpression(docTypeProperty.ValidationRegex),
                                        new CodePropertySetValueReferenceExpression())
                                        )
                                    );
                    }
                    p.SetStatements.Add(condition);
                    #endregion

                    //comment the property with the description from Umbraco
                    p.Comments.AddRange(GenerateSummary(docTypeProperty.Description));
                    currClass.Members.Add(p);
                }
                #endregion

                foreach (var child in docType.ChildContentTypes)
                {
                    var realDocType = docTypes.SingleOrDefault(dt => dt.Id == child);

                    //put a check that a docType is actually returned
                    //This will cater for the bug of when you don't select to generate a 
                    //docType but it is a child of the current
                    if (realDocType != null)
                    {
                        CodeMemberField childMember = new CodeMemberField();
                        string name = PluraliseName(realDocType.Alias);
                        childMember.Attributes = MemberAttributes.Private;
                        childMember.Name = "_" + name;
                        var t = new CodeTypeReference(typeof(IEnumerable<>));
                        t.TypeArguments.Add(realDocType.Alias);
                        childMember.Type = t;
                        currClass.Members.Add(childMember);

                        CodeMemberProperty p = new CodeMemberProperty();
                        p.Name = name;
                        p.Type = t;
                        p.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                        p.HasGet = true;
                        p.HasSet = false;

                        CodeExpression left = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), childMember.Name);
                        CodeExpression right = new CodePrimitiveExpression(null);

                        CodeExpression cond = GenerateEqualityConditionalStatement(left, right);

                        string hacketyHackHack;
                        if (this.GenerationLanaguage == Language.CSharp)
                        {
                            hacketyHackHack = "this._umbracoDocument.Children.Where(d => d.ContentType.Id == " + realDocType.Id + ").Select(d => new " + realDocType.Alias + "(d));";
                        }
                        else
                        {
                            hacketyHackHack = "Me._umbracoDocument.Children.Where(Function(d) d.ContentType.Id =  " + realDocType.Id + ").Select(Function(d) New " + realDocType.Alias + "(d))";
                        }

                        var trues = new CodeStatement[]{
                        new CodeAssignStatement(
                            new CodeFieldReferenceExpression(
                                new CodeThisReferenceExpression(), childMember.Name), 
                                new CodeSnippetExpression(hacketyHackHack)
                            ),
                    };
                        p.GetStatements.Add(new CodeConditionStatement(cond, trues));
                        p.GetStatements.Add(new CodeMethodReturnStatement(
                            new CodeFieldReferenceExpression(
                                new CodeThisReferenceExpression(), childMember.Name))
                            );

                        currClass.Members.Add(p); 
                    }
                }

                ns.Types.Add(currClass);

                switch (GenerationLanaguage)
                {
                    case Language.VB:
                        CreateVB(d, genName, currUnit);
                        break;
                    case Language.CSharp:
                    default:
                        CreateCSharp(d, genName, currUnit);
                        break;
                }
            }
        }

        private CodeExpression GenerateInequalityConditionalStatement(CodeExpression left, CodeExpression right)
        {
            //Build a binary conditional operation (an IF)
            CodeExpression cond;
            //if (GenerationLanaguage == Language.CSharp)
            //{
            cond = new CodeBinaryOperatorExpression(
                    left,
                    CodeBinaryOperatorType.IdentityInequality,
                    right
                );
            //}
            //else
            //{
            //    cond = new CodeBinaryOperatorExpression(
            //            new CodeBinaryOperatorExpression(
            //                left,
            //                CodeBinaryOperatorType.IdentityEquality,
            //                right
            //            ),
            //            CodeBinaryOperatorType.ValueEquality,
            //            new CodePrimitiveExpression(false)
            //        );
            //}
            return cond;
        }

        private CodeExpression GenerateEqualityConditionalStatement(CodeExpression left, CodeExpression right)
        {
            //Build a binary conditional operation (an IF)
            CodeExpression cond;
            //if (GenerationLanaguage == Language.CSharp)
            //{
            cond = new CodeBinaryOperatorExpression(
                    left,
                    CodeBinaryOperatorType.IdentityEquality,
                    right
                );
            //}
            //else
            //{
            //    cond = new CodeBinaryOperatorExpression(
            //            new CodeBinaryOperatorExpression(
            //                left,
            //                CodeBinaryOperatorType.IdentityInequality,
            //                right
            //            ),
            //            CodeBinaryOperatorType.ValueEquality,
            //            new CodePrimitiveExpression(false)
            //        );
            //}
            return cond;
        }

        private static CodeAttributeDeclaration[] GenerateDocTypePropertyAttributes(Property docTypeProperty)
        {
            CodeAttributeDeclaration umbInfoAtt = new CodeAttributeDeclaration("UmbracoFieldInfo",
                                    new CodeAttributeArgument("DisplayName", new CodePrimitiveExpression(docTypeProperty.Name)),
                                    new CodeAttributeArgument("Mandatory", new CodePrimitiveExpression(docTypeProperty.Mandatory)),
                                    new CodeAttributeArgument("IsCustom", new CodePrimitiveExpression(true)),
                                    new CodeAttributeArgument("Alias", new CodePrimitiveExpression(docTypeProperty.Alias))
                                    );
            CodeAttributeDeclaration dataMemberAtt = new CodeAttributeDeclaration(new CodeTypeReference(typeof(DataMemberAttribute), CodeTypeReferenceOptions.GlobalReference),
                         new CodeAttributeArgument("Name", new CodePrimitiveExpression(docTypeProperty.Alias))
                        );
            return new CodeAttributeDeclaration[] { umbInfoAtt, dataMemberAtt };
        }

        private static CodeTypeMember[] GenerateConstructors()
        {
            CodeConstructor defaultConstructor = new CodeConstructor();
            defaultConstructor.Attributes = MemberAttributes.Public;
            defaultConstructor.Statements.Add(IsDirtyAssignment());

            CodeConstructor idConstructor = new CodeConstructor();
            idConstructor.Attributes = MemberAttributes.Public;
            idConstructor.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "id"));

            //generate a constructor which calls the internal constructor which uses the Document object
            idConstructor.ChainedConstructorArgs.Add(
                new CodeObjectCreateExpression(new CodeTypeReference(typeof(umbraco.cms.businesslogic.web.Document), CodeTypeReferenceOptions.GlobalReference),
                    new CodePropertyReferenceExpression(null, "id")
                    )
                );

            //constructor from Unique ID
            CodeConstructor guidConstructor = new CodeConstructor();
            guidConstructor.Attributes = MemberAttributes.Public;
            guidConstructor.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Guid), "uniqueId"));
            guidConstructor.ChainedConstructorArgs.Add(
                new CodeObjectCreateExpression(new CodeTypeReference(typeof(umbraco.cms.businesslogic.web.Document), CodeTypeReferenceOptions.GlobalReference),
                    new CodePropertyReferenceExpression(null, "uniqueId")
                )
            );

            //private constructor which will also call the base constructor
            CodeConstructor internalConstructor = new CodeConstructor();
            internalConstructor.Attributes = MemberAttributes.FamilyOrAssembly;
            internalConstructor.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(umbraco.cms.businesslogic.web.Document), CodeTypeReferenceOptions.GlobalReference), "source"));
            internalConstructor.BaseConstructorArgs.Add(
                    new CodePropertyReferenceExpression(null, "source")
            );

            return new CodeTypeMember[] { defaultConstructor, idConstructor, guidConstructor, internalConstructor };
        }

        private static CodeNamespace GenerateNamespace(string name)
        {
            CodeNamespace ns = new CodeNamespace(name);
            //ns.Imports.Add(new CodeNamespaceImport("System"));
            ns.Imports.Add(new CodeNamespaceImport("Umbraco.InteractionLayer.Library"));
            ns.Imports.Add(new CodeNamespaceImport(typeof(IEnumerable<>).Namespace));
            ns.Imports.Add(new CodeNamespaceImport("System.Linq"));
            return ns;
        }

        private static CodeCommentStatement[] GenerateSummary(string summaryBody)
        {
            return new CodeCommentStatement[] {
                    new CodeCommentStatement("<summary>", true),
                    new CodeCommentStatement(summaryBody, true),
                    new CodeCommentStatement("</summary>", true)
                };
        }

        private static bool IsVowel(char c)
        {
            switch (c)
            {
                case 'O':
                case 'U':
                case 'Y':
                case 'A':
                case 'E':
                case 'I':
                case 'o':
                case 'u':
                case 'y':
                case 'a':
                case 'e':
                case 'i':
                    return true;
            }
            return false;
        }

        internal static string PluraliseName(string name)
        {
            if ((name.EndsWith("x", StringComparison.OrdinalIgnoreCase) || name.EndsWith("ch", StringComparison.OrdinalIgnoreCase)) || (name.EndsWith("ss", StringComparison.OrdinalIgnoreCase) || name.EndsWith("sh", StringComparison.OrdinalIgnoreCase)))
            {
                name = name + "es";
                return name;
            }
            if ((name.EndsWith("y", StringComparison.OrdinalIgnoreCase) && (name.Length > 1)) && !IsVowel(name[name.Length - 2]))
            {
                name = name.Remove(name.Length - 1, 1);
                name = name + "ies";
                return name;
            }
            if (!name.EndsWith("s", StringComparison.OrdinalIgnoreCase))
            {
                name = name + "s";
            }
            return name;
        }

        private static void CreateCSharp(DirectoryInfo d, string genName, CodeCompileUnit currUnit)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            using (StreamWriter sourceWriter = new StreamWriter(Path.Combine(d.FullName, genName + ".cs")))
            {
                provider.GenerateCodeFromCompileUnit(currUnit, sourceWriter, options);
            }
        }

        private static void CreateVB(DirectoryInfo d, string genName, CodeCompileUnit currUnit)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("VB");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            using (StreamWriter sourceWriter = new StreamWriter(Path.Combine(d.FullName, genName + ".vb")))
            {
                provider.GenerateCodeFromCompileUnit(currUnit, sourceWriter, options);
            }
        }
    }
}
