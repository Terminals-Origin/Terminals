using System.ComponentModel.DataAnnotations;

namespace Terminals.Data.Validation
{
    internal class DbBeforeConnectExecuteMetadata
    {
        [StringLength(255, ErrorMessage = Validations.MAX_255_CHARACTERS)]
        public string Command { get; set; }

        [StringLength(255, ErrorMessage = Validations.MAX_255_CHARACTERS)]
        public string CommandArguments { get; set; }

        [StringLength(255, ErrorMessage = Validations.MAX_255_CHARACTERS)]
        public string InitialDirectory { get; set; }
    }
}