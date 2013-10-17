using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        
        internal ValidationStates(IEnumerable<ValidationState> results)
        {
            this.results = results;
        }

        internal string ToOneMessage()
        {
            var builder = new StringBuilder();
            foreach (ValidationState result in results)
            {
                builder.AppendFormat("{0}: {1}\r\n", result.PropertyName, result.Message);
            }

            return builder.ToString();
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