using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Timers;
using HandySportFeed.Domain.Model;
using HandySportFeed.Domain.Repositories;
using HandySportFeed.Parsers;
using HandySportFeed.Parsers.LiveScoreMachesParser;

namespace HandySportFeed.WinService {
    public partial class HandySportFeed : ServiceBase {
        private Timer timer;
        private DateTime getTomorrowMathcesLastRun;

        private List<Tourney> tourneys;
        private List<MatchStatus> matchStatuses;
        private List<Match> todayMatches;
        private CommandRepository commandRepository;
        private MatchRepository matchRepository;
        
        public HandySportFeed() {
                InitializeComponent();
        }

        protected override void OnStart(string[] args) {
                commandRepository = new CommandRepository();
                matchRepository = new MatchRepository();
                tourneys = new TourneyRepository().FindAll().ToList();
                todayMatches = new MatchRepository().GetMatchesByDate(DateTime.Now);
                matchStatuses = new List<MatchStatus> {
                new MatchStatus { Id = 1, Name = "NS" },
                new MatchStatus { Id = 2, Name = "Live" },
                new MatchStatus { Id = 3, Name = "HT" },
                new MatchStatus { Id = 4, Name = "Canceled" },
                new MatchStatus { Id = 5, Name = "FT" }
            };
                if (new MatchRepository().GetMatchesByDate(DateTime.Now).Any())
                {
                    getTomorrowMathcesLastRun = DateTime.Now.Date + new TimeSpan(19, 0, 0);
                }
                else
                {
                    getTomorrowMathcesLastRun = DateTime.Now.AddDays(-1);
                }

                timer = new Timer(2 * 60 * 1000); // every 2 minutes
                timer.Elapsed += TimerElapsed;
                timer.Start();
           
        }

        protected override void OnStop() {
            timer.Stop();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e) {
            timer.Stop();
            if ((DateTime.Now - getTomorrowMathcesLastRun).TotalDays >= 1) {
                File.AppendAllText(@"C:\errors.txt", DateTime.Now + " calling ParseTomorrowMatches()" + Environment.NewLine);
                ParseTomorrowMatches();
            }
            var liveMatches = todayMatches.Where(p => p.Date >= DateTime.Now && p.Date.AddHours(2) <= DateTime.Now);
            if (liveMatches.Any()) {
                File.AppendAllText(@"C:\errors.txt", DateTime.Now + " calling UpdateMatches()" + Environment.NewLine);
                UpdateMatches();
            }
            timer.Start();
        }

        private void ParseTomorrowMatches() {
            var matches = new MatchesParser(new LiveScoreMatchesParserStrategy()).Parse("", DateTime.Now.AddDays(1)).ToList();
            File.AppendAllText(@"C:\errors.txt", "ParseTomorrowMatches() matches count = " + matches.Count() + Environment.NewLine);
            foreach (var match in matches)
            {
                var tourney = tourneys.FirstOrDefault(p => p.LiveScoreName == match.Tourney.LiveScoreName);
                if (tourney == null) {
                    File.AppendAllText(@"C:\errors.txt", "ParseTomorrowMatches() tourney == null " + match.Tourney.LiveScoreName + Environment.NewLine);
                    continue;
                }
                match.Tourney = tourney;
                match.TourneyId = tourney.Id;
                var homeCommand = commandRepository.FindByLiveScoreName(match.HomeCommand.LiveScoreName);
                if (homeCommand == null) {
                    File.AppendAllText(@"C:\errors.txt", "ParseTomorrowMatches() homeCommand == null " + match.HomeCommand.LiveScoreName + Environment.NewLine);
                    continue;
                }
                    
                match.HomeCommand = homeCommand;
                match.HomeCommandId = homeCommand.Id;
                var awayCommand = commandRepository.FindByLiveScoreName(match.AwayCommand.LiveScoreName);
                if (awayCommand == null) {
                    File.AppendAllText(@"C:\errors.txt", "ParseTomorrowMatches() awayCommand == null " + match.AwayCommand.LiveScoreName + Environment.NewLine);
                    continue;                    
                }
                match.AwayCommand = awayCommand;
                match.AwayCommandId = awayCommand.Id;
                var status = matchStatuses.FirstOrDefault(p => p.Name == match.MatchStatus.Name);
                if (status == null) {
                    File.AppendAllText(@"C:\errors.txt", "ParseTomorrowMatches() status == null " + match.MatchStatus.Name + Environment.NewLine);
                    continue;
                }
                match.MatchStatus = status;
                match.MatchStatusId = status.Id;
                matchRepository.Add(match);
            }
            getTomorrowMathcesLastRun = DateTime.Now;
        }

        private void UpdateMatches() {
            File.AppendAllText(@"C:\errors.txt", "UpdateMatches()" + Environment.NewLine);
        }
    }
}
