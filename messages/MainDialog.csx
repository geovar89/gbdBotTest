#load "BasicForm.csx"

using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
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
                /*if(form.Language.ToString() == "Greek"){
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
				var responseString = await response.Content.ReadAsStringAsync();*/
                String responseString = "{'totalcount':'1','time':'123ms','results':[{'url':'wiki.softone.gr/arthro1','title':'titlos arthrou 1','description':'keimeno arthrou 1 pou periexei tis lekseis pou zitise o xristis'}]}";
                JObject json = JObject.Parse(responseString);
                var count = createResults(json);
                await context.PostAsync(count);
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

    public String createResults(JObject data)
    {
        String htmlStart = "<!DOCTYPE html><html><body>";
        String htmlEnd = "</body></html>";
        String body = "";
        var results = data["results"];
        String count = data["totalcount"].ToString();
        int myInt = System.Convert.ToInt32(count);
        String time = data["time"].ToString();
        String finalHtml = "";


        for (var i = 0; i < myInt; i++)
        {
            body = body + results[i]["url"].ToString() + "</br>";
        }
        finalHtml = htmlStart + body + htmlEnd;
        return finalHtml;
        //return finalHtml;
    }
}