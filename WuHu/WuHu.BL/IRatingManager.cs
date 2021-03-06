﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.BL
{
    public interface IRatingManager
    {
        bool AddCurrentRatingFor(Player player);
        bool AddAllCurrentRatings();
        IList<Rating> GetAllRatings(int page);
        IList<Rating> GetAllRatingsFor(Player player);
        Rating GetCurrentRatingFor(Player player);
        Rating GetCurrentRatingFor(int playerId);
        int GetPageCount();
    }
}
