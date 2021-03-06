﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Domain
{
    [DataContract(Namespace = "http://WuHu.Domain")]
    public class Match
    {
        public Match() { }

        public Match(int? matchId, Tournament tournament, DateTime datetime, 
            byte? scoreteam1, byte? scoreteam2, double estimatedWinChance, bool isDone,
            Player player1, Player player2, Player player3, Player player4)
        {
            if (player1.Equals(player2) || player1.Equals(player3) || player1.Equals(player4) ||
                player2.Equals(player3) || player2.Equals(player4) || player3.Equals(player4))
            {
                throw new ArgumentException("A Player can't play with or against himself");
            }
            if (estimatedWinChance > 1 || estimatedWinChance < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(estimatedWinChance),
                    "The estimated win chance should be between 0 and 1");
            }
            this.MatchId = matchId;
            this.Datetime = datetime;
            this.Tournament = tournament;
            this.ScoreTeam1 = scoreteam1;
            this.ScoreTeam2 = scoreteam2;
            this.EstimatedWinChance = estimatedWinChance;
            this.IsDone = isDone;
            this.Player1 = player1;
            this.Player2 = player2;
            this.Player3 = player3;
            this.Player4 = player4;
        }

        public Match(Tournament tournament, DateTime datetime, byte? scoreteam1, 
            byte? scoreteam2, double estimatedWinChance, bool isDone,
            Player player1, Player player2, Player player3, Player player4)
        {
            if (player1.Equals(player2) || player1.Equals(player3) || player1.Equals(player4) ||
                player2.Equals(player3) || player2.Equals(player4) || player3.Equals(player4))
            {
                throw new ArgumentException("A Player can't play with or against himself");
            }
            if (estimatedWinChance > 1 || estimatedWinChance < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(estimatedWinChance), 
                    "The estimated win chance should be between 0 and 1");
            }
            this.Datetime = datetime;
            this.Tournament = tournament;
            this.ScoreTeam1 = scoreteam1;
            this.ScoreTeam2 = scoreteam2;
            this.EstimatedWinChance = estimatedWinChance;
            this.IsDone = isDone;
            this.Player1 = player1;
            this.Player2 = player2;
            this.Player3 = player3;
            this.Player4 = player4;
        }


        [DataMember]
        [Required]
        public Player Player1 { get; set; }
        [DataMember]
        [Required]
        public Player Player2 { get; set; }
        [DataMember]
        [Required]
        public Player Player3 { get; set; }
        [DataMember]
        [Required]
        public Player Player4 { get; set; }
        [DataMember]
        [Required]
        public Tournament Tournament { get; set; }
        [DataMember]
        public int? MatchId { get; set; }
        [DataMember]
        [Required]
        public DateTime Datetime { get; set; }
        [DataMember]
        public byte? ScoreTeam1 { get; set; }
        [DataMember]
        public byte? ScoreTeam2 { get; set; }
        [DataMember]
        [Required]
        public double EstimatedWinChance { get; set; }
        [DataMember]
        [Required]
        public bool IsDone { get; set; }

    }
}

