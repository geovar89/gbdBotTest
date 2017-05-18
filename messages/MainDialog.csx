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
			var lang = "";
			var words = "";
			var fixedWords = "";
            if (form != null)
            {
			
                //await context.PostAsync("Thanks for completing the form! Just type anything to restart it.");
				if(form.Language.ToString() == "Greek"){
					lang = "el";
				}
				else{
					lang = "en";
				}
				words = form.Words;
				if(words.IndexOf(",") > 0){
					string[] values = words.Split(new string[] { "," }, StringSplitOptions.None);
					foreach (var splitWord in values){
					    if (String.IsNullOrEmpty(fixedWords)){
							fixedWords = splitWord ;
					    } 
					    else{
    						fixedWords = fixedWords + ' ' + form.Operation.ToString()+ ' '+ splitWord ;
					    }
				    }
					words = fixedWords;
				}
				var valuesObj = new Dictionary<string, string>
				{
				   { "op", "search" },
				   { "ver", "v01" },
				   { "lang", lang },
				   { "words",words }
				};
                
				var content = new FormUrlEncodedContent(valuesObj);
				var response = await client.PostAsync("http://wiki.softone.gr/main.ashx", content);
				var responseString = await response.Content.ReadAsStringAsync();
            
				//responseString = responseString.Replace("&","&amp");
				//responseString = responseString.Replace("<","&lt");
				//responseString = responseString.Replace(">","&lg");
				await context.PostAsync(responseString);
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