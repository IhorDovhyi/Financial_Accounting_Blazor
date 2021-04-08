using Financial_Accounting_Blazor.Controllers;
using Financial_Accounting_Blazor.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace inancial_Accounting_Blazor_Tests
{
    [TestClass]
    public class MoneyTransactionControllerTest
    {
        public TestDB testDB;
        public TestUnitOfWork<TestDB> testUnitOfWork;

        [TestMethod]
        public void TestRepository_GetAllMethodTest_AllTransactionsExpected()
        {
            // Arrange
            testDB = new TestDB();
            var testRepository = new TestRepository(testDB);
            var expected = testDB.Transactions;

            // Act
            var actual = testRepository.GetAll().ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ClassIncomeUseCase_TestMethodGetIncome_AllIncomeTransactionsExpected()
        {
            // Arrange
            testDB = new TestDB();
            this.testUnitOfWork = new TestUnitOfWork<TestDB>(testDB);
            IncomeUseCase TestUseCase = new IncomeUseCase(testUnitOfWork);
            List<Transaction> expected = this.testUnitOfWork.GetAll().Where(x => x.sum >= 0).ToList();

            // Act
            UseCaseResult actual = JsonSerializer.Deserialize<UseCaseResult>(TestUseCase.Execute());

            // Assert
            Assert.AreEqual(expected.Count, actual.result.Count);
            Assert.IsTrue(actual.status);
            Assert.AreEqual(actual.messageCode, 200);
            Assert.AreEqual(actual.messageText, "OK");
        }

        [TestMethod]
        public void TestClassConsumptionUseCase_TestMethodGetConsumption_AllConsumptionTransactionsExpected()
        {
            // Arrange
            testDB = new TestDB();
            this.testUnitOfWork = new TestUnitOfWork<TestDB>(testDB);
            ConsumptionUseCase TestUseCase = new ConsumptionUseCase(testUnitOfWork);

            List<Transaction> expected = this.testUnitOfWork.GetAll().Where(x => x.sum < 0).ToList();

            // Act
            UseCaseResult actual = JsonSerializer.Deserialize<UseCaseResult>(TestUseCase.Execute());

            // Assert
            Assert.AreEqual(expected.Count, actual.result.Count);
            Assert.IsTrue(actual.status);
            Assert.AreEqual(actual.messageCode, 200);
            Assert.AreEqual(actual.messageText, "OK");
        }

        [TestMethod]
        public void TestClassPostUseCase_TestMethodPostTransaction_CorrectPostTransactionExpected()
        {
            // Arrange
            testDB = new TestDB();
            this.testUnitOfWork = new TestUnitOfWork<TestDB>(testDB);
            PostUseCase TestUseCase = new PostUseCase(testUnitOfWork);
            Transaction testTransaction = new Transaction() { sum = 12, DateTime = DateTime.Now };

            // Act
            bool actual = JsonSerializer.Deserialize<UseCaseResult>(TestUseCase.Execute(testTransaction)).status;
          
            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void TestClassPostUseCase_TestMethodPostTransactions_CorrectPostTransactionsExpected()
        {
            // Arrange
            testDB = new TestDB();
            this.testUnitOfWork = new TestUnitOfWork<TestDB>(testDB);
            PostUseCase TestUseCase = new PostUseCase(testUnitOfWork);
            Transaction testTransaction = new Transaction() { sum = 12, DateTime = DateTime.Now };

            // Act
            bool actual = JsonSerializer.Deserialize<UseCaseResult>(TestUseCase.Execute(testTransaction)).status;

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void TestClassPutUseCase_TestMethodPutTransaction_CorrectPutTransactionExpected()
        {
            // Arrange
            testDB = new TestDB();
            this.testUnitOfWork = new TestUnitOfWork<TestDB>(testDB);
            PutUseCase TestUseCase = new PutUseCase(testUnitOfWork);
            Transaction testTransaction = new Transaction { Id = 1, sum = 1221, DateTime = DateTime.Now };

            // Act
            bool actual = JsonSerializer.Deserialize<UseCaseResult>(TestUseCase.Execute(testTransaction)).status;

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void TestClassPutUseCase_TestMethodPutTransactions_CorrectPutTransactionsExpected()
        {
            // Arrange
            testDB = new TestDB();
            this.testUnitOfWork = new TestUnitOfWork<TestDB>(testDB);
            PutUseCase TestUseCase = new PutUseCase(testUnitOfWork);
            Transaction testTransaction = new Transaction { Id = 1, sum = 1221, DateTime = DateTime.Now };

            // Act
            bool actual = JsonSerializer.Deserialize<UseCaseResult>(TestUseCase.Execute(testTransaction)).status;

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void TestClassDeleteUseCase_TestMethodDeleteTransaction_CorrectDeleteTransactionExpected()
        {
            // Arrange
            testDB = new TestDB();
            this.testUnitOfWork = new TestUnitOfWork<TestDB>(testDB);
            DeleteUseCase TestUseCase = new DeleteUseCase(testUnitOfWork);
            int toDelete = 1;

            // Act
            bool actual = JsonSerializer.Deserialize<UseCaseResult>(TestUseCase.Execute(toDelete)).status;

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void TestClassDeleteUseCase_TestMethodDeleteTransactions_CorrectDeleteTransactionsExpected()
        {
            // Arrange
            testDB = new TestDB();
            this.testUnitOfWork = new TestUnitOfWork<TestDB>(testDB);
            DeleteUseCase TestUseCase = new DeleteUseCase(testUnitOfWork);
            int[] toDelete = new int[] { 1, 2 };

            // Act
            bool actual = JsonSerializer.Deserialize<UseCaseResult>(TestUseCase.Execute(toDelete)).status;

            // Assert
            Assert.IsTrue(actual);
        }
        [TestMethod]
        public void ClassIncomeUseCase_TestMethodGetIncomePerDate_CorrectIncomeDateExpected()
        {
            // Arrange
            testDB = new TestDB();
            this.testUnitOfWork = new TestUnitOfWork<TestDB>(testDB);
            IncomeUseCase TestUseCase = new IncomeUseCase(testUnitOfWork);
            DateTime testDate = new DateTime(2015, 7, 20, 18, 30, 25);
            // Act
            UseCaseResult actual = JsonSerializer.Deserialize<UseCaseResult>(TestUseCase.Execute(testDate.ToLocalTime()));

            // Assert
            Assert.IsTrue(DateTime.Compare(testDate.ToLocalTime(), actual.result[0].DateTime.ToLocalTime()) == 0);
            Assert.IsTrue(actual.result[0].sum > 0);
            Assert.IsTrue(actual.status);
            Assert.AreEqual(actual.messageCode, 200);
            Assert.AreEqual(actual.messageText, "OK");
        }
        [TestMethod]
        public void TestClassConsumptionUseCase_TestMethodGetConsumptionPerDate_CorrectConsumptionDateExpected()
        {
            // Arrange
            testDB = new TestDB();
            this.testUnitOfWork = new TestUnitOfWork<TestDB>(testDB);
            ConsumptionUseCase TestUseCase = new ConsumptionUseCase(testUnitOfWork);
            DateTime testDate = new DateTime(2020, 7, 10, 11, 30, 25);

            // Act
            UseCaseResult actual = JsonSerializer.Deserialize<UseCaseResult>(TestUseCase.Execute(testDate.ToLocalTime()));

            // Assert
            Assert.IsTrue(DateTime.Compare(testDate.ToLocalTime(), actual.result[0].DateTime.ToLocalTime()) == 0);
            Assert.IsTrue(actual.result[0].sum < 0);
            Assert.IsTrue(actual.status);
            Assert.AreEqual(actual.messageCode, 200);
            Assert.AreEqual(actual.messageText, "OK");
        }
        [TestMethod]
        public void TestClassGetOperationsUseCase_TestMethodGetOperationsPerDate_CorrectOperationsPerDateExpected()
        {
            // Arrange
            testDB = new TestDB();
            this.testUnitOfWork = new TestUnitOfWork<TestDB>(testDB);
            GetOperationsUseCase TestUseCase = new GetOperationsUseCase(testUnitOfWork);
            DateTime testDate = new DateTime(2020, 7, 10, 11, 30, 25);
            // Act
            int actual = JsonSerializer.Deserialize<UseCaseResult>(TestUseCase.Execute(testDate.ToLocalTime())).result.Count;
            // Assert
            Assert.AreEqual(1, actual);
        }
        [TestMethod]
        public void ClassIncomeUseCase_TestMethodGetIncomeForThePeriod_CorrectIncomeForThePeriodExpected()
        {
            // Arrange
            testDB = new TestDB();
            this.testUnitOfWork = new TestUnitOfWork<TestDB>(testDB);
            IncomeUseCase TestUseCase = new IncomeUseCase(testUnitOfWork);
            DateTime testStartDate = new DateTime(2015, 7, 20, 18, 30, 25);
            DateTime testEndDate = new System.DateTime(2021, 7, 10, 11, 30, 25);
         
            // Act
            UseCaseResult actual = JsonSerializer.Deserialize<UseCaseResult>(TestUseCase.Execute(testStartDate.ToLocalTime(), testEndDate.ToLocalTime()));

            // Assert
            Assert.IsTrue(DateTime.Compare(testStartDate.ToLocalTime(), actual.result[0].DateTime.ToLocalTime()) == 0 &&
                          DateTime.Compare(testEndDate.ToLocalTime(), actual.result[2].DateTime.ToLocalTime()) == 0);
            Assert.IsTrue(actual.result.All(x => x.sum > 0));
            Assert.IsTrue(actual.status);
            Assert.AreEqual(actual.messageCode, 200);
            Assert.AreEqual(actual.messageText, "OK");
        }
        [TestMethod]
        public void TestClassConsumptionUseCase_TestMethodGetConsumptionForThePeriod_CorrectGetConsumptionForThePeriodExpected()
        {
            // Arrange
            testDB = new TestDB();
            this.testUnitOfWork = new TestUnitOfWork<TestDB>(testDB);
            ConsumptionUseCase TestUseCase = new ConsumptionUseCase(testUnitOfWork);
            DateTime testStartDate = new DateTime(2020, 7, 10, 11, 30, 25);
            DateTime testEndDate = new System.DateTime(2020, 7, 10, 11, 30, 25);
            // Act
            UseCaseResult actual = JsonSerializer.Deserialize<UseCaseResult>(TestUseCase.Execute(testStartDate.ToLocalTime(), testEndDate.ToLocalTime()));

            // Assert
            Assert.IsTrue(DateTime.Compare(testStartDate.ToLocalTime(), actual.result[0].DateTime.ToLocalTime()) == 0 &&
                          DateTime.Compare(testEndDate.ToLocalTime(), actual.result[0].DateTime.ToLocalTime()) == 0);
            Assert.IsTrue(actual.result.All(x => x.sum < 0));
            Assert.IsTrue(actual.status);
            Assert.AreEqual(actual.messageCode, 200);
            Assert.AreEqual(actual.messageText, "OK");
        }
        [TestMethod]
        public void TestClassGetOperationsUseCase_TestMethodGetOperationsForThePeriode_CorrectGetOperationsForThePeriodeExpected()
        {
            // Arrange
            testDB = new TestDB();
            this.testUnitOfWork = new TestUnitOfWork<TestDB>(testDB);
            GetOperationsUseCase TestUseCase = new GetOperationsUseCase(testUnitOfWork);
            DateTime testStartDate = new DateTime(2015, 7, 20, 18, 30, 25);
            DateTime testEndDate = new DateTime(2021, 7, 10, 11, 30, 25);
            int expected = 4;
            // Act
            int actual = JsonSerializer.Deserialize<UseCaseResult>(TestUseCase.Execute(testStartDate.ToLocalTime(), testEndDate.ToLocalTime())).result.Count;
            // Assert
            Assert.IsTrue(expected == actual);
        }
    }
}
