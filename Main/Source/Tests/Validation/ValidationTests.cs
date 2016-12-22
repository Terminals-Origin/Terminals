using System;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;
using Terminals.Data.Interfaces;
using Terminals.Data.Validation;

namespace Tests.Validation
{
    [TestClass]
    internal class ValidationTests
    {
        protected static string LongText { get; set; }

        [ClassInitialize]
        public static void InitializeLongText(TestContext context)
        {
            LongText = GenerateLongText();
        }

        private static string GenerateLongText()
        {
            StringBuilder longTextBuilder = new StringBuilder();
            for (int index = 0; index < 500; index++)
            {
                longTextBuilder.Append(index.ToString());
            }

            return longTextBuilder.ToString();
        }

        protected static void AssertGroupValidation(IDataValidator validator, IGroup group)
        {
            group.Name = String.Empty;
            var results = validator.Validate(group);
            Assert.AreEqual(1, results.Count, "Group name validation failed");
        }

        protected static void AssertCredentialsValidation(IDataValidator validator, ICredentialSet credentailSet, int expectedErrorsCount)
        {
            credentailSet.Name = LongText;
            var results = validator.Validate(credentailSet);
            Assert.AreEqual(expectedErrorsCount, results.Count(), "CredentailSet validation failed");
        }

        protected static void AssertNameOnlyValidation(IDataValidator validator, IGroup group)
        {
            ValidationStates results = validator.ValidateNameProperty(group);
            Assert.AreEqual(1, results.Count(), "Group name validation failed");
            Assert.AreEqual(Validations.NAME_PROPERTY, results.First().PropertyName, "Failed property is not a 'Name'");
        }
    }
}