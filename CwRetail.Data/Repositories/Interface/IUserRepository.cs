﻿using CwRetail.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwRetail.Data.Repositories.Interface
{
    public interface IUserRepository
    {
        int Insert(User user);
    }
}
