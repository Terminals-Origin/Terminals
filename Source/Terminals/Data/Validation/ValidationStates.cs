using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Terminals.Data.Validation
{
    /// <summary>
    /// Encapsulated collection of validation results
    /// </summary>
    internal class ValidationStates : IEnumerable<ValidationState>
    {
        private readonly IEnumerable<ValidationState> results;

        internal bool Empty
        {
            get { return !this.results.Any(); }
        }

        internal ValidationStates(IEnumerable<ValidationState> results)
        {
            this.results = results;
        }

        internal string this[string propertyName]
        {
            get
            {
                IEnumerable<string> messages = this.results
                                                   .Where(result => result.PropertyName == propertyName)
                                                   .Select(result => result.Message);
                return String.Concat(messages);
            }
        }

        public IEnumerator<ValidationState> GetEnumerator()
        {
            return this.results.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public override string ToString()
        {
            return string.Format("ValidationStates:{0}", this.results.Count());
        }
    }
}