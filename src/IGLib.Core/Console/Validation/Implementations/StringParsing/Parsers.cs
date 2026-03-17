using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;

namespace IGLib
{

    internal sealed class IdentityStringParser : IStringParser<string>
    {
        public bool TryParse(string text, out string value)
        {
            value = text;
            return true;
        }
    }

    internal sealed class BooleanStringParser : IStringParser<bool>
    {
        public bool TryParse(string text, out bool value) =>
            bool.TryParse(text, out value);
    }


    internal sealed class Int32StringParser : IStringParser<int>
    {
        private readonly IFormatProvider _formatProvider;

        public Int32StringParser(IFormatProvider formatProvider)
        {
            _formatProvider = formatProvider;
        }

        public bool TryParse(string text, out int value) =>
            int.TryParse(text, NumberStyles.Integer, _formatProvider, out value);
    }



    internal sealed class DecimalStringParser : IStringParser<decimal>
    {
        private readonly IFormatProvider _formatProvider;

        public DecimalStringParser(IFormatProvider formatProvider)
        {
            _formatProvider = formatProvider;
        }

        public bool TryParse(string text, out decimal value) =>
            decimal.TryParse(
                text,
                NumberStyles.Number,
                _formatProvider,
                out value);
    }


    internal sealed class DoubleStringParser : IStringParser<double>
    {
        private readonly IFormatProvider _formatProvider;

        public DoubleStringParser(IFormatProvider formatProvider)
        {
            _formatProvider = formatProvider;
        }

        public bool TryParse(string text, out double value) =>
            double.TryParse(
                text,
                NumberStyles.Float | NumberStyles.AllowThousands,
                _formatProvider,
                out value);
    }


}
