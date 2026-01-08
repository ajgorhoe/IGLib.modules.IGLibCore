
// #nullable disable

using IG.Lib;
using IGLib.Core;
using IGLib.Types.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IGLib.Commands
{

    /// <summary>This command returns product of its arguments.</summary>
    public class CommandProduct : GenericCommandBase
    {

        public CommandProduct(string? description = null, string? descriptionUrl = null) :
            base((object?[]? parameters) =>
            {
                double product = 1.0;
                if (parameters != null)
                {
                    for (int i = 0; i < parameters.Length; ++i)
                    {
                        if (parameters[i] is null)
                            continue;
                        double? d2 = UtilTypes.ConvertTo<double>(parameters[i], precise: true);
                        product *= d2 ?? 1.0;
                    }
                }
                return (object?)product;
            }, description, descriptionUrl)
        {  }

    }


    /// <summary>This command returns sum of its arguments.</summary>
    public class CommandSum : GenericCommandBase
    {

        public CommandSum(string? description = null, string? descriptionUrl = null) :
            base((object?[]? parameters) =>
            {
                double sum = 0.0;
                if (parameters != null)
                {
                    for (int i = 0; i < parameters.Length; ++i)
                    {
                        if (parameters[i] is null)
                            continue;
                        double? d2 = UtilTypes.ConvertTo<double>(parameters[i], precise: true);
                        sum += d2 ?? 0.0;
                    }
                }
                return (object?)sum;
            }, description, descriptionUrl)
        {  }

    }


    /// <summary>This command returns average of its arguments. If no arguments are specified, 0.0 is returned.</summary>
    public class CommandAverage : GenericCommandBase
    {

        public CommandAverage(string? description = null, string? descriptionUrl = null) :
            base((object?[]? parameters) =>
            {
                double sum = 0.0;
                double numElements = 0.0;
                if (parameters != null)
                {
                    for (int i = 0; i < parameters.Length; ++i)
                    {
                        if (parameters[i] is null)
                            continue;
                        double? d2 = UtilTypes.ConvertTo<double>(parameters[i], precise: true);
                        sum += d2 ?? 0.0;
                        if (d2 != null)
                        {
                            numElements += 1.0;
                        }
                    }
                }
                return (object?)(sum / numElements);
            }, description, descriptionUrl)
        {  }

    }

}
