﻿using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.Dal.Test
{
    [TestClass]
    public class ScoreParameterTests
    {
        private static IDatabase database;
        private static IScoreParameterDao paramDao;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            database = DalFactory.CreateDatabase();
            paramDao = DalFactory.CreateScoreParameterDao(database);
        }
        
        private string GenerateName()
        {
            return Guid.NewGuid().ToString().Substring(0, 20); //random 20 character string
        }

        [TestMethod]
        public void Constructor()
        {
            Assert.IsNotNull(paramDao);

            var id = GenerateName();
            var param = new ScoreParameter(id, "val");
            Assert.IsNotNull(param);
        }

        [TestMethod]
        public void FindById()
        {
            var id = GenerateName();
            var param = new ScoreParameter(id, "val");
            paramDao.Insert(param);
            var foundParam = paramDao.FindById(id);

            Assert.AreEqual(param.Value, foundParam.Value);
            Assert.AreEqual(param.Key, foundParam.Key);


            var nullParam = paramDao.FindById("");
            Assert.IsNull(nullParam);
        }

        [TestMethod]
        public void FindAll()
        {
            var foundInitial = paramDao.FindAll().Count;

            const int insertAmount = 10;

            for (var i = 0; i < insertAmount; ++i)
            {
                var id = GenerateName();
                var param = new ScoreParameter(id, "val");
                paramDao.Insert(param);
            }

            var foundAfterInsert = paramDao.FindAll().Count;
            Assert.AreEqual(insertAmount + foundInitial, foundAfterInsert);
        }
        
        [TestMethod]
        public void Insert()
        {
            var id = GenerateName();
            var param = new ScoreParameter(id, "val");
            paramDao.Insert(param);

            var foundParam = paramDao.FindById(id);

            Assert.AreEqual(param.Value, foundParam.Value);
            Assert.AreEqual(param.Key, foundParam.Key);
        }

        [TestMethod]
        public void Update()
        {
            var id = GenerateName();
            var param = new ScoreParameter(id, "val");
            paramDao.Insert(param);

            var newValue = "newVal";
            param.Value = newValue;
            paramDao.Update(param);

            param = paramDao.FindById(id);
            Assert.AreEqual(newValue, param.Value);
        }
    }
}
