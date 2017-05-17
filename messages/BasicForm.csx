using System;
using Microsoft.Bot.Builder.FormFlow;

public enum CarOptions { Convertible = 1, SUV, EV };
public enum LanguageOptions {Greek = el, English=en};

// For more information about this template visit http://aka.ms/azurebots-csharp-form
[Serializable]
public class BasicForm
{
    [Prompt("Hi! What is your {&}?")]
    public string Name { get; set; }

    [Prompt("Please select your language {||}")]
    public LanguageOptions Language { get; set; }

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
