using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;
using Terminals.Data.Validation;

namespace Tests.Validation
{
    /// <summary>
    /// We cant make the test classes directly derive from this one,
    /// because it will require make the type constraints also public.
    /// </summary>
    internal class NameValidatorTestLab<TItem>
        where TItem : class, IStoreIdEquals<TItem>, INamedItem
    {
        internal const string ORIGINAL_NAME = "ItemA";

        internal const string UNIQUE_NAME = "Unique";

        private readonly TItem original;

        private readonly string uniqueItemErrorMessage;

        private readonly NameValidator<TItem> validator;

        internal NameValidatorTestLab(NameValidator<TItem> validator, TItem original, string uniqueItemErrorMessage)
        {
            this.original = original;
            this.uniqueItemErrorMessage = uniqueItemErrorMessage;
            this.validator = validator;
        }

        internal void ValidateNotRenamed()
        {
            string resultMessage = this.validator.ValidateCurrent(this.original, ORIGINAL_NAME);
            Assert.AreEqual(string.Empty, resultMessage, "Name has to be valid, if not changed.");
        }

        internal void ValidateRenamed()
        {
            string resultMessage = this.validator.ValidateCurrent(this.original, UNIQUE_NAME);
            const string MESSAGE = "Name has to be valid, if persistence doesnt contain item with validated name.";
            Assert.AreEqual(string.Empty, resultMessage, MESSAGE);
        }

        internal void ValidateRenamedDuplicit()
        {
            string resultMessage = this.validator.ValidateCurrent(this.original, UNIQUE_NAME);
            const string MESSAGE = "Name cant be valid, there already is another item with that name.";
            Assert.AreEqual(uniqueItemErrorMessage, resultMessage, MESSAGE);
        }

        internal void ValidateNewUniqueName()
        {
            string resultMessage = this.validator.ValidateNew(UNIQUE_NAME);
            const string MESSAGE = "New item Name has to be valid, if persistence doesnt contain item with validated name.";
            Assert.AreEqual(string.Empty, resultMessage, MESSAGE);
        }

        internal void ValidateNotUniqueNewName()
        {
            string resultMessage = this.validator.ValidateNew(ORIGINAL_NAME);
            const string MESSAGE = "New item name is already present in persistence.";
            Assert.AreEqual(uniqueItemErrorMessage, resultMessage, MESSAGE);
        }
    }
}