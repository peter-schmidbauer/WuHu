﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.BL
{
    public interface IPlayerManager
    {
        bool AddPlayer(Player player, string username, string password);
        bool UpdatePlayer(Player player, string username, string password);
    }
}
