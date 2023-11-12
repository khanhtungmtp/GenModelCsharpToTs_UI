using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;


namespace CtoTS
{
    public partial class AppCToTS : Form
    {
        public AppCToTS()
        {
            InitializeComponent();
            btnCopy.Enabled = false;
        }

        private void btnGen_Click(object sender, EventArgs e)
        {
            // Lấy đường dẫn đến thư mục chứa tệp tin thực thi của ứng dụng
            string assemblyLocation = System.AppContext.BaseDirectory;

            // Tạo đường dẫn đầy đủ đến tệp tin tạm thời C#
            string tempCSharpFilePath = Path.Combine(assemblyLocation, "TempAssembly", "TempCSharpFile.cs");

            // Kiểm tra xem thư mục TempAssembly có tồn tại không
            string tempAssemblyDirectory = Path.Combine(assemblyLocation, "TempAssembly");
            if (!Directory.Exists(tempAssemblyDirectory))
            {
                Directory.CreateDirectory(tempAssemblyDirectory);
            }

            // Lấy nội dung từ RichTextBox
            string cSharpCode = inputTextBoxCsharp.Text;

            // Tạo một tạm thời C# source file
            File.WriteAllText(tempCSharpFilePath, cSharpCode);

            // Sử dụng Roslyn để biên dịch và đọc class C#
            var syntaxTree = CSharpSyntaxTree.ParseText(cSharpCode);
            var compilation = CSharpCompilation.Create("TempAssembly")
                  .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                    .AddSyntaxTrees(syntaxTree);

            var semanticModel = compilation.GetSemanticModel(syntaxTree);

            var root = syntaxTree.GetRoot();
            var classDeclaration = root.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
            if (classDeclaration == null)
            {
                MessageBox.Show("Không tìm thấy class C# trong mã nguồn!");
                return;
            }

            string className = classDeclaration.Identifier.Text;
            CodeConverter codeConverter = new();
            string tsInterface = codeConverter.ConvertCSharpClassToTypeScriptInterface(className, semanticModel.SyntaxTree.ToString());

            // Lưu nội dung vào tệp tin .ts
            string folderTypeScipt = Path.Combine(Directory.GetCurrentDirectory(), "Models");
            CreateDirectoryIfNotExists(folderTypeScipt);
            string outputTypescriptPath = Path.Combine(folderTypeScipt, $"{className.ToLower()}.ts");
            File.WriteAllText(outputTypescriptPath, tsInterface);
            inputTextBoxTypescript.Text = tsInterface;
            if (inputTextBoxTypescript.Text != null)
            {
                btnCopy.Enabled = true;
                btnCopy.Text = "Copy";
            }
            MessageBox.Show($"TypeScript interface saved to: {className.ToLower()}.ts", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error{ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void inputTextBoxTypescript_Click(object sender, EventArgs e)
        {
            // Check if the caret is at the beginning of the line
            int currentLine = inputTextBoxTypescript.LineFromPosition(inputTextBoxTypescript.CurrentPosition);
            int lineStart = inputTextBoxTypescript.LineFromPosition(currentLine);

            if (inputTextBoxTypescript.CurrentPosition == lineStart)
            {
                // Hide the caret at the beginning of the line
                inputTextBoxTypescript.CaretWidth = 0;
            }
            else
            {
                // Show the caret elsewhere
                inputTextBoxTypescript.CaretWidth = 1;
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(inputTextBoxTypescript.Text);
            btnCopy.Text = "Copied";
        }
    }

    public class CodeConverter
    {
        public string ConvertToStandardPropertyNameTypeScript(string cSharpPropertyName)
        {
            StringBuilder result = new();
            if (IsAllUpperCase(cSharpPropertyName))
            {
                // co 2 truong hop
                /*
                 1: khong co dau _   ex: MTP
                 2: co dau _         ex: KHANHTUNG_MTP
                */
                if (cSharpPropertyName.Contains('_'))
                {
                    // Chuyển đổi chuỗi thành chữ thường và giữ nguyên chữ in hoa cuối cùng của phần truoc dấu '_' dau tien đến hết string
                    string resultString = ConvertToLowerCaseWithUppercaseFirstAfterUnderscore(cSharpPropertyName);
                    result.Append(resultString);
                }
                else
                {
                    result.Append(cSharpPropertyName.ToLower());
                }
            }
            else
            {
                string resultString = ConvertFirstCharToLowerCase(cSharpPropertyName);
                result.Append(resultString);
            }


            return result.ToString();
        }
        /// <summary>
        /// // Chuyển đổi chuỗi thành chữ thường và giữ nguyên chữ in hoa cuối cùng của phần truoc dấu '_' dau tien đến hết string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static string ConvertToLowerCaseWithUppercaseFirstAfterUnderscore(string input)
        {
            string[] parts = input.Split('_');

            if (parts.Length > 1)
            {
                parts[0] = ConvertLastCharToLowerCase(parts[0]);
                // Ghép các phần lại thành chuỗi mới
                return string.Join("_", parts);
            }

            return input;
        }

        static string ConvertLastCharToLowerCase(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                // Lấy chiều dài của chuỗi
                int length = input.Length;

                // Chuyển đổi chữ cái cuối cùng thành chữ thường
                char lastChar = char.ToUpper(input[length - 1]);

                // Tạo chuỗi mới bằng cách ghép chuỗi cũ và chữ cái cuối cùng đã chuyển đổi
                return input.Substring(0, length - 1).ToLower() + lastChar;
            }

            return input;
        }

        static string ConvertFirstCharToLowerCase(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                // Lấy chiều dài của chuỗi
                int length = input.Length;

                // Chuyển đổi chữ cái dau tien thành chữ thường
                char firstChar = char.ToLower(input[0]);

                // Tạo chuỗi mới bằng cách ghép chuỗi cũ và chữ cái dau tien đã chuyển đổi
                return firstChar + input.Substring(1, length - 1);
            }

            return input;
        }

        static bool IsAllUpperCase(string input)
        {
            return input == input.ToUpper();
        }

        public string ConvertCSharpTypeToTypeScript(string csharpType)
        {
            switch (csharpType)
            {
                case "int":
                case "float":
                case "double":
                case "decimal":
                    return "number";
                case "int?":
                case "float?":
                case "double?":
                case "decimal?":
                    return "number | null";
                case "string":
                    return "string";
                case "string?":
                    return "string | null";
                case "DateTime":
                    return "string | Date";
                case "DateTime?":
                    return "string | Date | null";
                case "bool":
                    return "boolean";
                default:
                    if (csharpType.StartsWith("List<") && csharpType.EndsWith(">"))
                    {
                        // Extract the class name from List<className>
                        string className = csharpType.Substring(5, csharpType.Length - 6);
                        // Convert to TypeScript array notation
                        return $"{className}[]";
                    }
                    return "any";
            }
        }

        public string ConvertCSharpPropertyToTypeScript(string csharpPropertyName, string csharpType)
        {
            string tsPropertyName = ConvertToStandardPropertyNameTypeScript(csharpPropertyName);
            string tsType = ConvertCSharpTypeToTypeScript(csharpType);

            return $"{tsPropertyName}: {tsType};";
        }

        public string ConvertCSharpClassToTypeScriptInterface(string className, string csharpClass)
        {
            // Phân tách các dòng trong mã nguồn C#
            string[] lines = csharpClass.Split('\n');

            // Dùng List để lưu trữ các thuộc tính
            List<string> tsProperties = new();

            // Quét qua từng dòng mã nguồn C# để tìm thuộc tính
            foreach (string line in lines)
            {
                // Sử dụng regex hoặc cách kiểm tra để xác định dòng có phải là khai báo thuộc tính
                if (line.Contains("public") && line.Contains("{ get; set; }"))
                {
                    // Tìm tên thuộc tính và kiểu dữ liệu từ dòng khai báo
                    string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    string csharpType = tokens[1];
                    string csharpPropertyName = tokens[2].Replace("{", "").Trim();

                    // Chuyển đổi thuộc tính C# sang TypeScript
                    string tsProperty = ConvertCSharpPropertyToTypeScript(csharpPropertyName, csharpType);
                    tsProperties.Add(tsProperty);
                }
            }

            // Ghép các thuộc tính TypeScript lại thành interface
            string tsInterface = $"export interface {className} {{\n\t{string.Join("\n\t", tsProperties)}\n}}";

            return tsInterface;
        }
    }
}