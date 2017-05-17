using System;
using Microsoft.Bot.Builder.FormFlow;

public enum LanguageOptions {Greek = 1, English};
public enum ResultOptions {5 = 1, 10, all};

// For more information about this template visit http://aka.ms/azurebots-csharp-form
[Serializable]
public class BasicForm
{
    [Prompt("Hi! What is your {&}?")]
    public string Name { get; set; }

    [Prompt("Please select your language {||}")]
    public LanguageOptions Language { get; set; }

	[Prompt("What words do you want to search?")]
    public string Words { get; set; }
	
	[Prompt("How many results do you want to see?")]
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
