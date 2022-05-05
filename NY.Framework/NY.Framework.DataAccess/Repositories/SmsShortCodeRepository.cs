﻿using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class SmsShortCodeRepository : ReadWriteRepositoryBase<SmsShortCode,int>,ISmsShortCodeRepository
    {
        public SmsShortCodeRepository(IDbContext dbcontext) 
            : base(dbcontext,new string[] { })
        {

        }

     
    }
}
