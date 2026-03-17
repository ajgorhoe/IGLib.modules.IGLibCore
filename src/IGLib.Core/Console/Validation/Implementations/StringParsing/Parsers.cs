using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;

namespace IGLib
{

    /// <summary>Dummy identity parser - just returns the original string when parsing a string value from a string.</summary>
    internal sealed class IdentityStringParser : IStringParser<string>
    {
        public bool TryParse(string text, out string value)
        {
            value = text;
            return true;
        }
    }

    /// <summary>Parses boolean from a striring.</summary>
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



    internal sealed class DateTimeStringParser : IStringParser<DateTime>
    {
        private readonly IFormatProvider _formatProvider;

        public DateTimeStringParser(IFormatProvider formatProvider)
        {
            _formatProvider = formatProvider;
        }

        public bool TryParse(string text, out DateTime value) =>
            DateTime.TryParse(
                text,
                _formatProvider,
                DateTimeStyles.None,
                out value);
    }


    internal sealed class GuidStringParser : IStringParser<Guid>
    {
        public bool TryParse(string text, out Guid value) =>
            Guid.TryParse(text, out value);
    }


    internal sealed class TimeSpanStringParser : IStringParser<TimeSpan>
    {
        private readonly IFormatProvider _formatProvider;

        public TimeSpanStringParser(IFormatProvider formatProvider)
        {
            _formatProvider = formatProvider;
        }

        public bool TryParse(string text, out TimeSpan value) =>
            TimeSpan.TryParse(text, _formatProvider, out value);
    }




}
