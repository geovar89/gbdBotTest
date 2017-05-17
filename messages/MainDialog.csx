#load "BasicForm.csx"

using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;

/// This dialog is the main bot dialog, which will call the Form Dialog and handle the results
[Serializable]
public class MainDialog : IDialog<BasicForm>
{
	private static readonly HttpClient client = new HttpClient();
    public MainDialog()
    {
    }

    public Task StartAsync(IDialogContext context)
    {
        context.Wait(MessageReceivedAsync);
        return Task.CompletedTask;
    }

    public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
    {
        var message = await argument;
        context.Call(BasicForm.BuildFormDialog(FormOptions.PromptInStart), FormComplete);
    }

    private async Task FormComplete(IDialogContext context, IAwaitable<BasicForm> result)
    {
        try
        {
            var form = await result;
            if (form != null)
            {
			
                //await context.PostAsync("Thanks for completing the form! Just type anything to restart it.");
				var values = new Dictionary<string, string>
				{
				   { "op", "search" },
				   { "ver", "v01" },
				   { "lang", "el" },
				   { "words","πωλήσεις" }
				};

				var content = new FormUrlEncodedContent(values);
				var response = await client.PostAsync("http://wiki.softone.gr/main.ashx", content);
				var responseString = await response.Content.ReadAsStringAsync();
            
				//responseString = responseString.Replace("&","&amp");
				//responseString = responseString.Replace("<","&lt");
				//responseString = responseString.Replace(">","&lg");
				await context.PostAsync(form.Language+' '+responseString);
            }
            else
            {
                await context.PostAsync("Form returned empty response! Type anything to restart it.");
            }
        }
        catch (OperationCanceledException)
        {
            await context.PostAsync("You canceled the form! Type anything to restart it.");
        }

        context.Wait(MessageReceivedAsync);
    }
}