using System;
using Xamarin.Forms;
using BookPollClientApp.Services;
using BookPollClientApp.Interfaces;
using System.Collections.Generic;
using BookPollClientApp.Models;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Threading;

namespace BookPollClientApp
{
    public partial class MainPage : ContentPage
    {
        // TODO: replace implementation
        readonly IPollQuestionService service = new MockPollQuestionService();

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (questions == null)
                await LoadQuestions();
        }

        async Task LoadQuestions()
        {
            IsBusy = true;

            try
            {
                // Add the questions
                questions = (await service.GetQuestionsAsync()).ToList();
                foreach (var q in questions)
                    questionPicker.Items.Add(q.Text);

                questionPicker.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Error", "Failed to download questions: "
                                            + ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public PollQuestion GetSelectedQuestion()
        {
            return (questionPicker.SelectedIndex >= 0)
                ? questions[questionPicker.SelectedIndex]
                : null;
        }

        private async void OnQuestionChanged(object sender, EventArgs e)
        {
            answerGroup.Children.Clear();

            var selectedQuestion = GetSelectedQuestion();
            if (selectedQuestion == null)
                return;

            try
            {
                IsBusy = true;

                string[] answers = selectedQuestion.Answers.Split('|');
                bool enabled = !string.IsNullOrEmpty(userName);
                for (int i = 0; i < answers.Length; i++)
                {
                    var answer = answers[i];
                    Button button = new Button { Text = answer, IsEnabled = enabled };
                    button.Clicked += OnSelectAnswer;
                    answerGroup.Children.Add(button);
                }

                // Get the answer from this user
                await GetSelectedAnswerAsync();
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Error", "Failed to download questions: " + ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnSelectAnswer(object sender, EventArgs e)
        {
            var selectedQuestion = GetSelectedQuestion();
            Debug.Assert(selectedQuestion != null);
            Debug.Assert(string.IsNullOrEmpty(userName) == false);

            int pos = answerGroup.Children.IndexOf((Button)sender);
            if (response == null)
            {
                response = new PollResponse {
                    Name = userName,
                    ResponseIndex = pos,
                    PollQuestionId = selectedQuestion.Id
                };
            }
            else
            {
                Debug.Assert(response.Name == userName);
                Debug.Assert(response.PollQuestionId == selectedQuestion.Id);
                response.ResponseIndex = pos;
            }
            await service.AddOrUpdatePollResponseAsync(response);

            SetSelectedAnswer();
        }


        private async void OnNameChanged(object sender, TextChangedEventArgs e)
        {
            userName = ((Entry)sender).Text.Trim();
            bool enabled = !string.IsNullOrEmpty(userName);
            foreach (Button button in answerGroup.Children)
                button.IsEnabled = enabled;

            await GetSelectedAnswerAsync();
        }

        private async Task GetSelectedAnswerAsync()
        {
            var selectedQuestion = GetSelectedQuestion();

            response = !string.IsNullOrEmpty(userName) && selectedQuestion != null
                ? await service.GetResponseForPollAsync(selectedQuestion.Id, userName)
                : null;

            SetSelectedAnswer();
        }

        private void SetSelectedAnswer()
        {
            int responseIndex = (response == null) ? -1 : response.ResponseIndex;

            for (int i = 0; i < answerGroup.Children.Count; i++)
            {
                Button button = (Button)answerGroup.Children[i];
                button.BackgroundColor = i == responseIndex
                            ? Color.FromHex("#90ff90")
                            : Color.Default;
            }
        }

        private async void OnDelete(object sender, EventArgs e)
        {
            if (response != null)
            {
                await service.DeletePollResponseAsync(response);
                response = null;
                SetSelectedAnswer();
            }
        }

        private async void OnShowResults(object sender, EventArgs e)
        {
            var selectedQuestion = GetSelectedQuestion();

            if (selectedQuestion == null)
                return;

            string[] answers = selectedQuestion.Answers.Split('|');

            // Populate the results asynchronously -- do not await this call or 
            // it will become synchronous with respect to the push to the next screen.
            var results = new ObservableCollection<Tuple<string,string>>();
            var fillDataTask = service.GetResponsesForPollAsync(selectedQuestion.Id);

            var t1 = fillDataTask.ContinueWith(tr => {
                    foreach (var r in tr.Result)
                    {
                        if (r.ResponseIndex >= 0 && r.ResponseIndex < answers.Length)
                            results.Add(Tuple.Create(r.Name, answers[r.ResponseIndex]));
                    }
                }, 
                CancellationToken.None, 
                TaskContinuationOptions.OnlyOnRanToCompletion, 
                TaskScheduler.FromCurrentSynchronizationContext());

            // If our async fill fails, then show an error.
            var t2 = fillDataTask.ContinueWith(async tr => {
                await this.DisplayAlert("Error", "Failed to download response: " + tr.Exception.Message, "OK");

            }, TaskContinuationOptions.OnlyOnFaulted);

            // And display the results page.
            await Navigation.PushAsync(new ResultsPage { BindingContext = new { Question = selectedQuestion.Text, Answers = results } });
        }

        #region Data
        private List<PollQuestion> questions;
        private string userName;
        private PollResponse response;
        #endregion

    }
}
