﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportFieldBooking.Biz
{
    public interface IRepositoryWrapper
    {
        public User.IRepository User { get; }

    }
}
