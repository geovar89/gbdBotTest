using System;
using Microsoft.Bot.Builder.FormFlow;

public enum LanguageOptions {Greek = 1, English};
public enum ResultOptions {All = 1, Five, Ten};
public enum OperationOptions {AND = 1, OR};

// For more information about this template visit http://aka.ms/azurebots-csharp-form
[Serializable]
public class BasicForm
{
    [Prompt("Hi! What is your {&}?")]
    public string Name { get; set; }

    [Prompt("Please select your language {||}")]
    public LanguageOptions Language { get; set; }

	[Prompt("What words do you want to search?Split different words with comma.")]
    public string Words { get; set; }
	
	[Prompt("Do you want results with all the words included or at least one{||}?")]
    public OperationOptions Operation { get; set; }
	
	[Prompt("How many results do you want to see{||}?")]
    public ResultOptions Results { get; set; }
	
    public static IForm<BasicForm> BuildForm()
    {
        // Builds an IForm<T> based on BasicForm
        return new FormBuilder<BasicForm>().Build();
    }

    public static IFormDialog<BasicForm> BuildFormDialog(FormOptions options = FormOptions.PromptInStart)
    {
        // Generated a new FormDialog<T> based on IForm<BasicForm>
        return FormDialog.FromForm(BuildForm, options);
    }
}
