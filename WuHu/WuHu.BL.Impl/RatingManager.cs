﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.BL.Impl
{
    public class RatingManager : IRatingManager
    {
        protected readonly IMatchDao MatchDao;
        protected readonly IPlayerDao PlayerDao;
        protected readonly ITournamentDao TournamentDao;
        protected readonly IScoreParameterDao ParamDao;
        protected readonly IRatingDao RatingDao;
        protected readonly Random Rand = new Random();
        protected bool TournamentLocked;

        public RatingManager()
        {
            var database = DalFactory.CreateDatabase();
            MatchDao = DalFactory.CreateMatchDao(database);
            PlayerDao = DalFactory.CreatePlayerDao(database);
            RatingDao = DalFactory.CreateRatingDao(database);
            TournamentDao = DalFactory.CreateTournamentDao(database);
            ParamDao = DalFactory.CreateScoreParameterDao(database);
        }

        public bool AddAllCurrentRatings()
        {
            var players = PlayerDao.FindAll();
            foreach (var player in players)
            {
                AddCurrentRatingFor(player);
            }

            return true;
        }

        public bool AddCurrentRatingFor(Player player)
        {
            if (player.PlayerId == null)
            {
                return false;
            }

            var kRating = int.Parse(ParamDao.FindById("kRating")
                ?.Value ?? DefaultParameter.KRating);
            var halflife = int.Parse(ParamDao.FindById("halflife")
                ?.Value ?? DefaultParameter.Halflife);
            var initialScore = int.Parse(ParamDao.FindById("initialScore")
                ?.Value ?? DefaultParameter.InitialScore);
            var scoredMatches = int.Parse(ParamDao.FindById("scoredMatches")
                ?.Value ?? DefaultParameter.ScoredMatches);

            if (RatingDao.FindCurrentRating(player) == null)
            {
                RatingDao.Insert(new Rating(player, DateTime.Now, initialScore));
                return true;
            }

            var matches = MatchDao.FindAllByPlayer(player)
                .OrderByDescending(m => m.Datetime)
                .Where(m => m.IsDone)
                .Take(scoredMatches)
                .ToList();

            var rating = initialScore;
            var matchNr = matches.Count() - 1;
            foreach (var match in matches)
            {
                if (match.ScoreTeam1 == null || match.ScoreTeam2 == null)
                {
                    throw new ArgumentException("Player has finished matches without a score");
                }

                var multiplier = Math.Pow(0.5, (double)matchNr / halflife);

                int playerWon;
                double winChance;

                if (player.PlayerId == match.Player1.PlayerId ||
                    player.PlayerId == match.Player2.PlayerId)
                {
                    playerWon = match.ScoreTeam1.Value > match.ScoreTeam2.Value ? 1 : 0;
                    winChance = match.EstimatedWinChance;
                }
                else
                {
                    playerWon = match.ScoreTeam1.Value < match.ScoreTeam2.Value ? 1 : 0;
                    winChance = 1 - match.EstimatedWinChance;
                }

                var deltaScore = kRating * (playerWon - winChance);
                rating += (int)Math.Floor(multiplier * deltaScore);
                --matchNr;
            }

            return RatingDao.Insert(new Rating(player, DateTime.Now, rating));
        }

        public IList<Rating> GetAllRatings(int page)
        {
            return RatingDao.FindAll(page);
        }

        public IList<Rating> GetAllRatingsFor(Player player)
        {
            return RatingDao.FindAllByPlayer(player);
        }

        public Rating GetCurrentRatingFor(int playerId)
        {
            return RatingDao.FindCurrentRating(playerId);
        }

        public Rating GetCurrentRatingFor(Player player)
        {
            return RatingDao.FindCurrentRating(player);
        }

        public int GetPageCount()
        {
            return RatingDao.PageCount();
        }
    }
}
