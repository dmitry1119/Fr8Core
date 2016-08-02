﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 14.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace PlanDirectory.Templates
{
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "d:\Dev\Fr8Core\Services\PlanDirectory\Templates\PlanTemplateDetailsTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "14.0.0.0")]
    public partial class PlanTemplateDetailsTemplate : PlanTemplateDetailsTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write(@"
<!DOCTYPE html>
<html>
<head>
    <link rel=""stylesheet"" href=""../bower_components/bootstrap/dist/css/bootstrap.min.css""/>
    <link rel=""stylesheet"" href=""../Content/metronic/components.css""/>

    <link href='https://fonts.googleapis.com/css?family=Francois+One' rel='stylesheet' type='text/css'>
    <link href='https://fonts.googleapis.com/css?family=Didact+Gothic' rel='stylesheet' type='text/css'>

    <link rel=""stylesheet"" href=""../Content/css/plan-category.css"" />
	<link rel=""stylesheet"" href=""../Content/css/plan-directory.css"" />

    <title>Fr8 - Plan Template Details - ");
            
            #line 17 "d:\Dev\Fr8Core\Services\PlanDirectory\Templates\PlanTemplateDetailsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(planTemplate.Name));
            
            #line default
            #line hidden
            this.Write(@"</title>
    <meta charset=""utf-8""/>
</head>
<body>
    <div class=""header-container"">
        <div class=""header"">
            <div class=""logo""></div>
        </div>
    </div>
    <div class=""container"">
        <h1>Plan Template Details - <em>");
            
            #line 27 "d:\Dev\Fr8Core\Services\PlanDirectory\Templates\PlanTemplateDetailsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(planTemplate.Name));
            
            #line default
            #line hidden
            this.Write("</em></h1>\r\n\t\t");
            
            #line 28 "d:\Dev\Fr8Core\Services\PlanDirectory\Templates\PlanTemplateDetailsTemplate.tt"
for (int i=0; i<5; i++)
		{
            
            #line default
            #line hidden
            this.Write("        <div class=\"row\">\r\n            <div class=\"col-md-12\">\r\n                <" +
                    "hr />\r\n            </div>\r\n        </div>\r\n        <div class=\"row\">\r\n          " +
                    "  <div class=\"col-md-12\">\r\n                <h3>V");
            
            #line 37 "d:\Dev\Fr8Core\Services\PlanDirectory\Templates\PlanTemplateDetailsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(i));
            
            #line default
            #line hidden
            this.Write(" Registered By ");
            
            #line 37 "d:\Dev\Fr8Core\Services\PlanDirectory\Templates\PlanTemplateDetailsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(i));
            
            #line default
            #line hidden
            this.Write("</h3>\r\n            </div>\r\n        </div>\r\n        <div class=\"row\">\r\n           " +
                    " <div class=\"col-md-12\">\r\n                <p>\r\n                    <em>");
            
            #line 43 "d:\Dev\Fr8Core\Services\PlanDirectory\Templates\PlanTemplateDetailsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(i));
            
            #line default
            #line hidden
            this.Write(@"</em>
                </p>
            </div>
        </div>
        <div class=""row"">
            <div class=""col-md-12"">
                <strong>Sample JSON</strong>
            </div>
        </div>		
        <div class=""row"">
            <div class=""col-md-12"">
                <pre>
");
            this.Write("\r\n                </pre>\r\n            </div>\r\n        </div>\r\n\t\t");
            
            #line 59 "d:\Dev\Fr8Core\Services\PlanDirectory\Templates\PlanTemplateDetailsTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("    </div>\r\n</body>\r\n");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 1 "d:\Dev\Fr8Core\Services\PlanDirectory\Templates\PlanTemplateDetailsTemplate.tt"

private global::Fr8.Infrastructure.Data.DataTransferObjects.PublishPlanTemplateDTO _planTemplateField;

/// <summary>
/// Access the planTemplate parameter of the template.
/// </summary>
private global::Fr8.Infrastructure.Data.DataTransferObjects.PublishPlanTemplateDTO planTemplate
{
    get
    {
        return this._planTemplateField;
    }
}


/// <summary>
/// Initialize the template
/// </summary>
public virtual void Initialize()
{
    if ((this.Errors.HasErrors == false))
    {
bool planTemplateValueAcquired = false;
if (this.Session.ContainsKey("planTemplate"))
{
    this._planTemplateField = ((global::Fr8.Infrastructure.Data.DataTransferObjects.PublishPlanTemplateDTO)(this.Session["planTemplate"]));
    planTemplateValueAcquired = true;
}
if ((planTemplateValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("planTemplate");
    if ((data != null))
    {
        this._planTemplateField = ((global::Fr8.Infrastructure.Data.DataTransferObjects.PublishPlanTemplateDTO)(data));
    }
}


    }
}


        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "14.0.0.0")]
    public class PlanTemplateDetailsTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
