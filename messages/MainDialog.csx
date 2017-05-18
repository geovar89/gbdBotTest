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
                /*var data = "{'info':'total results 46, total time 45 ms','results' : [{'title':'<a><strong>Μπάρα εργασίας</strong></a>','description' : '<span>Κατά την επιλογή από το Menu της εφαρμογής οποιουδήποτε Object πχ. <span style='font-weight:bold;'>πελάτες</span>, είδη, <span style='font-weight:bold;'>πωλήσεις</span> κλπ η μπάρα εργασίας της εφαρμογής είναι κοινή και τυποποιημένη. Οι εργασίες και οι δυνατότητες που παρέχονται μέσω της μπάρας εργασιών αναλύονται παρακάτω :             Ευρετήριο (χειρισμοί, επιλογή και σχεδίαση)         Συνήθεις εγγραφές         Προβολή (χειρισμοί, επιλογή και σχεδίαση)        Νέα εγγραφή (& save photo)         Copy from buffer  --Αντιγραφή από τελευταία         Καταχώρηση         Ακύρωση</span>'},{'title'		: '<a><strong>Task bar</strong></a>','description' : '<span>Μία μπάρα εργασίας, που θα τη βρείτε σε όλη την Soft1 εφαρμογή, είναι η Task bar. Αυτή η Soft1 μπάρα εργασίας είναι κοινή και τυποποιημένη για όλα τα object της εφαρμογής πχ. <span style='font-weight:bold;'>Πελάτες</span>, Είδη Αποθήκης, <span style='font-weight:bold;'>Πωλήσεις</span> κλπ. Παρακάτω θα δείτε και τις περιγραφές των σχετικών κουμπιών.    Αναλυτικά κάθε επιλογή  Λίστα: Eκτέλεση της λίστας βάσει οθόνης φίλτρων.  Φίλτρα: Οθόνη φίλτρων επιλεγμένης λίστας  Ομαδοποίηση δεδομένων ευρετηρίων που υποστηρίζει την οριζόντια ή κάθετη ομαδοποίηση.  Συνήθεις εγγραφές: Δυνατότητα</span>'}]}";*/
				var content = new FormUrlEncodedContent(valuesObj);
				var response = await client.PostAsync("http://wiki.softone.gr/main.ashx", content);
				var responseString = await response.Content.ReadAsStringAsync();
			
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