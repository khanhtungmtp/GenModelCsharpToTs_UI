using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;


namespace CtoTS
{
    public partial class AppCToTS : Form
    {
        public AppCToTS()
        {
            InitializeComponent();
            btnCopy.Enabled = false;
            // Assuming inputTextBoxCsharp is a Scintilla control
            inputTextBoxCsharp.TextChanged += inputTextBoxCsharp_TextChanged;

            // Initial check and update button visibility
            UpdateButtonVisibility();
        }

        private void btnGen_Click(object sender, EventArgs e)
        {
            // Get the C# code from the RichTextBox
            string cSharpCode = inputTextBoxCsharp.Text;

            // Use Roslyn to compile and read the C# class
            var syntaxTree = CSharpSyntaxTree.ParseText(cSharpCode);

            // Get the application's base directory
            string assemblyLocation = AppContext.BaseDirectory;

            // Get the path to System.Private.CoreLib.dll
            var coreLibPath = Path.Combine(assemblyLocation, "System.Private.CoreLib.dll");

            var compilation = CSharpCompilation.Create("TempAssembly")
                .AddReferences(MetadataReference.CreateFromFile(coreLibPath))
                .AddSyntaxTrees(syntaxTree);


            // Create a list to store TypeScript interfaces for all classes
            List<string> tsInterfaces = new();
            List<string> tsClassNames = new();

            // Loop through all class declarations in the syntax tree
            foreach (var classDeclaration in syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                // Get the original C# code for the current class
                var classSyntax = classDeclaration.SyntaxTree.GetRoot().FindNode(classDeclaration.Span);
                string cSharpCodeForClass = classSyntax.ToFullString();

                string className = classDeclaration.Identifier.Text;
                tsClassNames.Add(className);

                // Convert the C# class to TypeScript interface
                CodeConverter codeConverter = new CodeConverter();
                string tsInterface = codeConverter.ConvertCSharpClassToTypeScriptInterface(className, cSharpCodeForClass);

                // Add the TypeScript interface to the list
                tsInterfaces.Add(tsInterface);
            }

            // Check if any class names were found
            if (!tsClassNames.Any())
            {
                MessageBox.Show("Class name not found", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Combine all TypeScript interfaces into a single string
            string combinedInterfaces = string.Join("\n\n", tsInterfaces);

            // Create a directory for TypeScript files
            string folderTypeScript = Path.Combine(Directory.GetCurrentDirectory(), "Models");
            CreateDirectoryIfNotExists(folderTypeScript);

            // Determine the output TypeScript file name
            string nameFileOutput = CodeConverter.ConvertFirstCharToLowerCase(tsClassNames[0]);

            // Create the full path for the TypeScript output file
            string outputTypeScriptPath = Path.Combine(folderTypeScript, $"{nameFileOutput}.ts");

            // Write the TypeScript interfaces to the output file
            File.WriteAllText(outputTypeScriptPath, combinedInterfaces);
            inputTextBoxTypescript.Text = combinedInterfaces;

            // Enable the Copy button
            btnCopy.Enabled = true;
            btnCopy.Text = "Copy";

            MessageBox.Show($"TypeScript interface saved to: {nameFileOutput}.ts", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void inputTextBoxCsharp_Click(object sender, EventArgs e)
        {

        }

        private void inputTextBoxCsharp_TextChanged(object sender, EventArgs e)
        {
            UpdateButtonVisibility();
        }

        private void UpdateButtonVisibility()
        {
            // Set btnGen.Enabled based on inputTextBoxCsharp.Text
            btnGen.Enabled = !string.IsNullOrEmpty(inputTextBoxCsharp.Text);
        }

        private void AppCToTS_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            if (IsAppAlreadyRunning())
            {
                // mo lai ung dung neu dang chay
                Process currentProcess = Process.GetCurrentProcess();
                Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);
                foreach (Process process in processes)
                {
                    if (process.Id != currentProcess.Id)
                    {
                        NativeMethods.SetForegroundWindow(process.MainWindowHandle);
                        break;
                    }
                }

                this.Close(); // Đóng form hiện tại
            }
        }

        private bool IsAppAlreadyRunning()
        {
            Process currentProcess = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);
            return processes.Length > 1;
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
                if (parts[0].Length < 2)
                    parts[0] = parts[0].ToLower();
                else
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

        public static string ConvertFirstCharToLowerCase(string input)
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

        // Modify the ConvertCSharpClassToTypeScriptInterface method
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

    public class NativeMethods
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}