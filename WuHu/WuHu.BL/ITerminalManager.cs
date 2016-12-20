﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.BL
{
    public interface ITerminalManager : ICommonManager
    {
        bool Login(string username, string password);
        bool IsUserAuthenticated();
        void Logout();
        Credentials GetUserCredentials();
    }
}