using System;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;

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
    [Optional]
    public OperationOptions Operation { get; set; }
	
	[Prompt("How many results do you want to see{||}?")]
    [Optional]
    public ResultOptions Results { get; set; }
	
    public static IForm<BasicForm> BuildForm()
    {
        // Builds an IForm<T> based on BasicForm
        return new FormBuilder<BasicForm>()
            .Field(nameof(Name))
            .Field(nameof(Language))
            .Field(nameof(Words))
            .Field(new FieldReflector<BasicForm>(nameof(Operation))
                .SetActive((state) => state.Words.IndexOf(",") > 0)
            )
            .Field(nameof(Results))
            .Build();
    }

    public static IFormDialog<BasicForm> BuildFormDialog(FormOptions options = FormOptions.PromptInStart)
    {
        // Generated a new FormDialog<T> based on IForm<BasicForm>
        return FormDialog.FromForm(BuildForm, options);
    }
}
